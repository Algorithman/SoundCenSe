// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: MyMixingSampleProvider.cs
// 
// Last modified: 2016-07-17 22:06

using System;
using System.Collections.Generic;
using NAudio.Utils;
using NAudio.Wave;
using SoundCenSe.Configuration;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Utility;

namespace SoundCenSe.Audio
{
    public class MyMixingSampleProvider : ISampleProvider, IDisposable
    {
        #region Fields and Constants

        private const int maxInputs = 1024; // protect ourselves against doing something silly
        private float[] sourceBuffer;
        private readonly List<ISampleProvider> sources;

        #endregion

        #region Properties

        public int ConcurrentSounds
        {
            get { return sources.Count; }
        }

        /// <summary>
        ///     When set to true, the Read method always returns the number
        ///     of samples requested, even if there are no inputs, or if the
        ///     current inputs reach their end. Setting this to true effectively
        ///     makes this a never-ending sample provider, so take care if you plan
        ///     to write it out to a file.
        /// </summary>
        public bool ReadFully { get; set; }

        #endregion

        /// <summary>
        ///     Creates a new MixingSampleProvider, with no inputs, but a specified WaveFormat
        /// </summary>
        /// <param name="waveFormat">The WaveFormat of this mixer. All inputs must be in this format</param>
        public MyMixingSampleProvider(WaveFormat waveFormat)
        {
            if (waveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                throw new ArgumentException("Mixer wave format must be IEEE float");
            }
            sources = new List<ISampleProvider>();
            WaveFormat = waveFormat;
            ReadFully = true;
        }

        /// <summary>
        ///     Creates a new MixingSampleProvider, based on the given inputs
        /// </summary>
        /// <param name="sources">
        ///     Mixer inputs - must all have the same waveformat, and must
        ///     all be of the same WaveFormat. There must be at least one input
        /// </param>
        public MyMixingSampleProvider(IEnumerable<ISampleProvider> sources)
        {
            this.sources = new List<ISampleProvider>();
            foreach (var source in sources)
            {
                AddMixerInput(source);
            }
            if (this.sources.Count == 0)
            {
                throw new ArgumentException("Must provide at least one input in this constructor");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ISampleProvider Members

        /// <summary>
        ///     The output WaveFormat of this sample provider
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        ///     Reads samples from this sample provider
        /// </summary>
        /// <param name="buffer">Sample buffer</param>
        /// <param name="offset">Offset into sample buffer</param>
        /// <param name="count">Number of samples required</param>
        /// <returns>Number of samples read</returns>
        public int Read(float[] buffer, int offset, int count)
        {
            int outputSamples = 0;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, count);
            lock (sources)
            {
                int index = sources.Count - 1;
                while (index >= 0)
                {
                    var source = sources[index];
                    int samplesRead = source.Read(sourceBuffer, 0, count);
                    int outIndex = offset;
                    for (int n = 0; n < samplesRead; n++)
                    {
                        if (n >= outputSamples)
                        {
                            buffer[outIndex++] = sourceBuffer[n];
                        }
                        else
                        {
                            buffer[outIndex++] += sourceBuffer[n];
                        }
                    }
                    outputSamples = Math.Max(samplesRead, outputSamples);
                    if (samplesRead == 0)
                    {
                        sources.RemoveAt(index);
                    }
                    index--;
                }
            }
            // optionally ensure we return a full buffer
            if (ReadFully && outputSamples < count)
            {
                int outputIndex = offset + outputSamples;
                while (outputIndex < offset + count)
                {
                    buffer[outputIndex++] = 0;
                }
                outputSamples = count;
            }
            return outputSamples;
        }

        #endregion

        public ChannelSound Add(SoundFile soundFile, float volume = 1.0f, float panning = 0.0f, string channel = "")
        {
            if (soundFile.Cache == null)
            {
                soundFile.GenerateCache(WaveFormat);
            }
            CachedSoundSampleProvider cssp = new CachedSoundSampleProvider(soundFile.Cache);
            ChannelSound cs = new ChannelSound(cssp, volume, channel, soundFile.Filename,
                soundFile.Cache.AudioData.Length/(Constants.AudioChannels*Constants.Samplerate/1000));

            AddMixerInput(cs);
            return cs;
        }

        /// <summary>
        ///     Adds a new mixer input
        /// </summary>
        /// <param name="mixerInput">Mixer input</param>
        public void AddMixerInput(ISampleProvider mixerInput)
        {
            // we'll just call the lock around add since we are protecting against an AddMixerInput at
            // the same time as a Read, rather than two AddMixerInput calls at the same time
            lock (sources)
            {
                if (sources.Count >= maxInputs)
                {
                    throw new InvalidOperationException("Too many mixer inputs");
                }
                sources.Add(mixerInput);
            }
            if (WaveFormat == null)
            {
                WaveFormat = mixerInput.WaveFormat;
            }
            else
            {
                if (WaveFormat.SampleRate != mixerInput.WaveFormat.SampleRate ||
                    WaveFormat.Channels != mixerInput.WaveFormat.Channels)
                {
                    throw new ArgumentException("All mixer inputs must have the same WaveFormat");
                }
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (sources)
                {
                    sources.Clear();
                }
            }
        }

        public List<Tuple<string, string, int>> GetSoundData()
        {
            List<Tuple<string, string, int>> result = new List<Tuple<string, string, int>>();

            lock (sources)
            {
                foreach (ISampleProvider isp in sources)
                {
                    ChannelSound cs = (ChannelSound) isp;
                    if (cs != null)
                    {
                        result.Add(new Tuple<string, string, int>(cs.ChannelName.Capitalize(), cs.Filename, cs.Length));
                    }
                }
            }
            return result;
        }

        public bool IsPlaying(ISampleProvider which)
        {
            lock (sources)
            {
                return sources.Contains(which);
            }
        }

        /// <summary>
        ///     Removes all mixer inputs
        /// </summary>
        public void RemoveAllMixerInputs()
        {
            lock (sources)
            {
                sources.Clear();
            }
        }

        /// <summary>
        ///     Removes a mixer input
        /// </summary>
        /// <param name="mixerInput">Mixer input to remove</param>
        public void RemoveMixerInput(ISampleProvider mixerInput)
        {
            lock (sources)
            {
                sources.Remove(mixerInput);
            }
        }
    }
}