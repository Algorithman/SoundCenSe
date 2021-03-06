﻿using System;
using Misc;
using System.Collections.Generic;
using FMOD;
using System.Linq;
using NLog;

namespace SoundCenSeGTK
{
    public class fmodPlayer : IDisposable, IPlayerManager
    {
        #region Fields and Constants
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static fmodPlayer instance;

        private readonly Dictionary<string, ChannelGroup> channelGroups = new Dictionary<string, ChannelGroup>();
        private Random rnd = new Random();

        public EventHandler<SoundPlayingEventArgs> SoundPlaying;

        #endregion

        #region Properties

        public static fmodPlayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new fmodPlayer(Config.Instance.Channels.Select(x => x.Channel).ToList());
                }
                return instance;
            }
        }

        #endregion

        public fmodPlayer(List<string> channelNames)
        {
            foreach (string channelName in channelNames)
            {
                ChannelGroup cg;
                FmodSystem.ERRCHECK(FmodSystem.System.createChannelGroup(channelName, out cg));
                channelGroups.Add(channelName, cg);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IPlayerManager Members

        public Threshold Threshold { get; set; }

        public float Volume { get; set; }

        public void Play(ISound sound, long x, long y, long z)
        {
            SoundSoundFile sf = new SoundSoundFile((Sound)sound, ((Sound)sound).GetRandomSoundFile());
            if ((sf.SoundFile == null) && (sf.Sound.loop==Loop.Start_Looping))
            {
                return;
            }
            PlaySound(sf, sound.Channel);
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ChannelGroup cg in channelGroups.Values)
                {
                    cg.stop();
                    cg.release();
                }
            }
        }

        public void FastForward(string channel)
        {
            fmodChannelSound fcs = FmodChannelPool.Instance.GetSingleChannel(channel.ToLower());
            if (fcs != null)
            {
                fcs.Channel.stop();
            }
        }

        private void LoopSound(object sender, RestartSoundLoopEventArgs restartSoundLoopEventArgs)
        {
            fmodChannelSound fmc = (fmodChannelSound)sender;
            SoundSoundFile ssf = new SoundSoundFile((Sound)fmc.SoundSoundFile.Sound, (SoundFile)fmc.SoundSoundFile.SoundFile);

            if (((Sound)ssf.Sound).SoundFiles.Count > 1)
            {
                SoundFile sf = (SoundFile)ssf.SoundFile;
                if (((Sound)ssf.Sound).SoundFiles.Count(x => x.Disabled) < ((Sound)ssf.Sound).SoundFiles.Count() - 1)
                {
                    while (sf == ssf.SoundFile)
                    {
                        sf = ((Sound)ssf.Sound).GetRandomSoundFile();
                    }
                }
                else
                {
                    sf = ((Sound)ssf.Sound).GetRandomSoundFile();
                }
                ssf.SoundFile = sf;
                string channelname = fmc.ChannelName;
                fmc.Dispose();
                PlaySound(ssf, channelname);
            }
        }

        public void MuteChannel(string channel, bool mute)
        {
            if (string.IsNullOrEmpty(channel))
            {
                channel = "sfx";
            }
            ChannelGroup cg = null;
            if (channelGroups.ContainsKey(channel.ToLower()))
            {
                cg=channelGroups[channel.ToLower()];
                cg.setMute(mute);
                Config.Instance.GetChannelData(channel.ToLower()).Mute=mute;
            }
        }

        private void OnPlaySound(string channel, SoundSoundFile sf, float channelVolume, bool channelMute)
        {
            var handler = SoundPlaying;
            if (handler != null)
            {
                handler(this, new SoundPlayingEventArgs(sf.Sound, sf.SoundFile, channelMute, channelVolume));
            }
        }

        public void PlaySound(SoundSoundFile sf, string channel, float volume = 1.0f, bool mute = false)
        {
            FMOD.Sound newSound;
            if (sf.Sound.loop == Loop.Stop_Looping)
            {
                if (!string.IsNullOrEmpty(channel))
                {
                    fmodChannelSound chan = FmodChannelPool.Instance.GetSingleChannel(channel.ToLower());
                    if (chan != null)
                    {
                        chan.SoundSoundFile = sf;
                        chan.StopLooping();
                    }
                }
            }

            if (sf.SoundFile == null)
            {
                sf.SoundFile = ((Sound)sf.Sound).GetRandomSoundFile();
            }
            if (sf.SoundFile == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(channel))
            {
                channel = "sfx";
            }
            if (sf.Sound.PlaybackThreshold > (long)Config.Instance.playbackThreshold)
            {
                return;
            }
            int concurrency = FmodChannelPool.Instance.ConcurrentSounds("sfx");

            if (channel.ToLower() != "sfx")
            {
                if (FmodChannelPool.Instance.ConcurrentSounds(channel) > 0)
                {
                    // Already running music?
                    fmodChannelSound oldfmc = FmodChannelPool.Instance.GetSingleChannel(channel.ToLower());
                    if (oldfmc != null)
                    {
                        oldfmc.LoopSound -= LoopSound;
                        oldfmc.Dispose();
                    }
                }
            }
            if ((sf.Sound.Concurrency != -1) && (sf.Sound.Concurrency <= concurrency))
            {
                return;
            }

            if (FmodChannelPool.Instance.IsSoundPlaying(sf.SoundFile.Filename))
            {
                return;
            }

            FmodSystem.ERRCHECK(FmodSystem.System.createSound(sf.SoundFile.Filename, MODE.DEFAULT | MODE.CREATESTREAM,
                out newSound));

            uint soundLength = 0;
            FmodSystem.ERRCHECK(newSound.getLength(out soundLength, TIMEUNIT.MS));
            sf.SoundFile.Length = soundLength;
            sf.fmodSound = newSound;
            Channel ch;
            FmodSystem.ERRCHECK(FmodSystem.System.playSound(newSound, channelGroups[channel], true, out ch));
            if (sf.Sound.Delay > 0)
            {
                float freq;
                FmodSystem.ERRCHECK(ch.getFrequency(out freq));
                ulong dspC1;
                ulong dspC2;
                FmodSystem.ERRCHECK(ch.getDSPClock(out dspC1, out dspC2));
                ulong newDelay = Convert.ToUInt64(sf.Sound.Delay * freq / 500);

                    FmodSystem.ERRCHECK(ch.setDelay(newDelay + dspC1, dspC2 + newDelay, false));
            }
            /*
            if (rnd.Next(5000) > 2000)
            {
             
                FMOD.DSP dsp;
                FmodSystem.System.createDSPByType(DSP_TYPE.PAN,out dsp);
                dsp.setParameterInt((int) DSP_PAN.MODE, (int) DSP_PAN_MODE_TYPE.STEREO);
                dsp.setParameterFloat((int) DSP_PAN.STEREO_POSITION, (rnd.Next(5000)/5000 - 0.5f)*100.0f);

                ch.addDSP(0, dsp);
             
            }
            */
            float channelVolume = Config.Instance.GetChannelData(channel.ToLower()).Volume;
            bool channelMute = Config.Instance.GetChannelData(channel.ToLower()).Mute;
            fmodChannelSound fmc = new fmodChannelSound(ch, channel, volume, channelMute);
            fmc.SoundSoundFile = sf;
            fmc.LoopSound += LoopSound;
            fmc.SoundFinished += SoundFinished;
            fmc.Mute = false;
            fmc.Start();
            fmc.Volume = channelVolume * volume;
            fmc.Mute = channelMute;
            OnPlaySound(channel, sf, channelVolume, channelMute);
        }

        private void SoundFinished(object sender, SoundFinishedEventArgs soundFinishedEventArgs)
        {
            fmodChannelSound fmc = (fmodChannelSound)sender;
            if (fmc != null)
            {
                fmc.Dispose();
            }
        }

        public void StopAll()
        {
            foreach (var c in FmodChannelPool.Instance.AllChannels())
            {
                c.LoopSound -= LoopSound;
            }
            FmodChannelPool.Instance.StopAll();
        }

        public void VolumeChannel(string channel, float volume)
        {
            channelGroups[channel.ToLower()].setVolume(volume);
        }
    }
}

