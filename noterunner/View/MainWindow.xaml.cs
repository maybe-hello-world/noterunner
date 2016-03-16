using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using noterunner.View;
using noterunner.Controller;

namespace noterunner
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();
			SoundCapture.OnFreqChanged += FreqChanged;
		}

		private void FreqChanged(double ifreq)
		{
			freq.Content = ifreq;
		}

		private void recordButton_Click(object sender, RoutedEventArgs e)
		{
			MainWorker.StartRecording();

			recordButton.Visibility = Visibility.Hidden;
			stopButton.Visibility = Visibility.Visible;
		}

		private void stopButton_Click(object sender, RoutedEventArgs e)
		{
			MainWorker.StopRecording();

			stopButton.Visibility = Visibility.Hidden;
			recordButton.Visibility = Visibility.Visible;
		}
	}
}