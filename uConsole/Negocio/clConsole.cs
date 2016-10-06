using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uConsole.Negocio
{
	class clConsole : INotifyPropertyChanged
	{

		public int Velocidade { get; set; }
		public bool Conectado { get; set; }
		public string Diretorio { get; set; }
		public Windows.Devices.SerialCommunication.SerialDevice _Porta;
		public List<clArquivo> Arquivos { get; set; }


		public string txtConexao => Conectado ? "Conectado" : "Não Conectado";

		public Windows.Devices.SerialCommunication.SerialDevice Porta
		{
			get { return _Porta; }
			set
			{
				_Porta = value;
				OnPropertyChanged("Porta");
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
