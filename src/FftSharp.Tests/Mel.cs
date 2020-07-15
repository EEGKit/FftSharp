﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FftSharp.Tests
{
    class Mel
    {
        [Test]
        public void Test_Mel_VsFFT()
        {
            double[] audio = SampleData.SampleAudio1();
            int sampleRate = 48_000;
            int melBinCount = 20;

            double[] fftMag = FftSharp.Transform.FFTmagnitude(audio);
            double[] fftMagMel = FftSharp.Transform.MelScale(fftMag, sampleRate, melBinCount);
            double fftFreqPeriod = FftSharp.Transform.FFTfreqPeriod(sampleRate, fftMag.Length);
            double maxMel = FftSharp.Transform.MelFromFreq(sampleRate / 2);

            var plt = new ScottPlot.MultiPlot(1000, 600, 2, 1);

            // TRADITIONAL SPECTROGRAM
            plt.subplots[0].PlotSignal(fftMag, fftFreqPeriod);
            for (int i = 0; i < melBinCount; i++)
            {
                double thisMel = (double)i / melBinCount * maxMel;
                double thisFreq = FftSharp.Transform.MelToFreq(thisMel);
                plt.subplots[0].PlotVLine(thisFreq, lineWidth: 2);
            }
            plt.subplots[0].YLabel("Magnitude");
            plt.subplots[0].XLabel("Frequency (Hz)");

            // MEL SPECTROGRAM
            plt.subplots[1].PlotSignal(fftMagMel);
            for (int i = 0; i < melBinCount; i++)
            {
                plt.subplots[1].PlotVLine(i, lineWidth: 2);
            }
            plt.subplots[1].YLabel("Magnitude");
            plt.subplots[1].XLabel("Frequency (Mel)");
            
            plt.SaveFig("audio-mel.png");
        }
    }
}
