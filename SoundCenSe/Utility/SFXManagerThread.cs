// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: SFXManagerThread.cs
// 
// Last modified: 2016-07-17 22:06

using System;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NLog;
using SoundCenSe.Audio;
using SoundCenSe.Configuration;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Enums;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Utility
{
    public class SFXManagerThread : IStoppable
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private float _volume = 1.0f;
        private int delayedSounds;
        internal MyMixingSampleProvider mixer;
        private bool mute;
        private readonly Random rnd = new Random((int) DateTime.Now.Ticks);


        public VolumeSampleProvider SampleProvider;

        public EventHandler<SoundPlayingEventArgs> SoundPlaying;

        private bool stop;

        #endregion

        #region Properties

        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;
                SampleProvider.Volume = value ? Volume : 0;
            }
        }

        public Threshold Threshold { get; set; }

        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                SampleProvider.Volume = mute ? 0 : value;
            }
        }

        #endregion

        public SFXManagerThread()
        {
            mixer =
                new MyMixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(Constants.Samplerate,
                    Constants.AudioChannels));
            mixer.ReadFully = true;
            SampleProvider = new VolumeSampleProvider(mixer);
        }

        #region IStoppable Members

        public void Start()
        {
            logger.Warn("SFX Manager Thread started");
            Task.Factory.StartNew(() => DoWork());
        }

        public void Stop()
        {
            stop = true;
            mixer.RemoveAllMixerInputs();
            logger.Warn("SFX Manager Thread stopped");
        }

        #endregion

        public void DoWork()
        {
            delayedSounds = 0;
            stop = false;
            while (!stop)
            {
                if (SFXQueue.Count == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }
                Tuple<long, long, long, Sound> data = SFXQueue.GetSound();
                if (data == null)
                {
                    continue;
                }
                Sound sound = data.Item4;
                if (sound.HasNoSoundFiles())
                {
                    logger.Info("Not playing Sound " + sound);
                    continue;
                }

                if ((int) Threshold < sound.PlaybackThreshold)
                {
                    logger.Info("Not playing sound " + sound + " (Threshold too low)");
                    continue;
                }

                if (sound.Timeout > 0)
                {
                    if (sound.LastPlayed < DateTime.UtcNow.Ticks - sound.Timeout)
                    {
                        sound.LastPlayed = DateTime.UtcNow.Ticks;
                    }
                    else
                    {
                        logger.Info("Not playing sound " + sound + " (timeout)");
                        continue;
                    }
                }

                if (sound.Probability > 0)
                {
                    if (rnd.Next(100) > sound.Probability)
                    {
                        logger.Info("Sound " + sound + " dropped due to probability");
                        continue;
                    }
                }

                if ((sound.Concurrency == -1) || (sound.Concurrency > mixer.ConcurrentSounds + delayedSounds))
                {
                    long x = data.Item1;
                    long y = data.Item2;
                    long z = data.Item3;

                    float distVol = 1.0f;
                    double dist = Math.Sqrt(x*x + y*y + z*z);
                    if (dist > 80)
                    {
                        distVol = 0.1f;
                    }
                    else if (dist > 0)
                    {
                        distVol = 1*(0.9f*(float) dist/80.0f);
                    }

                    float balance = 0.0f;
                    if (x < -80)
                    {
                        balance = -1.0f;
                    }
                    else if (x > 80)
                    {
                        balance = 1.0f;
                    }
                    else
                    {
                        balance = x/80.0f;
                    }

                    SoundFile sf = sound.GetRandomSoundFile();
                    long delay = sound.Delay;

                    if (delay > 0)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            delayedSounds++;
                            DateTime toPlay = DateTime.UtcNow + TimeSpan.FromMilliseconds(delay);
                            if (sf.Cache == null)
                            {
                                sf.GenerateCache(SampleProvider.WaveFormat);
                            }
                            ChannelSound cs = new ChannelSound(new CachedSoundSampleProvider(sf.Cache), distVol*Volume,
                                "",
                                sf.Filename,
                                sf.Cache.AudioData.Length/(Constants.AudioChannels*Constants.Samplerate/1000));
                            cs.Mute = mute;
                            Thread.Sleep((int) (toPlay - DateTime.UtcNow).TotalMilliseconds);
                            delayedSounds--;
                            mixer.AddMixerInput(cs);
                            OnPlaySound(sound, sf);
                        });
                    }
                    else
                    {
                        if (sf.Cache == null)
                        {
                            sf.GenerateCache(SampleProvider.WaveFormat);
                        }
                        ChannelSound cs = new ChannelSound(new CachedSoundSampleProvider(sf.Cache), distVol*Volume, "",
                            sf.Filename, sf.Cache.AudioData.Length/(Constants.AudioChannels*Constants.Samplerate/1000));
                        cs.Mute = mute;
                        mixer.AddMixerInput(cs);
                        OnPlaySound(sound, sf);
                    }
                }
                else
                {
                    logger.Info("Not playing sound " + sound + " (" + (mixer.ConcurrentSounds + delayedSounds) +
                                " (" + delayedSounds + ")) sounds running");
                }
            }
        }

        private void OnPlaySound(Sound sound, SoundFile sf)
        {
            var handler = SoundPlaying;
            if (handler != null)
            {
                handler(this, new SoundPlayingEventArgs(sound, sf, Mute, Volume));
            }
        }
    }
}