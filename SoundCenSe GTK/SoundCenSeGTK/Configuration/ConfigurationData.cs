using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Misc;

namespace SoundCenSeGTK
{
    public class ConfigurationData
    {
        #region Properties

        public bool achievements { get; set; }

        public bool allowPackOverride { get; set; }

        public List<string> autoUpdateURLs { get; set; }

        public List<ChannelData> Channels { get; set; }

        public bool deleteFiles { get; set; }

        public HashSet<string> disabledSounds { get; set; }

        public long expectedMinimalSoundsCound { get; set; }

        public string fileName { get; set; }

        public string gamelogEncoding { get; set; }

        public string gamelogPath { get; set; }

        public bool gui { get; set; }

        public HashSet<string> mutedChannels { get; set; }

        public bool noWarnAbsolutePaths { get; set; }

        public Threshold playbackThreshold { get; set; }

        public bool replaceFiles { get; set; }

        public string soundpacksPath { get; set; }

        public List<string> supplementalLogs { get; set; }

        public float volume { get; set; }

        #endregion

        public ConfigurationData()
        {
            autoUpdateURLs = new List<string>();
            disabledSounds = new HashSet<string>();
            mutedChannels = new HashSet<string>();
            supplementalLogs = new List<string>();
            Channels = new List<ChannelData>();
        }

        public ChannelData GetChannelData(string channel)
        {
            ChannelData cx = Channels.FirstOrDefault(x => x.Channel == channel);
            if (cx == null)
            {
                // Dummy if not in list
                cx = new ChannelData();
                cx.Volume = 1.0f;
                cx.Mute = false;
                cx.Channel = "";
            }
            return cx;
        }

        public static ConfigurationData Load(string filename)
        {
            using (TextReader tr = new StreamReader(filename))
            {
                return JsonConvert.DeserializeObject<ConfigurationData>(tr.ReadToEnd());
            }
        }

        public void MuteChannel(string channel, bool mute)
        {
            foreach (ChannelData cd in Channels)
            {
                if (cd.Channel == channel)
                {
                    cd.Mute = mute;
                    return;
                }
            }
            ChannelData cdNew = new ChannelData();
            cdNew.Channel = channel;
            cdNew.Mute = mute;
            cdNew.Volume = 1.0f;
            Channels.Add(cdNew);
        }

        public void Save(string filename)
        {
            using (TextWriter tw = new StreamWriter(filename))
            {
                tw.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }

        public void SetChannelVolume(string channel, float vol)
        {
            foreach (ChannelData cd in Channels)
            {
                if (cd.Channel == channel)
                {
                    cd.Volume = vol;
                    return;
                }
            }
            ChannelData cdNew = new ChannelData();
            cdNew.Channel = channel;
            cdNew.Mute = false;
            cdNew.Volume = vol;
            Channels.Add(cdNew);
        }
    }
}

