using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using noterunner.Controller;

namespace noterunner.View
{
	/// <summary>
	/// Interaction logic for DeviceList.xaml
	/// </summary>
	public partial class DeviceList : Window
	{
		public int selectedDevice { get; set; }

		public DeviceList()
		{
			InitializeComponent();

			DeviceListbox.Items.Clear();
			for (int x = 0; x < SoundCapture.deviceCount; x++)
			{
				DeviceListbox.Items.Add(SoundCapture.availableDevices.ElementAt(x));
			}

			DeviceListbox.SelectedIndex = 0;

			if (App.Current.MainWindow == this) App.Current.MainWindow = null;
		}

		private void select_Click(object sender, RoutedEventArgs e)
		{
			selectedDevice = DeviceListbox.SelectedIndex;
			this.DialogResult = true;
		}
	}
}