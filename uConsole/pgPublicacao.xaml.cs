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
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace uConsole
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class pgPublicacao : Page
	{
		public pgPublicacao()
		{
			this.InitializeComponent();
		}

		public async void ListaDiretorio(object sender, RoutedEventArgs e)
		{
			OutputTextBlock.Text = "";

			FolderPicker folderPicker = new FolderPicker();
			folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
			folderPicker.FileTypeFilter.Add(".docx");
			folderPicker.FileTypeFilter.Add(".xlsx");
			folderPicker.FileTypeFilter.Add(".pptx");
			StorageFolder folder = await folderPicker.PickSingleFolderAsync();
			if (folder != null)
			{
				// Application now has read/write access to all contents in the picked folder (including other sub-folder contents)
				StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
				OutputTextBlock.Text = "Picked folder: " + folder.Path;
			}
			else
			{
				OutputTextBlock.Text = "Operation cancelled.";
			}

		}
	}
}
