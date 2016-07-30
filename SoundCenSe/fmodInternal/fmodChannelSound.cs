// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: fmodChannelSound.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Collections.Generic;
using FMOD;
using SoundCenSe.Enums;
using SoundCenSe.Events;

namespace SoundCenSe.fmodInternal
{
    public class fmodChannelSound : IDisposable
    {
        #region Fields and Constants

        private static int counter;

        public static Queue<Sound> SoundsToFree = new Queue<Sound>();

        private readonly CHANNEL_CALLBACK cb;

        private int id;

        public EventHandler<RestartSoundLoopEventArgs> LoopSound;
        private bool mute;

        public EventHandler<SoundFinishedEventArgs> SoundFinished;

        private float volume;

        #endregion

        #region Properties

        public Channel Channel
        {
            get
            {
                Channel ch;
                FmodSystem.System.getChannel(Id, out ch);
                if (ch.isValid())
                {
                    return ch;
                }
                return null;
            }
        }

        public string ChannelName { get; set; }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Mute
        {
            get { return mute; }
            set
            {
                if (value != mute)
                {
                    mute = value;
                    Channel ch;
                    FmodSystem.System.getChannel(Id, out ch);
                    if (ch.isValid())
                    {
                        ch.setMute(mute);
                    }
                }
            }
        }

        public Channel Raw { get; set; }
        public SoundSoundFile SoundSoundFile { get; set; }

        public float Volume
        {
            get { return volume; }
            set
            {
                if (volume != value)
                {
                    volume = value;
                    Channel ch;
                    FmodSystem.System.getChannel(Id, out ch);
                    if (ch.isValid())
                    {
                        ch.setVolume(value);
                    }
                }
            }
        }

        #endregion

        public fmodChannelSound(Channel channel, string channelName, float volume = 1.0f, bool mute = false)
        {
            Raw = channel;
            channel.getIndex(out id);
            Volume = volume;
            Mute = mute;
            ChannelName = channelName;
            cb = Callback;

            channel.setCallback(cb);
            FmodChannelPool.Instance.RegisterChannel(this);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private RESULT Callback(IntPtr channelraw, CHANNELCONTROL_TYPE controltype, CHANNELCONTROL_CALLBACK_TYPE type,
            IntPtr commanddata1, IntPtr commanddata2)
        {
            if (Raw.isValid())
            {
                if (channelraw == Raw.getRaw())
                {
                    if (controltype == CHANNELCONTROL_TYPE.CHANNEL)
                    {
                        if (type == CHANNELCONTROL_CALLBACK_TYPE.END)
                        {
                            // Console.WriteLine("Callback called for " + SoundSoundFile.SoundFile.Filename);
                            //Console.WriteLine("System ptr: " + FmodSystem.System.getRaw());
                            //Console.WriteLine("Thread id: " + Thread.CurrentThread.ManagedThreadId + " " + Thread.CurrentThread.IsThreadPoolThread);
                            OnSoundEnded();
                            //Console.WriteLine("System ptr: " + FmodSystem.System.getRaw());
                        }
                    }
                }
            }
            return RESULT.OK;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
                Channel ch;
                RESULT r = FmodSystem.System.getChannel(Id, out ch);
                if (ch.isValid())
                {
                    if (SoundSoundFile.fmodSound != null)

                    {
                        Sound s;
                        r = ch.getCurrentSound(out s);
                        SoundsToFree.Enqueue(s);
                    }
                    FmodChannelPool.Instance.UnregisterChannel(this);
                }
            }
        }

        public bool isPlaying(SoundSoundFile currentlyPlaying)
        {
            bool isPlaying = false;
            Channel.isPlaying(out isPlaying);
            if (isPlaying && (currentlyPlaying.Sound == SoundSoundFile.Sound) &&
                (currentlyPlaying.SoundFile == SoundSoundFile.SoundFile))
            {
                return true;
            }
            return false;
        }

        protected virtual void OnSoundEnded()
        {
            if (SoundSoundFile.Sound.loop == Loop.Stop_Looping)
            {
                var handler = SoundFinished;
                if (handler != null)
                {
                    handler(this, new SoundFinishedEventArgs(SoundSoundFile.Sound, SoundSoundFile.SoundFile));
                }
            }
            else
            {
                var handler2 = LoopSound;
                if (handler2 != null)
                {
                    handler2(this, new RestartSoundLoopEventArgs(SoundSoundFile));
                }
            }
        }

        public static void ReleaseSounds()
        {
            lock (SoundsToFree)
            {
                while (SoundsToFree.Count > 0)
                {
                    Sound s = SoundsToFree.Dequeue();
                    if (s.isValid())
                    {
                        counter++;
                        RESULT r = s.release();
                    }
                }
            }
            GC.Collect();
        }

        public void Start()
        {
            Channel ch;
            FmodSystem.System.getChannel(Id, out ch);
            if (ch.isValid())
            {
                ch.setPaused(false);
            }
        }

        public void Stop()
        {
            //Console.WriteLine("Stop: Getting Channel");
            Channel ch;
            RESULT r = FmodSystem.System.getChannel(Id, out ch);
            if (ch.isValid())
            {
                //  Console.WriteLine("Result getChannel: " + r);
                Sound s;
                r = ch.getCurrentSound(out s);
                //Console.WriteLine("Result getCurrentSound: " + r);
                bool isPlaying = false;
                if (ch.isValid())
                {
                    r = ch.isPlaying(out isPlaying);
                    //  Console.WriteLine("Result IsPlaying: " + r + " " + isPlaying);
                }
                if (isPlaying)
                {
                    if (ch.isValid())
                    {
                        r = ch.stop();
                        //  Console.WriteLine("Result Stop: " + r);
                    }
                }
            }
        }

        public void StopLooping()
        {
        }
    }
}