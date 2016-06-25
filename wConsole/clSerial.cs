using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace wConsole
{
	public class clSerial
	{

		public bool Conectado
		{
			get { return cn.IsOpen; }
		}

		private bool _livre = true;
		public bool Livre { get { return _livre; } } 
		public string Porta { get; set; }
		public StringBuilder Retorno { get; set; } 


		private SerialPort cn = new SerialPort();


		public clSerial()
		{
			cn.DataReceived += new SerialDataReceivedEventHandler(RecebeDados);
			Retorno = new StringBuilder();
		}

		public string[] Portas()
		{
			return SerialPort.GetPortNames();
		}


		public void Conecta()
		{
			if (cn.IsOpen)
				cn.Close();

			cn.PortName = Porta;
			cn.BaudRate = 9600;
			cn.Open();
			for (int i = 0; i <= 50; i++)
			{
				if (cn.IsOpen)
					break;
				Thread.Sleep(100);
			}
			if (!cn.IsOpen)
				throw new Exception("não conseguiu conectar");
		}

		public void Envia(string Mensagem)
		{
			//Retorno.Append(cn.ReadExisting());
			int ct = 0;
			_livre = false;
			cn.Write(Mensagem);
			while ((!_livre) || (ct < 100))
			{
				Thread.Sleep(10);
				ct++;
			}

		}

		public void DesConecta()
		{
			cn.Close();
		}

		private void RecebeDados(object sender, SerialDataReceivedEventArgs e)
		{
			SerialPort sp = (SerialPort)sender;
			string indata = sp.ReadExisting();
			if (indata.IndexOf(">")>=0)
				_livre = true;
			Retorno.Append(indata);
		}
	}
}
