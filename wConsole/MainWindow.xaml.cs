using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wConsole
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		clSerial Serial = new clSerial();
		clDiretorio Diretorio = new clDiretorio();
		public MainWindow()
		{
			InitializeComponent();
			string[] portas = Serial.Portas();
			foreach (string porta in portas)
				cbPortas.Items.Add(porta);

		}

		private void btnConecta_Click(object sender, RoutedEventArgs e)
		{
			if (rdConectado.IsChecked != true)
			{
				Serial.Porta = cbPortas.SelectedItem.ToString();
				try
				{
					Serial.Conecta();
					//txtSaida.Text = Serial.Envia("=node.heap()");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			else
			{
				Serial.DesConecta();
			}
			rdConectado.IsChecked = Serial.Conectado;
			btnConecta.Content = Serial.Conectado ? "Desconecta" : "Conecta";
		}

		private void trMudaTexto(object sender, TextChangedEventArgs e)
		{
			if ((Diretorio != null) & gArquivos != null)
			{

			}
			Diretorio.Diretorio = txtDiretorio.Text;

			ConfiguraGrid();


		}

		private void btnPublica_Click(object sender, RoutedEventArgs e)
		{
			List<clArquivo> filaPublicacao = Diretorio.Arquivos.Where(p => p.Publicavel == true).ToList();
			Serial.Retorno.Clear();
			pgArquivo.Minimum = 0;
			pgArquivo.Maximum = filaPublicacao.Count;
			pgArquivo.Value = 0;
			StringBuilder pacote = new StringBuilder();
			foreach (var item in filaPublicacao)
			{
				List<string> linhas = item.Publica();
				pgConteudo.Minimum = 0;
				pgConteudo.Maximum = linhas.Count;
				pgConteudo.Value = 0;
				foreach (string linha in linhas)
				{
					Serial.Envia(linha + "\r\n");

					pgConteudo.Value += 1;
					Application.Current.Dispatcher.Invoke(
						DispatcherPriority.Background,
						new ThreadStart(delegate { }));

				}
				pgArquivo.Value += 1;

			}
			Serial.Envia("=node.restart()\r\n");
			Serial.Envia(txtStartUp.Text + "\r\n");
			Diretorio.Atualiza();
			ConfiguraGrid();
			txtSaida.Text = Serial.Retorno.ToString();
			//Serial.Envia("=node.restart()");
			//Serial.Envia(pacote.ToString());
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			txtSaida.Text = Serial.Retorno.ToString();

		}

		private void btnExecuta_Click(object sender, RoutedEventArgs e)
		{
			Serial.Envia(txtExecuta.Text + "\r\n");
			txtSaida.Text = Serial.Retorno.ToString();
		}

		private void ConfiguraGrid()
		{
			//Console.WriteLine(Diretorio.Arquivos[0].AtualizaoEm.ToString());
			gArquivos.ItemsSource = Diretorio.Arquivos;
			gArquivos.Items.Refresh();
			//gArquivos.Columns[0].Visibility = Visibility.Collapsed;
			gArquivos.Columns[1].Visibility = Visibility.Collapsed;
			gArquivos.Columns[2].Visibility = Visibility.Collapsed;

		}

		private void btnLimpaSaida_Click(object sender, RoutedEventArgs e)
		{
			Serial.Retorno.Clear();
			txtSaida.Text = Serial.Retorno.ToString();
		}
	}
}
