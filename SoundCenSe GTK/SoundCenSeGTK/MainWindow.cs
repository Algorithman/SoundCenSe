using System;
using Gtk;
using SoundCenSeGUI;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Misc;
using SoundCenSeGTK;
using System.Linq;
using System.Reflection;

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
            this.Title="SoundCenSe GTK " +
                string.Join(".",
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location)
                    .FileVersion.Split('.')
                    .Take(3)
                    .ToArray());

            // Set AutoDetect to false when not running on windows
            Config.Instance.autoDetect &= System.IO.Path.DirectorySeparatorChar == '\\';
            // Set checkbox for autodetect visible only for windows
            cbAutoDetect.Visible = System.IO.Path.DirectorySeparatorChar == '\\';

            // Remove Notebook page 5 (debug)
            notebook1.RemovePage(5);

            // Switch to page 2 (disabled Sounds) and add them there
            notebook1.Page = 2;
            AddDisabledSounds();

            // Switch to page 3 (Config) and set the values
            notebook1.Page = 3;
            entrySoundpackPath.Text = Config.Instance.soundpacksPath;
            entryGamelogPath.Text = Config.Instance.gamelogPath;
            cbAutoDetect.Active = Config.Instance.autoDetect;
            labelGamelogPath.Visible = !Config.Instance.autoDetect;
            entryGamelogPath.Visible = !Config.Instance.autoDetect;
            btnGamelogPath.Visible = !Config.Instance.autoDetect;

            // Back to page 0 (Audio) and set channels, threshold etc.
            notebook1.Page = 0;
            int count = Enum.GetValues(typeof(Threshold)).Length;
            foreach (var v in Enum.GetValues(typeof(Threshold)))
            {
                cbThreshold.AppendText(v.ToString());
            }

            cbThreshold.Active = (int)Config.Instance.playbackThreshold;

            List<string> temp = new List<string>() { "SFX", "Music", "Weather", "Swords", "Trading" };
            AddSoundPanelEntries(temp);
            // ShowAll();

            Running += StartListening;
            Stopped += StopListening;

            // Start main tick timer
            GLib.Timeout.Add(50, Timer);
        }

        private void AddDisabledSounds()
        {
            foreach (string soundName in Config.Instance.disabledSounds)
            {
                AddDisabledSound(soundName);
            }
        }

        private void AddDisabledSound(string soundName)
        {
            SoundDisabler sd = new SoundDisabler(soundName);
            sd.SoundDisabled += EnableSound;
            vboxDisabledSounds.PackStart(sd, false, false, 0);
            sd.Show();
        }

        private void EnableSound(object sender, DisableSoundEventArgs e)
        {
            SoundDisabler sd = (SoundDisabler)sender;
            Config.Instance.disabledSounds.Remove(e.Filename);
            sd.Destroy();
            foreach (Sound s in allSounds.Sounds)
            {
                foreach (SoundFile sf in s.SoundFiles)
                {
                    if (sf.Filename == e.Filename)
                    {
                        sf.Disabled = false;
                    }
                }
            }
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
                Dictionary<string, Sound> oldMusic = GetOldMusic(allSounds);
                LL = new LogFileListener(Config.Instance.gamelogPath, true);

                LL.GamelogEvent += SP.ProcessLine;
                LL.GamelogEvent += ShowLogInStatus;

                fmodPlayer.Instance.SoundPlaying += Playing;
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

            List<string> lines = new List<string>();
            using (FileStream fs = new FileStream(Config.Instance.gamelogPath, FileMode.Open, FileAccess.Read,
                                       FileShare.ReadWrite))
            {
                using (TextReader tr = new StreamReader(fs))
                {


                    string line = "";
                    while ((line = tr.ReadLine()) != null)
                    {
                        if (line == "*** STARTING NEW GAME ***")
                        {
                            lines.Clear();
                        }
                        lines.Add(line);
                    }

                }
            }
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

        private int TickCounter = 0;

        private bool Timer()
        {
            switch (runState)
            {
                case RunState.Startup:
                    allSounds = new SoundsXML(Config.Instance.soundpacksPath);
                    DisableDisabledSounds();
                    runState = RunState.LookingForDF;
                    btnUpdate.Sensitive = true;
                    break;
                case RunState.Updating:
                    btnUpdate.Sensitive = false;
                    PD.DoWork();
                    break;
                case RunState.LookingForDF:
                    btnUpdate.Sensitive = true;
                    DFRunCheck();
                    break;
                case RunState.PlayingDF:
                    TickCounter++;
                    LL.Tick();
                    FmodSystem.ERRCHECK(FmodSystem.System.update());
                    if ((TickCounter % 10) == 0)
                    {
                        DFRunCheck();
                    }
                    foreach (SoundPanelEntry spe in panelEntries.Values)
                    {
                        spe.Tick();
                    }
                    break;
            }
            return true;
        }

        private void DisableDisabledSounds()
        {
            foreach (Sound s in allSounds.Sounds)
            {
                foreach (SoundFile sf in s.SoundFiles)
                {
                    if (Config.Instance.disabledSounds.Contains(sf.Filename))
                    {
                        sf.Disabled = true;
                    }
                }
            }
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
                AddDisabledSound(disableSoundEventArgs.Filename);
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
            if (!Config.Instance.autoDetect)
            {
                if (File.Exists(Config.Instance.gamelogPath))
                {
                    OnDFRunning(1);
                    return;
                }
            }
            else
            {
                Process[] processlist = Process.GetProcesses();
                foreach (Process p in processlist)
                {
                    try
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
                    catch
                    {
                    }
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

        protected void AutoDetectChanged(object sender, EventArgs e)
        {
            Config.Instance.autoDetect = cbAutoDetect.Active;
            entryGamelogPath.Text = Config.Instance.gamelogPath;
            labelGamelogPath.Visible = !cbAutoDetect.Active;
            entryGamelogPath.Visible = !cbAutoDetect.Active;
            btnGamelogPath.Visible = !cbAutoDetect.Active;
            if (cbAutoDetect.Active && (runState != RunState.Startup))
            {
                runState = RunState.LookingForDF;
            }
        }

        protected void btnSoundPackPathClicked(object sender, EventArgs e)
        {
            var folderChooser = new Gtk.FileChooserDialog("Select Soundpack folder", this, FileChooserAction.SelectFolder, Gtk.Stock.Cancel, ResponseType.Cancel, Gtk.Stock.Open, ResponseType.Accept);
            folderChooser.SetCurrentFolder(Config.Instance.soundpacksPath);
            if (folderChooser.Run() == (int)ResponseType.Accept)
            {
                Config.Instance.soundpacksPath = folderChooser.Filename;
                entrySoundpackPath.Text = folderChooser.Filename;
                runState = RunState.Startup;
            }
            folderChooser.Destroy();
        }

        protected void btnGamelogPathClicked(object sender, EventArgs e)
        {
            var fileChooser = new Gtk.FileChooserDialog("Select Dwarf Fortress gamelog", this, FileChooserAction.Open, Gtk.Stock.Cancel, ResponseType.Cancel, Gtk.Stock.Open, ResponseType.Accept);
            string path = System.IO.Path.GetDirectoryName(Config.Instance.gamelogPath);
            if (path != null)
            {
                fileChooser.SetCurrentFolder(path);
            }
            var filter = new Gtk.FileFilter();
            filter.Name = "Dwarf Fortress gamelog";
            filter.AddPattern("gamelog.txt");
            fileChooser.AddFilter(filter);
            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                Config.Instance.gamelogPath = fileChooser.Filename;
                entryGamelogPath.Text = fileChooser.Filename;
                runState = RunState.Startup;
            }
            fileChooser.Destroy();
        }
    }
}