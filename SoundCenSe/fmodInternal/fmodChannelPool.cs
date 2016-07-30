// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: fmodChannelPool.cs
// 
// Last modified: 2016-07-30 19:37

using System.Collections.Generic;
using System.Linq;

namespace SoundCenSe.fmodInternal
{
    public class FmodChannelPool
    {
        #region Fields and Constants

        private static FmodChannelPool instance;

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

        public IEnumerable<fmodChannelSound> AllChannels()
        {
            return channels.Select(x => x.Value);
        }

        public int ConcurrentSounds(string channelname)
        {
            lock (channels)
            {
                return channels.Count(x => x.Key.Value == channelname);
            }
        }

        public fmodChannelSound GetSingleChannel(string channel)
        {
            lock (channels)
            {
                return channels.Single(x => x.Key.Value == channel.ToLower()).Value;
            }
        }

        public void RegisterChannel(fmodChannelSound channel)
        {
            lock (channels)
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
    }
}