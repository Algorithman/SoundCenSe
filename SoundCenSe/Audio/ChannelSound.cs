// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: ChannelSound.cs
// 
// Last modified: 2016-07-17 22:06

using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using SoundCenSe.Events;

namespace SoundCenSe.Audio
{
    public class ChannelSound : ISampleProvider
    {
        #region Fields and Constants

        private ISampleProvider input;
        private bool mute;
        private readonly Stack<ISampleProvider> Samplers = new Stack<ISampleProvider>();
        private float volume = 1.0f;
        private VolumeSampleProvider volumeSampler;

        #endregion

        #region Properties

        public string ChannelName { get; set; } = "";
        public string Filename { get; set; }

        public ISampleProvider Input
        {
            get { return input; }
            set
            {
                if (value != input)
                {
                    GenerateSamplers(value);
                    input = value;
                }
            }
        }

        public int Length { get; set; }

        public bool Mute
        {
            get { return mute; }
            set
            {
                if (value != mute)
                {
                    mute = value;
                    Volume = volume;
                }
            }
        }

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                if (volumeSampler != null)
                {
                    volumeSampler.Volume = mute ? 0.0f : value;
                }
            }
        }

        #endregion

        public ChannelSound(ISampleProvider input, float volume, string chName, string fName, int len)
        {
            this.input = input;
            this.volume = volume;
            GenerateSamplers(input);
            ChannelName = chName;
            Filename = fName;
            Length = len;
        }

        private void GenerateSamplers(ISampleProvider sampler)
        {
            Samplers.Clear();
            Samplers.Push(sampler);
            volumeSampler = new VolumeSampleProvider(Samplers.Peek());
            volumeSampler.Volume = volume;
            Samplers.Push(volumeSampler);
        }

        #region Implementation of ISampleProvider

        /// <summary>
        ///     Fill the specified buffer with 32 bit floating point samples
        /// </summary>
        /// <param name="buffer">The buffer to fill with samples.</param>
        /// <param name="offset">Offset into buffer</param>
        /// <param name="count">The number of samples to read</param>
        /// <returns>
        ///     the number of samples written to the buffer.
        /// </returns>
        public int Read(float[] buffer, int offset, int count)
        {
            return Samplers.Peek().Read(buffer, offset, count);
        }

        /// <summary>
        ///     Gets the WaveFormat of this Sample Provider.
        /// </summary>
        /// <value>
        ///     The wave format.
        /// </value>
        public WaveFormat WaveFormat
        {
            get { return Samplers.Peek().WaveFormat; }
        }

        internal void ChannelMute(object sender, ChannelMuteEventArgs e)
        {
            Mute = e.Mute;
        }

        #endregion
    }
}