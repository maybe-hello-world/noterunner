using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace noterunner.Controller
{
	class SoundCapture
	{
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

		public static void StartCapturing()
		{
			waveSource = new WaveIn();
		}
	}
}