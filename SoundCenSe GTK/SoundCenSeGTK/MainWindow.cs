using System;
using Gtk;
using SoundCenSeGUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Misc;
using SoundCenSeGTK;

namespace SoundCenSeGTK
{
    public partial class MainWindow: Gtk.Window
    {
        private Dictionary<string,SoundPanelEntry> panelEntries = new Dictionary<string, SoundPanelEntry>();
        private static PackDownloader PD = null;

        public MainWindow()
            : base(Gtk.WindowType.Toplevel)
        {
            Build();
            Config.Load(@"Configuration.json");

            int count = Enum.GetValues(typeof(Threshold)).Length;
            foreach (var v in Enum.GetValues(typeof(Threshold)))
            {
                cbThreshold.AppendText(v.ToString());
            }

            cbThreshold.Active = (int)Config.Instance.playbackThreshold;


            notebook1.Page = 0;
            List<string> temp = new List<string>() { "SFX", "Music", "Weather", "Swords", "Trading" };
            AddSoundPanelEntries(temp);
            ShowAll();

            Running += StartListening;
            Stopped += StopListening;

            // Start main tick timer
            GLib.Timeout.Add(50, Timer);
        }

        private SoundProcessor SP = null;
        private LogFileListener LL = null;

        private void StartListening(object sender, DwarfFortressRunningEventArgs e)
        {
            if (runState == RunState.LookingForDF)
            {
                image1.Pixbuf = Image.LoadFromResource("SoundCenSeGTK.SignalGreen.png").Pixbuf;

                runState = RunState.PlayingDF;
                SP = new SoundProcessor(allSounds);
                LL = new LogFileListener(Config.Instance.gamelogPath, true);

                LL.GamelogEvent += SP.ProcessLine;
                LL.GamelogEvent += ShowLogInStatus;

                fmodPlayer.Instance.SoundPlaying += Playing;
                Dictionary<string, Sound> oldMusic = GetOldMusic(allSounds);
                foreach (Sound s in oldMusic.Values)
                {
                    fmodPlayer.Instance.Play(s, 0, 0, 0);
                }
            }
        }

        private Dictionary<string, Sound> GetOldMusic(SoundsXML allSounds)
        {
            DummyPlayerManager dpm = new DummyPlayerManager();
            DummySoundProcessor sp = new DummySoundProcessor(allSounds);
            sp.DummyPlayerManager = dpm;

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

            tr.Close();
            fs.Close();

            foreach (string l in lines)
            {
                sp.ProcessLine(this, new GamelogEventArgs(l));
            }
            return dpm.Channels;
        }

        private void Playing(object sender, SoundPlayingEventArgs soundPlayingEventArgs)
        {
            string channel = soundPlayingEventArgs.Sound.Channel.Capitalize();
            if (string.IsNullOrEmpty(channel))
            {
                channel = "SFX";
            }
            string soundFile = soundPlayingEventArgs.SoundFile.Filename;

            panelEntries[channel].SetValues(soundFile, soundPlayingEventArgs.SoundFile.Length, soundPlayingEventArgs.Mute, soundPlayingEventArgs.Volume, soundPlayingEventArgs.Sound.loop == Loop.Start_Looping);


        }

        public void ShowLogInStatus(object sender, GamelogEventArgs e)
        {
            StatusBar.Push(0, e.Line);
        }

        private void StopListening(object sender, DwarfFortressStoppedEventArgs e)
        {
            image1.Pixbuf = Image.LoadFromResource("SoundCenSeGTK.SignalRed.png").Pixbuf;
            if (runState == RunState.PlayingDF)
            {
                runState = RunState.LookingForDF;
                SP = null;
                if (LL != null)
                {
                    LL.Dispose();
                }
            }
        }

        private RunState runState = RunState.Startup;

        private SoundsXML allSounds = null;

        private bool Timer()
        {
            switch (runState)
            {
                case RunState.Startup:
                    allSounds = new SoundsXML(Config.Instance.soundpacksPath);
                    runState = RunState.LookingForDF;
                    break;
                case RunState.Updating:
                    PD.DoWork();
                    break;
                case RunState.LookingForDF:
                    DFRunCheck();
                    break;
                case RunState.PlayingDF:
                    LL.Tick();
                    FmodSystem.System.update();
                    DFRunCheck();
                    foreach (SoundPanelEntry spe in panelEntries.Values)
                    {
                        spe.Tick();
                    }
                    break;
            }
            return true;
        }

        protected void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            Config.Instance.Save("Configuration.json");
            Application.Quit();
            a.RetVal = true;
        }

        private void AddSoundPanelEntries(List<string> channels)
        {
            foreach (string channel in channels)
            {
                SoundPanelEntry spe = new SoundPanelEntry(channel);
                ChannelData cd = Config.Instance.GetChannelData(channel.ToLower());

                spe.Volume = cd.Volume;
                spe.Mute = cd.Mute;
                spe.ChannelFastForward += ChannelFastForward;
                spe.ChannelMute += ChannelMute;
                spe.VolumeChanged += VolumeChanged;
                spe.SoundDisabled += SoundDisabled;
                vbox2.PackStart(spe, true, true, 0);
                spe.Show();
                panelEntries.Add(channel, spe);
            }
        }

        private void SoundDisabled(object sender, DisableSoundEventArgs disableSoundEventArgs)
        {
            if (!Config.Instance.disabledSounds.Contains(disableSoundEventArgs.Filename))
            {
                // lbDisabledSounds.Items.Add(disableSoundEventArgs.Filename);
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

        private void VolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            fmodPlayer.Instance.VolumeChannel(e.Channel, (float)e.Volume);
            Config.Instance.SetChannelVolume(e.Channel.ToLower(), (float)e.Volume);

        }
        private void ChannelFastForward(object sender, ChannelFastForwardEventArgs e)
        {
            fmodPlayer.Instance.FastForward(e.ChannelName);
        }

        private void ChannelMute(object sender, ChannelMuteEventArgs e)
        {
            fmodPlayer.Instance.MuteChannel(e.Channel, e.Mute);
        }


        private void DFRunCheck()
        {
            Process[] processlist = Process.GetProcesses();
            foreach (Process p in processlist)
            {
                if ((p.ProcessName == "Dwarf_Fortress") || (p.ProcessName == "Dwarf Fortress"))
                {
                    if (System.IO.Path.DirectorySeparatorChar == '\\')
                    {
                        Config.Instance.gamelogPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(p.MainModule.FileName), "gamelog.txt");
                    }
                    else
                    {
                        Config.Instance.gamelogPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(p.MainModule.FileName), "..", "gamelog.txt");
                    }

                    OnDFRunning(p.Id);
                    return;
                }
               
            }
            OnDFNotRunning();
        }

        public EventHandler<DwarfFortressRunningEventArgs> Running;
        public EventHandler<DwarfFortressStoppedEventArgs> Stopped;

        private void OnDFRunning(int processId)
        {
            var handler = Running;
            if (handler != null)
            {
                handler(this, new DwarfFortressRunningEventArgs(processId));
            }
        }

        private void OnDFNotRunning()
        {
            var handler = Stopped;
            if (handler != null)
            {
                handler(this, new DwarfFortressStoppedEventArgs());
            }
        }


        protected void btnUpdateClick(object sender, EventArgs e)
        {
            UpdateListbox.Buffer.Text = "";
            AddToUpdateListbox("Start update soundpack");
            PD = new PackDownloader();
            PD.FinishedFile += FinishedSingleFile;
            PD.UpdateFinished += UpdateFinished;
            PD.DownloadStarted += DownloadStarted;
            PD.UpdateSoundPack();
            runState = RunState.Updating;
        }

        private void FinishedSingleFile(object sender, DownloadFinishedEventArgs e)
        {
            Application.Invoke(delegate
                {
                    AddToUpdateListbox("Downloaded " + System.IO.Path.GetFileName(e.File.DestinationPath));
                });
        }

        private void AddToUpdateListbox(string s)
        {
            TextIter ti = UpdateListbox.Buffer.EndIter;
            UpdateListbox.Buffer.Insert(ref ti, s + Environment.NewLine);
        }

        private void UpdateFinished(object sender, UpdateFinishedEventArgs e)
        {
            Application.Invoke(delegate
                {
                    AddToUpdateListbox("Update finished");
                });
            runState = RunState.LookingForDF;
        }

        private void DownloadStarted(object sender, StartDownloadEventArgs e)
        {
            Application.Invoke(delegate
                {
                    AddToUpdateListbox("Downloading " + System.IO.Path.GetFileName(e.File.DestinationPath));
                });
        }

        protected void ScrollDown(object o, SizeAllocatedArgs args)
        {
            Application.Invoke(delegate
                {
                    UpdateListbox.ScrollToIter(UpdateListbox.Buffer.EndIter, 0, false, 0, 0);
                });
        }

        protected void ThresholdChanged(object sender, EventArgs e)
        {
            Config.Instance.playbackThreshold = (Threshold)cbThreshold.Active;
        }
    }
}