using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace noterunner.Controller
{
	class SoundCapture
	{
		private const double MAX_FREQ = 3000;
		private const int SAMPLE_RATE = 44100;
		private const int BUFFER_MSECS = 100;

		public delegate void FreqChanged(double freq);
		public static event FreqChanged OnFreqChanged;

		public static int deviceCount { get; private set; }
		public static List<string> availableDevices { get; private set; }

		private static WaveIn waveSource = null;
		public static int deviceNumber { get; set; } = 0;

		/// <summary>
		/// Initialize recording devices.
		/// Всем привет.
		/// </summary>
		public static void Initialize()
		{
			deviceCount = WaveIn.DeviceCount;
			availableDevices = new List<string>();

			for (int waveInDevice = 0; waveInDevice < deviceCount; waveInDevice++)
			{
				WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
				availableDevices.Add(deviceInfo.ProductName);
			}
		}

		public static void StopCapturing()
		{
			if (waveSource != null)
			{
				waveSource.StopRecording();
				waveSource.Dispose();
				waveSource = null;
			}
		}

		/// <summary>
		/// Start capturing of data.
		/// </summary>
		public static void StartCapturing()
		{
			waveSource = new WaveIn();
			waveSource.DeviceNumber = deviceNumber;
			waveSource.BufferMilliseconds = BUFFER_MSECS;
			waveSource.WaveFormat = new WaveFormat(SAMPLE_RATE, 1);

			waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
			waveSource.StartRecording();
		}

		private static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
		{
			int BytesRecorded = Model.Tools.TruncToPow2(e.BytesRecorded);
			Model.Complex[] complexData = new Model.Complex[BytesRecorded];

			for (int i = 0; i < BytesRecorded; i++)
			{
				complexData[i] = new Model.Complex(e.Buffer[i], 0);
			}

			//Получаем массив комплексных чисел.
			Model.FourierTransform.FFT(complexData, Model.FourierTransform.Direction.Forward);

			//Находим магнитуду каждого элемента в массиве

			for (int i = 0; i < BytesRecorded/2 - 1; i++)
			{
				complexData[i].Re = complexData[i].Magnitude;
			}


			//Находим индекс максимальной магнитуды в массиве
			double max_magnitude = 0;
			int index = 0;
			double t_maxfreq = MAX_FREQ * BytesRecorded / SAMPLE_RATE;

			for (int i = 1; i < t_maxfreq; i++)
			{
				if (complexData[i].Re > max_magnitude)
				{
					index = i;
					max_magnitude = complexData[i].Re;
				}
			}

			//Находим частоту: freq = max_index * Fs / N (Fs = 44100, N - size of FFT)
			max_magnitude = index * SAMPLE_RATE / BytesRecorded;
			OnFreqChanged(max_magnitude);
		}
	}
}