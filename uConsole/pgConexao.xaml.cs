using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Devices.SerialCommunication;
using Windows.Devices.Enumeration;

using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uConsole
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class pgConexao : Page
	{
		private Negocio.clConsole Console = new Negocio.clConsole();
		private ObservableCollection<DeviceInformation> seriais = new ObservableCollection<DeviceInformation>();

		public pgConexao()
		{
			this.InitializeComponent();
			
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var teste = (Negocio.clConsole)e.Parameter;
			DataContext = teste;
			listaSeriais();
			cbSeriais.ItemsSource = seriais;
			//seriais[0].PortName
			
		}


		private async void listaSeriais()
		{
			seriais.Clear();
			var aqs = SerialDevice.GetDeviceSelector();
			foreach(var dis in await DeviceInformation.FindAllAsync(aqs))
			{
				//SerialDevice item = await SerialDevice.FromIdAsync(dis.Id);
				seriais.Add(dis);
			}
			//var dis = await DeviceInformation.FindAllAsync(aqs);
			
			
		}

		private void Refresh_Click(object sender, RoutedEventArgs e)
		{
			listaSeriais();
		}
	}
}
