// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ChannelThread.cs
// 
// Last modified: 2016-07-24 13:52

using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SoundCenSe.Audio;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Utility
{
    public class ChannelThread : IStoppable
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string channelName = "";
        private ChannelSound currentlyPlaying;
        private readonly MyMixingSampleProvider mixer;
        private bool mute;

        public EventHandler<SoundPlayingEventArgs> SoundPlaying;
        private bool stop;
        private float volume = 1.0f;

        #endregion

        #region Properties

        private SoundFile CurrentMusic { get; set; }

        public Sound Looping { get; set; }

        public bool Mute
        {
            get { return mute; }
            set
            {
                logger.Info("Muting " + value + " for channel " + channelName);
                mute = value;
                if (currentlyPlaying != null)
                {
                    currentlyPlaying.Mute = value;
                }
            }
        }

        public Sound Singular { get; set; }

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (currentlyPlaying != null)
                {
                    currentlyPlaying.Volume = value;
                }
            }
        }

        #endregion

        public ChannelThread(string channelName, MyMixingSampleProvider mixer)
        {
            this.channelName = channelName;
            this.mixer = mixer;
            volume = Config.Instance.GetChannelData(channelName.ToLower()).Volume;
            mute = Config.Instance.GetChannelData(channelName.ToLower()).Mute;
        }

        #region IStoppable Members

        public void Start()
        {
            logger.Warn("ChannelThread " + channelName + " started");
            stop = false;
            Task.Factory.StartNew(() => DoWork());
        }

        public void Stop()
        {
            logger.Warn("ChannelThread " + channelName + " stopped");
            stop = true;
        }

        #endregion

        private void DoWork()
        {
            long delay = 0;
            Sound playingSound = null;
            SoundFile playingSoundFile = null;
            stop = false;
            while (!stop)
            {
                if ((currentlyPlaying == null) || (!mixer.IsPlaying(currentlyPlaying)))
                {
                    if ((Singular != null) || (Looping != null))
                    {
                        if (CurrentMusic != null)
                        {
                            CurrentMusic.Cache = null;
                        }
                        CurrentMusic = null;
                        if (Singular != null)
                        {
                            CurrentMusic = Singular.GetRandomSoundFile();
                            playingSound = Singular;
                            playingSoundFile = CurrentMusic;
                            delay = Singular.Delay;
                        }

                        Singular = null;

                        if ((CurrentMusic == null) && (Looping != null))
                        {
                            CurrentMusic = Looping.GetRandomSoundFile();
                            playingSound = Looping;
                            playingSoundFile = CurrentMusic;
                            delay = Looping.Delay;
                            logger.Info("Channel looping " + channelName + ": " + CurrentMusic);
                        }
                        logger.Info("Channel playing " + CurrentMusic);

                        if (currentlyPlaying != null)
                        {
                            mixer.RemoveMixerInput(currentlyPlaying);
                            currentlyPlaying = null;
                        }

                        if (delay > 0)
                        {
                            logger.Info("Delaying channel " + channelName + " playback by " + delay + " ms.");
                            Thread.Sleep((int) delay);
                            delay = 0;
                        }
                        if (CurrentMusic != null)
                        {
                            CurrentMusic.GenerateCache(mixer.WaveFormat);
                            currentlyPlaying = mixer.Add(CurrentMusic, Volume, 0.0f, playingSound.Channel);
                            currentlyPlaying.Mute = mute;
                            Task.Factory.StartNew(() => OnPlayingSound(playingSound, playingSoundFile));
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            if (currentlyPlaying != null)
            {
                mixer.RemoveMixerInput(currentlyPlaying);
            }
        }

        internal ChannelSound GetPlaying()
        {
            return currentlyPlaying;
        }

        private void OnPlayingSound(Sound playingSound, SoundFile playingSoundFile)
        {
            var handler = SoundPlaying;
            if (handler != null)
            {
                handler(this, new SoundPlayingEventArgs(playingSound, playingSoundFile, Mute, Volume));
            }
        }
    }
}