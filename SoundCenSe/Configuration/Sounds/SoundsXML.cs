// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundsXML.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NLog;
using SoundCenSe.Configuration.Sounds.Playlist;
using SoundCenSe.Enums;
using SoundCenSe.Events;

namespace SoundCenSe.Configuration.Sounds
{
    public class SoundsXML : IDisposable
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly List<string> parsedDirectories = new List<string>();
        public EventHandler<XMLParseProgressEventArgs> ParseProgress;
        public EventHandler<XMLParseDoneEventArgs> XMLDone;

        #endregion

        #region Properties

        private bool ignoreEmptySounds { get; set; }
        private bool noWarnAbsolutePath { get; set; }
        private string rootDirectory { get; set; }

        public List<Sound> Sounds { get; set; }

        #endregion

        public SoundsXML(string directory, bool ignoreEmptySounds = false, bool noWarnAbsolutePath = false)
        {
            rootDirectory = directory;
            this.ignoreEmptySounds = ignoreEmptySounds;
            this.noWarnAbsolutePath = noWarnAbsolutePath;
            Sounds = new List<Sound>();
            LoadDirectory(rootDirectory, ignoreEmptySounds);
            OnParseDone();
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Sound s in Sounds)
                {
                    s.Dispose();
                }
            }
        }

        public List<Sound> GetSoundsByXMLFile(string xmlFile)
        {
            return Sounds.Where(x => x.ParentFile == xmlFile).ToList();
        }

        public void LoadDirectory(string directory, bool ignoreEmpty)
        {
            if (!directory.EndsWith(Path.DirectorySeparatorChar + ""))
            {
                directory += Path.DirectorySeparatorChar;
            }
            logger.Info("Scanning directory '" + directory + "'");

            if (parsedDirectories.Contains(Path.GetFullPath(directory)))
            {
                logger.Info("Ignoring parsed folder");
            }
            else
            {
                if (Directory.Exists(directory))
                {
                    string[] files =
                        Directory.GetFiles(directory, "*.xml", SearchOption.AllDirectories).OrderBy(x => x).ToArray();

                    foreach (string file in files)
                    {
                        try
                        {
                            logger.Info("Loading sound config '" + file + "'");
                            LoadFile(file, ignoreEmpty);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Failed to load '" + file + "'");
                            logger.Error(ex.Message);
                        }
                    }
                }
            }
        }

        public void LoadFile(string filename, bool ignoreEmpty)
        {
            XmlDocument doc = new XmlDocument();
            TextReader tr = new StreamReader(filename);
            string data = tr.ReadToEnd().Replace("<?xml version=\"1.1\"", "<?xml version=\"1.0\"");
            tr.Close();
            doc.LoadXml(data);
            XmlNodeList rootElement = doc.GetElementsByTagName("sounds");
            string defaultAnsiFormat = null;
            bool strictAttributions = false;
            if (rootElement.Count > 0)
            {
                defaultAnsiFormat = rootElement.Item(0).ParseStringAttribute("defaultAnsiFormat", null);
                strictAttributions = rootElement.Item(0).ParseBoolAttribute("strictAttributions", false);
            }

            XmlNodeList soundTags = doc.GetElementsByTagName("sound");
            ParseSounds(soundTags, filename, ignoreEmpty, defaultAnsiFormat, strictAttributions);

            XmlNodeList directoryReferences = doc.GetElementsByTagName("includeDirectory");
            ParseDirectories(directoryReferences);

            XmlNodeList listings = doc.GetElementsByTagName("includeListing");
            ParseListings(listings);

            OnXMLParseProgress(filename);
        }

        protected void OnParseDone()
        {
            var handler = XMLDone;
            if (handler != null)
            {
                handler(this, new XMLParseDoneEventArgs());
            }
        }

        protected void OnXMLParseProgress(string filename)
        {
            var handler = ParseProgress;
            if (handler != null)
            {
                handler(this, new XMLParseProgressEventArgs(filename));
            }
        }

        private void ParseAttributions(SoundFile soundfile, XmlNodeList soundfileChildren)
        {
            foreach (XmlNode node in soundfileChildren)
            {
                if (node.LocalName == "attribution")
                {
                    Attribution attribution = Attribution.ReadFromXML(node, soundfile);
                    if (attribution != null)
                    {
                        soundfile.Attributions.Add(attribution);
                    }
                }
            }
        }

        private void ParseDirectories(XmlNodeList directoryReferences)
        {
            foreach (XmlNode node in directoryReferences)
            {
                string reference = node.ParseStringAttribute("path", null);
                if (reference == null)
                {
                    logger.Warn("Directory reference tag without 'path' attribute.");
                }
                else
                {
                    LoadDirectory(reference, ignoreEmptySounds);
                }
            }
        }

        private void ParseListings(XmlNodeList listings)
        {
            foreach (XmlNode node in listings)
            {
                string filepath = node.ParseStringAttribute("filePathAndName", null);

                if ((filepath == null) || (Path.GetExtension(filepath) != "xml"))
                {
                    logger.Warn(
                        "Include listing tag without valid 'filePathAndName' attribute encountered (make sure it ends in '.xml'!).");
                }
                else
                {
                    logger.Info("Loading included config " + filepath);
                    LoadFile(filepath, ignoreEmptySounds);
                }
            }
        }

        private List<string> ParsePlaylist(string parentFilename, string playlistFilename)
        {
            logger.Info("Loading playlist " + playlistFilename);

            string extension = Path.GetExtension(playlistFilename).ToLower();

            IPlaylistParser playlistParser = null;

            if (extension == ".pls")
            {
                playlistParser = new PLSParser();
            }
            else if (extension == ".m3u")
            {
                playlistParser = new M3UParser();
            }

            if (playlistParser == null)
            {
                logger.Info("Unsupported playlist format for '" + playlistFilename + "'");
                return new List<string>();
            }
            Stream input = null;
            try
            {
                using (input = new FileStream(playlistFilename, FileMode.Open, FileAccess.Read))
                {
                    return playlistParser.Parse(playlistFilename, input);
                }
            }
            catch (Exception e)
            {
                logger.Fatal("Playlist parsing: " + e.Message);
                logger.Fatal(e.StackTrace);
            }
            return new List<string>();
        }

        private List<SoundFile> ParseSoundFile(XmlNode soundFileNode, string filename)
        {
            XmlNode filenameAttribute = soundFileNode.Attributes.GetNamedItem("fileName");
            if (filenameAttribute == null)
            {
                logger.Warn("Could not locate 'filename' attribute in sound element, please check spelling.");
                return null;
            }

            bool isPlayList = soundFileNode.ParseBoolAttribute("playlist", false);
            List<string> soundFileNames = new List<string>();
            if (isPlayList)
            {
                string playlistFilename = Path.Combine(Path.GetDirectoryName(filename), filenameAttribute.Value);
                if (!File.Exists(playlistFilename))
                {
                    if (File.Exists(playlistFilename))
                    {
                        if (!noWarnAbsolutePath)
                        {
                            logger.Warn("File " + playlistFilename + " is on absolute path.");
                        }
                    }
                    else
                    {
                        logger.Warn("Did not find " + playlistFilename + ", ignoring.");
                        return null;
                    }
                }
                soundFileNames.AddRange(ParsePlaylist(filename, playlistFilename));
            }
            else
            {
                string soundFileName = Path.Combine(Path.GetDirectoryName(filename), filenameAttribute.Value);
                if (!File.Exists(soundFileName))
                {
                    if (File.Exists(filenameAttribute.Value))
                    {
                        if (!noWarnAbsolutePath)
                        {
                            string soundFilename = filenameAttribute.Value;
                            logger.Warn("File " + soundFileName + " is on absolute Path.");
                        }
                    }
                    else
                    {
                        logger.Warn("Did not file " + soundFileName + ", ignoring.");
                        return null;
                    }
                }
                soundFileNames.Add(soundFileName);
            }

            int weight = soundFileNode.ParseIntAttribute("weight", 100);

            float volumeAdjustment = soundFileNode.ParseFloatAttribute("volumeAdjustment", 0.0f);
            float balanceAdjustment = soundFileNode.ParseFloatAttribute("balanceAdjustment", 0.0f);
            bool randomBalance = soundFileNode.ParseBoolAttribute("randomBalance", false);

            List<SoundFile> soundFiles = new List<SoundFile>(soundFileNames.Count);

            foreach (string soundFilename in soundFileNames)
            {
                SoundFile soundFile = new SoundFile(soundFilename, weight, volumeAdjustment, balanceAdjustment,
                    randomBalance);
                ParseAttributions(soundFile, soundFileNode.ChildNodes);
                soundFiles.Add(soundFile);
            }
            return soundFiles;
        }

        private void ParseSounds(XmlNodeList soundTags, string filename, bool ignoreEmpty, string defaultAnsiFormat,
            bool strictAttributions)
        {
            foreach (XmlNode soundNode in soundTags)
            {
                string logPattern = soundNode.ParseStringAttribute("logPattern", null);
                if (logPattern == null)
                {
                    logger.Warn("Sound tag without logPattern attribute.");
                }

                string channel = soundNode.ParseStringAttribute("channel", null);

                String ansiFormat = soundNode.ParseStringAttribute("ansiFormat", defaultAnsiFormat);

                // Default
                Loop channelLoop = Loop.Stop_Looping;


                if (soundNode.Attributes.GetNamedItem("loop") != null)
                {
                    string loop = soundNode.Attributes.GetNamedItem("loop").Value;
                    if ((loop == "true") || (loop == "start"))
                    {
                        channelLoop = Loop.Start_Looping;
                    }
                    else if (loop == "stop")
                    {
                        channelLoop = Loop.Stop_Looping;
                    }
                    else
                    {
                        logger.Warn("Loop paramateter '" + loop + "' not recognized");
                    }
                }

                bool haltOnMatch = soundNode.ParseBoolAttribute("haltOnMatch", true);
                long delay = soundNode.ParseLongAttribute("delay", 0);
                long concurrency = soundNode.ParseLongAttribute("concurency", -1);
                long timeout = soundNode.ParseLongAttribute("timeout", 0);
                long probability = soundNode.ParseLongAttribute("probability", 0);
                long playbackThreshold = soundNode.ParseLongAttribute("playbackThreshhold", (long) Threshold.Fluffy);

                List<SoundFile> soundFiles = new List<SoundFile>();

                XmlNodeList soundElements = soundNode.ChildNodes;
                foreach (XmlNode configNode in soundElements)
                {
                    string name = configNode.LocalName;
                    if (name.Equals("soundFile"))
                    {
                        List<SoundFile> soundFileTagSoundFiles = ParseSoundFile(configNode, filename);
                        if (soundFileTagSoundFiles != null)
                        {
                            foreach (SoundFile soundFile in soundFileTagSoundFiles)
                            {
                                if (strictAttributions && soundFile.Attributions.Count == 0)
                                {
                                    logger.Info("Sound file '" + soundFile.Filename + "' in '" + logPattern +
                                                "' lacks attributions!");
                                }
                            }
                            soundFiles.AddRange(soundFileTagSoundFiles);
                        }
                    }
                }

                if (string.IsNullOrEmpty(logPattern))
                {
                    logger.Info("Sound does not have a logpattern, ignoring.");
                }
                else if ((soundFiles.Count == 0 && ignoreEmpty) && (channelLoop != Loop.Stop_Looping))
                {
                    logger.Info("Sound for '" + logPattern +
                                "' does not contain any soundFiles and is not speech or end of loop, ignoring.");
                }
                else
                {
                    Sounds.Add(new Sound(filename, soundFiles, logPattern, ansiFormat, channelLoop, channel, concurrency,
                        haltOnMatch, timeout, delay, probability, playbackThreshold));
                    logger.Info("Added sound for " + logPattern);
                }
            }
        }

        public override string ToString()
        {
            return rootDirectory;
        }

        public List<string> XMLFiles()
        {
            List<string> files = new List<string>();
            foreach (Sound sound in Sounds)
            {
                if (!files.Contains(sound.ParentFile))
                {
                    files.Add(sound.ParentFile);
                }
            }
            return files;
        }
    }
}