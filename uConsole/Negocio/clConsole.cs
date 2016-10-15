using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using Windows.Devices.SerialCommunication;
using System.Diagnostics;
using System.Threading;
namespace uConsole.Negocio
{
	class clConsole : INotifyPropertyChanged
	{

		public int Velocidade { get; set; }
		public bool Conectado { get; set; }
		public string Diretorio { get; set; }
		public List<clArquivo> Arquivos { get; set; }
		public string Saida => _Saida.ToString();
		public bool NConectado => !Conectado;
		public bool EnLeitura = false;

		private System.Text.StringBuilder _Saida = new StringBuilder();
		private SerialDevice Porta;
		private DataWriter Grava;
		private DataReader Le;
		//private CancellationToken cancellationToken = new CancellationToken();

		private Object ReadCancelLock = new object();
		private CancellationTokenSource ReadCancellationTokenSource;

		public clConsole(CancellationTokenSource ReadCancel)
		{
			ReadCancellationTokenSource = ReadCancel;
		}

		
		//public Timer tmr;

		public async void Conecta(string id)
		{
			Porta = await SerialDevice.FromIdAsync(id);
			Debug.WriteLine(Porta?.PortName);
			if (Porta != null)
			{
				Conectado = true;
				OnPropertyChanged("txtConexao");
				OnPropertyChanged("Conectado");
				OnPropertyChanged("NConectado");
				Porta.BaudRate = 9600;
				Porta.DataBits = 8;
				Porta.StopBits = SerialStopBitCount.One;
				Porta.Parity = SerialParity.None;
				Porta.Handshake = SerialHandshake.None;
				Porta.ReadTimeout = TimeSpan.FromMilliseconds(1000);
				Porta.WriteTimeout = TimeSpan.FromMilliseconds(1000);
				Le = new DataReader(Porta.InputStream);
				Grava = new DataWriter(Porta.OutputStream);
				//tmr = new Timer(this.Verifica, new AutoResetEvent(false), 1000, 1000);
			}


		}

		public void DesConecta()
		{
			Conectado = false;
			OnPropertyChanged("txtConexao");
			OnPropertyChanged("Conectado");
			OnPropertyChanged("NConectado");
			Porta.Dispose();
		}

		private  void Verifica(object status)
		{
			 Lendo();
		}

		public async void Envia(string comando)
		{
			Grava.WriteString(comando+"\r\n");
			await Grava.StoreAsync();
			//await Lendo(ReadCancellationTokenSource.Token);
			Lendo();

		}

		public async void Lendo()
		{
			if (!EnLeitura)
			{
				EnLeitura = true;
				ReadCancellationTokenSource = new CancellationTokenSource();

				Task<UInt32> LoadAsyncTask;
				uint ReadBufferLength = 1024;
				Le.InputStreamOptions = InputStreamOptions.Partial;
				LoadAsyncTask = Le.LoadAsync(ReadBufferLength).AsTask(ReadCancellationTokenSource.Token);
				UInt32 BytesRead = await LoadAsyncTask;
				if (BytesRead > 0)
				{
					byte[] leitura = new byte[BytesRead];
					Le.ReadBytes(leitura);
					_Saida.Append(Encoding.ASCII.GetString(leitura));
					OnPropertyChanged("Saida");
				}
				EnLeitura = false;
			} else
			{
				Debug.WriteLine("ainda lendo");
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
