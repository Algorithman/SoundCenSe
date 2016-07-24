// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: CachedSound.cs
// 
// Last modified: 2016-07-24 13:52

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

#endregion

namespace SoundCenSe.Audio
{
    public class CachedSound
    {
        #region Properties

        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        #endregion

        public CachedSound(string audioFileName, WaveFormat wf)
        {
            var audioFileReader = new AudioFileReader(audioFileName);

            MediaFoundationResampler mfp = null;
            IWaveProvider iwp = audioFileReader;
            if (!audioFileReader.WaveFormat.EQUALS(wf))
            {
                mfp = new MediaFoundationResampler(audioFileReader, wf);
                iwp = mfp;
            }

            WaveFormat = wf;


            var wholeFile = new List<float>((int) audioFileReader.TotalTime.TotalMilliseconds*96);
            var readBuffer = new float[wf.SampleRate*wf.Channels];
            var readBufferByte = new byte[wf.SampleRate*wf.Channels*4];
            int samplesRead;

            while ((samplesRead = iwp.Read(readBufferByte, 0, readBufferByte.Length)) > 0)
            {
                Buffer.BlockCopy(readBufferByte, 0, readBuffer, 0, samplesRead);
                wholeFile.AddRange(readBuffer.Take(samplesRead >> 2));
            }

            AudioData = wholeFile.ToArray();
            if (mfp != null)
            {
                mfp.Dispose();
            }
            audioFileReader.Dispose();
        }
    }
}