using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using System.ComponentModel;
using System.IO;

namespace Publicador2.Model
{
	public class ArquivoModel : INotifyPropertyChanged
	{
		public ArquivoModel(FileInfo arquivo)
		{
			Arquivo = arquivo;
			OnPropertyChanged(nameof(Cor));
		}

		public ArquivoModel(string arquivo)
		{
			Arquivo = new FileInfo(arquivo);
			OnPropertyChanged(nameof(Cor));
		}

		public FileInfo Arquivo { get; set; }

		public string Nome => Arquivo.Name;
		public DateTime DataAlteracao => Arquivo.LastWriteTime;


		public DateTime? DataPublicacao { get; set; }

		public string Cor {
			get
			{
				if (DataPublicacao == null || DataPublicacao > Arquivo.LastWriteTime)
					return "Red";
				return "Blue";
			}
		}
		public List<string> Publica()
		{
			List<string> retorno = new List<string>();
			retorno.Add($"file.remove(\"" + Arquivo.Name + "\");");
			retorno.Add($"file.open(\"" + Arquivo.Name + "\",\"w+\");");
			retorno.Add("w = file.writeline");
			using (StreamReader sr = Arquivo.OpenText())
			{
				string linha = "";
				while ((linha = sr.ReadLine()) != null)
				{
					linha = linha.Trim();
					retorno.Add($"w([==[{linha}]==]);");
				}
			}
			retorno.Add("file.close();");
			return retorno;

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
