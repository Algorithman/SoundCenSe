// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: SoundCenSeForm.cs
// 
// Last modified: 2016-07-17 22:06

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SoundCenSe.Configuration;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Events;
using SoundCenSe.Input;
using SoundCenSe.Output;
using SoundCenSe.Utility;
using SoundCenSe.Utility.Updater;

namespace SoundCenSe
{
    public partial class SoundCenSeForm : Form
    {
        #region Fields and Constants

        private SoundsXML allSounds;

        private bool closing;
        private readonly DwarfFortressAware DF;
        private LogFileListener LL;
        private PlayerManager PM;
        private SoundProcessor SP;

        readonly List<Control> wereEnabled = new List<Control>();

        #endregion

        public SoundCenSeForm()
        {
            InitializeComponent();

            // Load Configuration
            Config.Load(@"Configuration.json");
            PM = new PlayerManager();
            DF = new DwarfFortressAware();
            DF.DwarfFortressRunning += DFRunning;
            DF.DwarfFortressStopped += DFStopped;
            DF.Start();
            PM.Playing += Playing;
            soundPanel.Muting += Muting;
            soundPanel.FastForward += FastForward;
            soundPanel.VolumeChanged += VolumeChanged;
        }


        private void AddProgress(long expectedSize)
        {
            toolStripProgressBar1.Value += (int) expectedSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DFStopped(this, new DwarfFortressStoppedEventArgs());
            DF.Stop();
            toolStripProgressBar1.Visible = true;
            EnableControls(false);
            PackDownloader PD = new PackDownloader();
            PD.FinishedFile += FinishedSingleUpdateFile;
            PD.UpdateFinished += UpdateFinished;
            PD.UpdateSoundPack();

            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = PD.Count();
            PD.Start();
        }

        public void DFRunning(object sender, DwarfFortressRunningEventArgs e)
        {
            if (closing)
            {
                return;
            }
            if (PM != null)
            {
                PM.Dispose();
                PM = null;
            }
            PM = new PlayerManager();
            PM.Playing += Playing;
            if (LL != null)
            {
                LL.Stop();
                LL.Dispose();
            }
            allSounds = new SoundsXML(Config.Instance.soundpacksPath);
            Config.Instance.gamelogPath = DF.GameLogPath;
            SP = new SoundProcessor(new SoundsXML(Config.Instance.soundpacksPath), PM);
            Dictionary<string, Sound> oldMusics = GetOldMusic(allSounds);
            if (oldMusics.ContainsKey("music"))
            {
                PM.Play(oldMusics["music"], 0, 0, 0);
            }
            if (oldMusics.ContainsKey("weather"))
            {
                PM.Play(oldMusics["weather"], 0, 0, 0);
            }

            LL = new LogFileListener(DF.GameLogPath);
            LL.GamelogEvent += SP.ProcessLine;
            LL.GamelogEvent += ShowLogInStatus;
            LL.BeginAtEnd();
            LL.Start();
            this.InvokeIfRequired(() =>
            {
                toolStripSignal1.Signal = true;
                Status("Connected to Dwarf Fortress in " + Path.GetDirectoryName(DF.GameLogPath));
                btnUpdate.Enabled = false;
            });
        }

        public void DFStopped(object sender, DwarfFortressStoppedEventArgs e)
        {
            if (closing)
            {
                return;
            }
            if (LL != null)
            {
                LL.Dispose();
            }
            if (PM != null)
            {
                PM.Dispose();
            }
            LL = null;
            PM = null;
            SP = null;
            this.InvokeIfRequired(() =>
            {
                toolStripSignal1.Signal = false;
                btnUpdate.Enabled = true;
                soundPanel.Clear();
            });
        }

        /// <summary>
        ///     Enable or disable all Controls
        /// </summary>
        /// <param name="enable">Whether enable (true) or disable (false)</param>
        private void EnableControls(bool enable)
        {
            if (enable == false)
            {
                wereEnabled.Clear();
            }
            this.InvokeIfRequired(() =>
            {
                foreach (Control c in this.Controls)
                {
                    if (!enable && c.Enabled)
                    {
                        wereEnabled.Add(c);
                        c.Enabled = enable;
                    }
                    else
                    {
                        if (wereEnabled.Contains(c))
                        {
                            c.Enabled = enable;
                        }
                    }
                }
            });
        }

        private void FastForward(object sender, ChannelFastForwardEventArgs channelFastForwardEventArgs)
        {
            if (closing)
            {
                return;
            }
            PM.FastForward(channelFastForwardEventArgs.Channel);
        }

        private void FinishedSingleUpdateFile(object sender, DownloadFinishedEventArgs downloadFinishedEventArgs)
        {
            this.InvokeIfRequired(
                () =>
                {
                    Status("Downloaded " + Path.GetFileName(downloadFinishedEventArgs.File.DestinationPath));
                    AddProgress(downloadFinishedEventArgs.File.ExpectedSize);
                });
        }

        private Dictionary<string, Sound> GetOldMusic(SoundsXML allSounds)
        {
            DummyPlayerManager dpm = new DummyPlayerManager();
            DummySoundProcessor sp = new DummySoundProcessor(allSounds, dpm);

            FileStream fs = new FileStream(Config.Instance.gamelogPath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite);
            TextReader tr = new StreamReader(fs);

            List<string> lines = new List<string>();

            string line = "";
            while ((line = tr.ReadLine()) != null)
            {
                if (line == "*** STARTING NEW GAME ***")
                {
                    lines.Clear();
                }
                lines.Add(line);
            }

            foreach (string l in lines)
            {
                sp.ProcessLine(this, new GamelogEventArgs(l));
            }
            return dpm.Channels;
        }

        private void Muting(object sender, ChannelMuteEventArgs channelMuteEventArgs)
        {
            if (closing)
            {
                return;
            }
            string ch = channelMuteEventArgs.Channel.ToLower().StartsWith("sfx")
                ? "sfx"
                : channelMuteEventArgs.Channel.ToLower();
            PM.MuteChannel(ch, channelMuteEventArgs.Mute);
            Config.Instance.MuteChannel(ch, channelMuteEventArgs.Mute);
        }

        private void Playing(object sender, SoundPlayingEventArgs soundPlayingEventArgs)
        {
            if (closing)
            {
                return;
            }
            string channel = soundPlayingEventArgs.Sound.Channel.Capitalize();
            if (string.IsNullOrEmpty(channel))
            {
                channel = "SFX";
            }

            string soundFile = Path.GetFileNameWithoutExtension(soundPlayingEventArgs.SoundFile.Filename);
            int soundLength = soundPlayingEventArgs.SoundFile.Cache.AudioData.Length/
                              (Constants.AudioChannels*Constants.Samplerate/1000);
            this.InvokeIfRequired(
                () =>
                {
                    soundPanel.SetValues(channel, soundFile, soundLength, soundPlayingEventArgs.Mute,
                        soundPlayingEventArgs.Volume);
                });
        }

        private void ShowLogInStatus(object sender, GamelogEventArgs gamelogEventArgs)
        {
            this.InvokeIfRequired(() => Status(gamelogEventArgs.Line));
        }

        private void SoundCenSeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DF != null)
            {
                DF.Stop();
            }
            if (PM != null)
            {
                PM.Stop();
            }
            if (LL != null)
            {
                LL.Stop();
            }
            Config.Save("Configuration.json");
        }

        private void SoundCenSeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            closing = true;
            e.Cancel = false;
        }

        private void Status(string line)
        {
            statusStrip1.InvokeIfRequired(() =>
            {
                if (statusStrip1.Visible && toolStripStatusLabel1.Visible)
                {
                    toolStripStatusLabel1.Text = line;
                }
            });
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            soundPanel.Tick();
        }

        private void UpdateFinished(object sender, UpdateFinishedEventArgs updateFinishedEventArgs)
        {
            EnableControls(true);
            this.InvokeIfRequired(() => toolStripProgressBar1.Visible = false);
            DF.Start();
        }

        private void VolumeChanged(object sender, ChannelVolumeEventArgs channelVolumeEventArgs)
        {
            if (closing)
            {
                return;
            }
            string ch = channelVolumeEventArgs.Channel.ToLower().StartsWith("sfx")
                ? "sfx"
                : channelVolumeEventArgs.Channel.ToLower();
            PM.ChannelVolume(ch, channelVolumeEventArgs.Volume);
            Config.Instance.SetChannelVolume(ch, channelVolumeEventArgs.Volume);
        }
    }
}