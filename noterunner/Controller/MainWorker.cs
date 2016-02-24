using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace noterunner.Controller
{
	class MainWorker
	{
		public static void DeviceChosen(int inDeviceNumber)
		{
			SoundCapture.deviceNumber = inDeviceNumber >= 0 ? inDeviceNumber : 0;
		}

		//[STAThread]
		public static void Main()
		{
			SoundCapture.Initialize();

			//Choose input device
			View.DeviceList devWin = new View.DeviceList();
			if (devWin.ShowDialog() == true) SoundCapture.deviceNumber = devWin.selectedDevice >= 0 ? devWin.selectedDevice : 0;

			MainWindow mainWin = new MainWindow();
			mainWin.ShowDialog();
		}
	}
}