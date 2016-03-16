﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace noterunner.Controller
{
	class MainWorker
	{

		//[STAThread]
		public static void Main()
		{
			SoundCapture.Initialize();

			//Choose input device
			View.DeviceList devWin = new View.DeviceList();
			if (devWin.ShowDialog().Value == true) SoundCapture.deviceNumber = devWin.selectedDevice >= 0 ? devWin.selectedDevice : 0;

			MainWindow mainWin = new MainWindow();
			mainWin.Closed += OnWindowClosed;
			mainWin.ShowDialog();

		}

		private static void OnWindowClosed(object sender, EventArgs e)
		{
			SoundCapture.StopCapturing();
			Environment.Exit(0);
		}

		public static void StartRecording()
		{
			SoundCapture.StartCapturing();
		}

		internal static void StopRecording()
		{
			SoundCapture.StopCapturing();
		}
	}
}