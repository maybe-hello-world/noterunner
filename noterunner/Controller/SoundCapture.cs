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

		public delegate void FreqChanged(double freq);
		public static event FreqChanged OnFreqChanged;

		public static int deviceCount { get; private set; }
		public static List<string> availableDevices { get; private set; }

		private static WaveIn waveSource = null;
		public static int deviceNumber { get; set; } = 0;

		/// <summary>
		/// Initialize recording devices.
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

		internal static void StopCapturing()
		{
			waveSource.StopRecording();
		}

		/// <summary>
		/// Start capturing of data.
		/// </summary>
		public static void StartCapturing()
		{
			waveSource = new WaveIn();
			waveSource.DeviceNumber = deviceNumber;
			waveSource.WaveFormat = new WaveFormat(44100, 1);

			waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
			waveSource.StartRecording();
		}

		private static void waveSource_DataAvailable(object sender, WaveInEventArgs e)
		{
			//Model.Complex[] complexData = new Model.Complex[e.BytesRecorded];
			Model.Complex[] complexData = new Model.Complex[8192];
			//for (int i = 0; i < e.BytesRecorded; i++)
			for (int i = 0; i < 8192; i++)
			{
				complexData[i] = new Model.Complex(e.Buffer[i], 0);
			}

			//Получаем массив комплексных чисел.
			Model.FourierTransform.FFT(complexData, Model.FourierTransform.Direction.Forward);

			//Находим магнитуду каждого элемента в массиве
			//for (int i = 0; i < e.BytesRecorded / 2; i++)
			for (int i = 0; i < 8192/2; i++)
			{
				complexData[i].Re = complexData[i].Magnitude;
			}


			//Находим индекс максимальной магнитуды в массиве
			double max_magnitude = 0;
			int index = 0;
			double t_maxfreq = MAX_FREQ * 8192 / 44100;
			//for (int i = 0; i < e.BytesRecorded / 2; i++)
			//for (int i = 1; i < 8192 / 2 - 1; i++)
			for (int i = 1; i < t_maxfreq; i++)
			{
				if (complexData[i].Re > max_magnitude)
				{
					index = i;
					max_magnitude = complexData[i].Re;
				}
			}

			//Находим частоту: freq = max_index * Fs / N (Fs = 44100, N - size of FFT)
			max_magnitude = index * 44100 / 8192;
			OnFreqChanged(max_magnitude);
		}
	}
}