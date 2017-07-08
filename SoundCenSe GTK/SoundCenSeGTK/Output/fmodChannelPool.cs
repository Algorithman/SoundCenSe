using System;
using System.Collections.Generic;
using System.Linq;
using Misc;
using NLog;

namespace SoundCenSeGTK
{
    public class FmodChannelPool
    {
        #region Fields and Constants

        private static FmodChannelPool instance;
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<KeyValuePair<int, string>, fmodChannelSound> channels =
            new Dictionary<KeyValuePair<int, string>, fmodChannelSound>();

        #endregion

        #region Properties

        public static FmodChannelPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FmodChannelPool();
                }
                return instance;
            }
        }

        #endregion

        public fmodChannelSound GetByRaw(IntPtr raw)
        {
            return channels.Values.SingleOrDefault(x => x.Channel.getRaw() == raw);
        }

        public IEnumerable<fmodChannelSound> AllChannels()
        {
            return channels.Select(x => x.Value);
        }

        public int ConcurrentSounds(string channelname)
        {
            return channels.Count(x => x.Key.Value == channelname);
        }

        public fmodChannelSound GetSingleChannel(string channel)
        {
            if (!channels.Any(x => x.Key.Value == channel.ToLower()))
            {
                return null;
            }
            return channels.SingleOrDefault(x => x.Key.Value == channel.ToLower()).Value;
        }

        public void RegisterChannel(fmodChannelSound channel)
        {
            if (channel.ChannelName.ToLower() != "sfx")
            {
                if (channels.Count(x => x.Key.Value == channel.ChannelName.ToLower()) > 0)
                {
                    return;
                }
            }
            KeyValuePair<int, string> kv = new KeyValuePair<int, string>(channel.Id, channel.ChannelName);
            channels.Add(kv, channel);
        }

        public void StopAll()
        {
            fmodChannelSound[] list;
            lock (channels)
            {
                list = channels.Select(x => x.Value).ToArray();
            }
            foreach (var c in list)
            {
                c.Dispose();
            }
        }


        public void UnregisterChannel(fmodChannelSound channel)
        {
            lock (channels)
            {
                KeyValuePair<int, string> kv = new KeyValuePair<int, string>(channel.Id, channel.ChannelName);
                channels.Remove(kv);
            }
        }

        public bool IsSoundPlaying(string filename)
        {
            List<fmodChannelSound> channelsounds = null;
            lock (channels)
            {
                channelsounds = channels.Values.ToList();
            }
            foreach (var sf in channelsounds)
            {
                if (sf.SoundSoundFile != null)
                {
                    if (sf.SoundSoundFile.SoundFile != null)
                    {
                        if (sf.SoundSoundFile.SoundFile.Filename == filename)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        logger.Log(LogLevel.Fatal, "sf.SoundSoundFile.SoundFile is NULL while looking for " + filename);
                    }
                }
                else
                {
                    logger.Log(LogLevel.Fatal, "sf.SoundSoundFile is NULL while looking for " + filename);
                }
            }
            
            return false;
        }
    }
}

