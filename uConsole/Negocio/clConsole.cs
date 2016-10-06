using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using Windows.Devices.SerialCommunication;
using System.Diagnostics;
namespace uConsole.Negocio
{
	class clConsole : INotifyPropertyChanged
	{

		public int Velocidade { get; set; }
		public bool Conectado { get; set; }
		public string Diretorio { get; set; }
		public List<clArquivo> Arquivos { get; set; }


		private SerialDevice Porta;
		private DataWriter Grava;
		private DataReader Le;


		public string txtConexao => Conectado ? "Conectado" : "Não Conectado";


		public async void Conecta(string id)
		{
			Porta = await SerialDevice.FromIdAsync(id);
			Debug.WriteLine(Porta?.PortName);
			if (Porta != null)
			{
				Conectado = true;
				OnPropertyChanged("txtConexao");
				Porta.BaudRate = 9600;
				Porta.DataBits = 8;
				Porta.StopBits = SerialStopBitCount.One;
				Porta.Parity = SerialParity.None;
				Porta.Handshake = SerialHandshake.None;
				Porta.ReadTimeout = TimeSpan.FromMilliseconds(1000);
				Porta.WriteTimeout = TimeSpan.FromMilliseconds(1000);
				Le = new DataReader(Porta.InputStream);
				Grava = new DataWriter(Porta.OutputStream);

			}

		}


		protected void OnPropertyChanged(string propertyname)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;


	}
}
