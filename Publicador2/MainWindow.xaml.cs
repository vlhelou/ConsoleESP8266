using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Publicador2
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		ViewModel.PrincipalVM viewmodel;
		View.PublicacaoPg publicacaoPagina = new View.PublicacaoPg();
		View.ComandoPg comandoPagina = new View.ComandoPg();
		public MainWindow()
		{
			InitializeComponent();
			viewmodel = new ViewModel.PrincipalVM();
			DataContext = viewmodel;
			publicacaoPagina.DataContext = viewmodel;

			publicacaoPagina = new View.PublicacaoPg();
			comandoPagina = new View.ComandoPg();
			viewmodel.BarraArquivo = publicacaoPagina.pbArquivo;
			viewmodel.BarraLinha = publicacaoPagina.pbLinha;
			comandoPagina.DataContext = viewmodel;

			publicacaoPagina.DataContext = viewmodel;
			viewmodel.ListaSeriais();
			
			
		}

		private void btnNavega_Click(object sender, RoutedEventArgs e)
		{
			//FMApoio.Navigate(new View.PublicacaoPg(viewmodel));
			Button botao = (Button)sender;
			switch (botao.Name)
			{
				case "btnPublicacao":
					FMApoio.Navigate(publicacaoPagina);
					break;
				case "btnComando":
					FMApoio.Navigate(comandoPagina);
					break;
			}


		}

		private void TextBlock_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			scrSaida.ScrollToBottom();
		}

    }


}
