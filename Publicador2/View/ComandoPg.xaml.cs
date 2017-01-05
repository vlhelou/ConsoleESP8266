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

namespace Publicador2.View
{
	/// <summary>
	/// Interaction logic for ComandoPg.xaml
	/// </summary>
	public partial class ComandoPg : Page
	{
		ViewModel.PrincipalVM viewmodel;

		public ComandoPg()
		{
			InitializeComponent();
		}

		public ComandoPg(ViewModel.PrincipalVM Model)
		{
			viewmodel = Model;
			DataContext = viewmodel;
		}

	}
}
