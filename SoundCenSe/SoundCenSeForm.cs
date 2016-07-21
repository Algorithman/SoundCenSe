// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundCenSeForm.cs
// 
// Last modified: 2016-07-21 22:01

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
            soundPanel.SoundDisabled += SoundDisabled;
            FillDisabledSounds();
        }


        private void AddProgress(long expectedSize)
        {
            toolStripProgressBar1.Value += (int) expectedSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DFStopped(this, new DwarfFortressStoppedEventArgs());
            DF.Stop();
            listBox1.Items.Clear();
            toolStripProgressBar1.Visible = true;
            EnableControls(false);
            PackDownloader PD = new PackDownloader();
            PD.FinishedFile += FinishedSingleUpdateFile;
            PD.UpdateFinished += UpdateFinished;
            PD.DownloadStarted += DownloadStarted;
            PD.UpdateSoundPack();

            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = PD.Count();
            PD.Start();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }


        public void DFRunning(object sender, DwarfFortressRunningEventArgs e)
        {
            if (closing)
            {
                return;
            }
            allSounds = new SoundsXML(Config.Instance.soundpacksPath);
            SetDisabledSounds();
            List<string> channelNames = new List<string>();
            channelNames.Add("SFX");
            foreach (Sound s in allSounds.Sounds)
            {
                if (channelNames.Contains(s.Channel))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(s.Channel))
                {
                    channelNames.Add(s.Channel);
                }
            }

            soundPanel.InvokeIfRequired(() =>
            {
                soundPanel.Clear();
                soundPanel.FillEntries(channelNames);
            });

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
            Config.Instance.gamelogPath = DF.GameLogPath;
            Dictionary<string, Sound> oldMusics = GetOldMusic(allSounds);
            allSounds = new SoundsXML(Config.Instance.soundpacksPath);
            SP = new SoundProcessor(allSounds, PM);
            SetDisabledSounds();
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

        private void DownloadStarted(object sender, StartDownloadEventArgs e)
        {
            Task.Factory.StartNew(() =>
                listBox1.InvokeIfRequired(
                    () => listBox1.Items.Add("Download of " + Path.GetFileName(e.File.SourceURL) + " started")));
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

        private void FillDisabledSounds()
        {
            foreach (string disabled in Config.Instance.disabledSounds)
            {
                listBox2.Items.Add(disabled);
            }
        }

        private void FinishedSingleUpdateFile(object sender, DownloadFinishedEventArgs downloadFinishedEventArgs)
        {
            this.InvokeIfRequired(
                () =>
                {
                    listBox1.Items.Add("Downloaded " + Path.GetFileName(downloadFinishedEventArgs.File.DestinationPath));
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

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var index = listBox2.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBox2.SelectedIndex = index;
                }
            }
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

            string soundFile = soundPlayingEventArgs.SoundFile.Filename;
            int soundLength = soundPlayingEventArgs.SoundFile.Cache.AudioData.Length/
                              (Constants.AudioChannels*Constants.Samplerate/1000);
            this.InvokeIfRequired(
                () =>
                {
                    soundPanel.SetValues(channel, soundFile, soundLength, soundPlayingEventArgs.Mute,
                        soundPlayingEventArgs.Volume);
                });
        }

        private void SetDisabledSounds()
        {
            foreach (Sound s in allSounds.Sounds)
            {
                foreach (SoundFile sf in s.SoundFiles)
                {
                    foreach (string disabledSound in Config.Instance.disabledSounds)
                    {
                        if (sf.Filename == disabledSound)
                        {
                            sf.Disabled = true;
                        }
                    }
                }
            }
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

        private void SoundDisabled(object sender, DisableSoundEventArgs disableSoundEventArgs)
        {
            if (!Config.Instance.disabledSounds.Contains(disableSoundEventArgs.Filename))
            {
                listBox2.Items.Add(disableSoundEventArgs.Filename);
                Config.Instance.disabledSounds.Add(disableSoundEventArgs.Filename);
                foreach (Sound s in allSounds.Sounds)
                {
                    foreach (SoundFile sf in s.SoundFiles)
                    {
                        if (sf.Filename == disableSoundEventArgs.Filename)
                        {
                            sf.Disabled = true;
                        }
                    }
                }
            }
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

        private void reenableSoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = listBox2.SelectedItem.ToString();
            if (Config.Instance.disabledSounds.Contains(file))
            {
                listBox2.Items.Remove(file);
                Config.Instance.disabledSounds.Remove(file);
                foreach (Sound s in allSounds.Sounds)
                {
                    foreach (SoundFile sf in s.SoundFiles)
                    {
                        if (sf.Filename == file)
                        {
                            sf.Disabled = false;
                        }
                    }
                }
            }
        }
    }
}