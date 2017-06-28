using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
namespace Publicador2.ViewModel
{
	public class PrincipalVM : INotifyPropertyChanged
	{

		public PrincipalVM()
		{
			string origem = System.AppDomain.CurrentDomain.BaseDirectory + "config.json";
			if (System.IO.File.Exists(origem))
			{
				JObject origemComando = JObject.Parse(System.IO.File.ReadAllText(origem));
				List<Model.ComandosModel> prelista = new List<Model.ComandosModel>();
				foreach (var cmd in origemComando["Comandos"])
				{
					prelista.Add(cmd.ToObject<Model.ComandosModel>());
				}
				Comandos = new ObservableCollection<Model.ComandosModel>(prelista.OrderBy(p=>p.Nome).ToList());
				OnPropertyChanged(nameof(Comandos));
			}
		}

		private bool _livre = true;

		#region Comandos
		private string _LinhaComando = "";

		public string LinhaComando
		{
			get { return _LinhaComando; }
			set
			{
				_LinhaComando = value;
				OnPropertyChanged(nameof(LinhaComando));
			}
		}

		public ICommand EnviaComando
		{
			get
			{
				return new CommandHandler(() => this.ClickEnviaComando());
			}
		}

		public void ClickEnviaComando()
		{
			if (Conexao != null)
			{

				Envia(LinhaComando);
			}
		}

		public Model.ComandosModel SelecionaComando {
			
			set
			{
				_LinhaComando = value.Valor;
				OnPropertyChanged(nameof(LinhaComando));
				ClickEnviaComando();
			}
		}
		public ObservableCollection<Model.ComandosModel> Comandos { get; set; } = new ObservableCollection<Model.ComandosModel>();

		#endregion


		#region BarraSuperior
		private StringBuilder _SaidaConsole = new StringBuilder();

		public string SaidaConsole => _SaidaConsole.ToString();

		public DateTime DataPublicacao { get; set; }
		public string Diretorio { get; set; }


		#endregion

		#region Conexao

		public bool Conectado { get; set; }
		public bool NConectado => !Conectado;

		public Visibility ShowBtnRefresh { get; set; } = Visibility.Visible;
		public Visibility ShowBtnConecta { get; set; } = Visibility.Collapsed;
		public Visibility ShowBtnDesconecta { get; set; } = Visibility.Collapsed;

		public ObservableCollection<string> PortasSerais { get; set; } = new ObservableCollection<string>();

		private string[] _Portas;
		public void ListaSeriais()
		{
			_Portas = SerialPort.GetPortNames();
			PortasSerais.Clear();
			PortasSerais = new ObservableCollection<string>(_Portas);
			OnPropertyChanged(nameof(PortasSerais));
		}

		private string _Porta;
		public string Porta {
			get { return _Porta; }
			set
			{
				if (_Porta != value)
				{
					_Porta = value;
					if (string.IsNullOrEmpty(_Porta))
					{
						ShowBtnConecta = Visibility.Collapsed;
					} else
					{
						ShowBtnConecta = Visibility.Visible;
					}
					OnPropertyChanged(nameof(ShowBtnConecta));
					OnPropertyChanged(nameof(Porta));
				}
			}
		}


		private SerialPort Conexao;

		public ICommand Refresh
		{
			get
			{
				return new CommandHandler(() => this.RefreshClick());
			}
		}
		public void RefreshClick()
		{
			ListaSeriais();
		}

		public ICommand Conecta
		{
			get
			{
				return new CommandHandler(() => this.ConectaClick());
			}
		}

		private void ConectaClick()
		{
			Conexao = new SerialPort(Porta, 9600);
			if (!Conexao.IsOpen)
			{
				Conexao.Open();
				for (int i = 0; i <= 50; i++)
				{
					if (Conexao.IsOpen)
					{
						Conectado = true;
						ShowBtnConecta = Visibility.Collapsed;
						ShowBtnDesconecta = Visibility.Visible;
						ShowBtnRefresh = Visibility.Collapsed;
						OnPropertyChanged(nameof(ShowBtnConecta));
						OnPropertyChanged(nameof(ShowBtnDesconecta));
						OnPropertyChanged(nameof(ShowBtnRefresh));
						OnPropertyChanged(nameof(Conectado));
						OnPropertyChanged(nameof(NConectado));
						break;
					}

					Thread.Sleep(100);
				}

				Conexao.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

			}

		}

		public ICommand DesConecta
		{
			get
			{
				return new CommandHandler(() => this.DesConectaClick());
			}
		}

		private void DesConectaClick()
		{
			if (Conexao.IsOpen)
			{
				Conexao.Close();
				Conexao.Dispose();
				Conectado = false;
				ShowBtnConecta = Visibility.Visible;
				ShowBtnDesconecta = Visibility.Collapsed;
				ShowBtnRefresh = Visibility.Visible;
				OnPropertyChanged(nameof(ShowBtnConecta));
				OnPropertyChanged(nameof(ShowBtnDesconecta));
				OnPropertyChanged(nameof(ShowBtnRefresh));
				OnPropertyChanged(nameof(Conectado));
				OnPropertyChanged(nameof(NConectado));



			}

		}

		private void DataReceivedHandler(
							   object sender,
							   SerialDataReceivedEventArgs e)
		{
			_livre = false;
			SerialPort sp = (SerialPort)sender;
			string indata = sp.ReadExisting();
			_SaidaConsole.Append(indata);
			_livre = true;
			OnPropertyChanged(nameof(SaidaConsole));
		}
		public void Envia(string Mensagem)
		{
			//Retorno.Append(cn.ReadExisting());
			int ct = 0;
			//_livre = false;
			Conexao.Write(Mensagem + Environment.NewLine);
			while ((!_livre) || (ct < 100))
			{
				Thread.Sleep(1);
				ct++;
			}
		}

		#endregion


		#region Publicacao 

		public ProgressBar BarraArquivo { get; set; }
		public ProgressBar BarraLinha { get; set; }

		public Visibility ShowBtnPublicacao { get; set; } = Visibility.Hidden;

		public int ArquivosTotal { get; set; }
		public int ArquivosProcessados { get; set; }

		public int LinhasTotal { get; set; }
		public int LinhasProcessadas { get; set; }
		public ObservableCollection<Model.ArquivoModel> Arquivos { get; set; } = new ObservableCollection<Model.ArquivoModel>();

		public ICommand ListaDiretorio
		{
			get
			{
				return new CommandHandler(() => this.ClickListaDiretorio());
			}
		}

		public void ClickListaDiretorio()
		{
			var dialog = new System.Windows.Forms.FolderBrowserDialog();

			dialog.RootFolder = Environment.SpecialFolder.Desktop;
			dialog.SelectedPath = @"E:\NodeMcu\Fontes\I2C";
			System.Windows.Forms.DialogResult result = dialog.ShowDialog();
			Diretorio = dialog.SelectedPath;
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				Arquivos.Clear();
				foreach (string arq in System.IO.Directory.GetFiles(Diretorio, "*.lua"))
				{
					Model.ArquivoModel item = new Model.ArquivoModel(arq);
					Arquivos.Add(item);
				}
				ShowBtnPublicacao = Visibility.Visible;

				OnPropertyChanged(nameof(Diretorio));
				OnPropertyChanged(nameof(Arquivos));
				OnPropertyChanged(nameof(ShowBtnPublicacao));

			}

		}


		public ICommand PublicaArquivos
		{
			get
			{
				return new CommandHandler(() => this.ClickPublicaArquivos());
			}
		}

		public void ClickPublicaArquivos()
		{
			List<Model.ArquivoModel> FilaPublicacao = new List<Model.ArquivoModel>();
			ArquivosProcessados = 1;
			ArquivosTotal = 0;
			OnPropertyChanged(nameof(ArquivosProcessados));

			foreach (string arquivo in System.IO.Directory.GetFiles(Diretorio, "*.lua"))
			{
				System.IO.FileInfo arq = new System.IO.FileInfo(arquivo);
				if (DataPublicacao == null || DataPublicacao < arq.LastWriteTime)
					FilaPublicacao.Add(new Model.ArquivoModel(arq));
			}
			ArquivosTotal = FilaPublicacao.Count;
			OnPropertyChanged(nameof(ArquivosTotal));
			foreach (var arquivo in FilaPublicacao)
			{
				var linhas = arquivo.Publica();
				LinhasTotal = linhas.Count();
				LinhasProcessadas = 1;
				OnPropertyChanged(nameof(LinhasTotal));
				OnPropertyChanged(nameof(LinhasProcessadas));
				foreach (string linha in linhas)
				{
					Envia(linha);

					LinhasProcessadas++;
					OnPropertyChanged(nameof(LinhasProcessadas));
					BarraLinha.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
				}


				ArquivosProcessados++;

				OnPropertyChanged(nameof(ArquivosProcessados));
				BarraArquivo.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => { }));
			}
			DataPublicacao = DateTime.Now;
		}

		#endregion


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
