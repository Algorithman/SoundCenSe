// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: PlayerManager.cs
// 
// Last modified: 2016-07-22 20:04

using System;
using System.Collections.Generic;
using NAudio.Wave;
using NLog;
using SoundCenSe.Audio;
using SoundCenSe.Configuration;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Enums;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;
using SoundCenSe.Utility;

namespace SoundCenSe.Output
{
    public class PlayerManager : IPlayerManager, IDisposable
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly MyMixingSampleProvider channelMixer;

        private readonly IWavePlayer musicOutputDevice;

        public EventHandler<SoundPlayingEventArgs> Playing;
        private readonly SFXManagerThread SFX;

        private readonly MyMixingSampleProvider SFXMixer;

        private readonly IWavePlayer sfxOutputDevice;
        private float volume = 1.0f;

        #endregion

        #region Properties

        private Dictionary<string, ChannelThread> channels { get; } = new Dictionary<string, ChannelThread>();

        #endregion

        public PlayerManager()
        {
            channelMixer =
                new MyMixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(Constants.Samplerate,
                    Constants.AudioChannels));
            SFX = new SFXManagerThread();
            SFXMixer = SFX.mixer;

            SFX.Threshold = Threshold;
            SFX.Volume = Config.Instance.GetChannelData("sfx").Volume;
            SFX.Mute = Config.Instance.GetChannelData("sfx").Mute;
            SFX.SoundPlaying += SoundPlaying;
            SFX.Start();
            musicOutputDevice = new WaveOutEvent();
            musicOutputDevice.Init(channelMixer);
            musicOutputDevice.Play();

            sfxOutputDevice = new WaveOutEvent();
            sfxOutputDevice.Init(SFXMixer);
            sfxOutputDevice.Play();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IPlayerManager Members

        public float Volume
        {
            get { return volume; }
            set
            {
                if (value != volume)
                {
                    volume = value;
                    foreach (ChannelThread ct in channels.Values)
                    {
                        ct.Volume = value;
                    }
                    SFX.Volume = value;
                }
            }
        }

        public Threshold Threshold { get; set; } = Threshold.Everything;

        public void Play(Sound sound, long x, long y, long z)
        {
            if (sound.PlaybackThreshold <= (long) Config.Instance.playbackThreshold)
            {
                if (!string.IsNullOrEmpty(sound.Channel))
                {
                    PlayMusic(sound);
                }
                else
                {
                    SFXQueue.AddSound(sound, x, y, z);
                }
            }
        }

        #endregion

        public void ChannelVolume(string channel, float vol)
        {
            if (channel == "sfx")
            {
                SFX.Volume = vol;
            }
            else
            {
                if (channels.ContainsKey(channel.ToLower()))
                {
                    channels[channel.ToLower()].Volume = vol;
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
                musicOutputDevice.Stop();
                musicOutputDevice.Dispose();
            }
        }

        public void FastForward(string channel)
        {
            if (!channel.ToLower().StartsWith("sfx"))
            {
                if (channels.ContainsKey(channel.ToLower()))
                {
                    ChannelThread ct = channels[channel.ToLower()];
                    channelMixer.RemoveMixerInput(ct.GetPlaying());
                }
            }
        }

        public List<Tuple<string, string, int>> GetSoundData()
        {
            List<Tuple<string, string, int>> result = channelMixer.GetSoundData();
            result.AddRange(SFXMixer.GetSoundData());
            return result;
        }

        public void MuteChannel(string channel, bool mute)
        {
            if (channel == "sfx")
            {
                SFX.Mute = mute;
            }
            else
            {
                if (channels.ContainsKey(channel.ToLower()))
                {
                    ChannelThread ct = channels[channel.ToLower()];
                    ct.Mute = mute;
                }
            }
        }

        private void PlayMusic(Sound sound)
        {
            if (!channels.ContainsKey(sound.Channel))
            {
                channels.Add(sound.Channel, new ChannelThread(sound.Channel, channelMixer));
                channels[sound.Channel].SoundPlaying += Playing;
                channels[sound.Channel].Start();
            }

            ChannelThread ct = channels[sound.Channel];

            if (sound.loop == Loop.Start_Looping)
            {
                ct.Looping = sound;
            }
            else if (sound.loop == Loop.Stop_Looping)
            {
                ct.Looping = null;
                ct.Singular = sound;
            }
            else
            {
                ct.Singular = sound;
            }
        }

        private void SoundPlaying(object sender, SoundPlayingEventArgs soundPlayingEventArgs)
        {
            var handler = Playing;
            if (handler != null)
            {
                handler(this, soundPlayingEventArgs);
            }
        }

        public void Stop()
        {
            foreach (ChannelThread ct in channels.Values)
            {
                ct.Stop();
            }
            channels.Clear();
            SFX.Stop();
        }
    }
}