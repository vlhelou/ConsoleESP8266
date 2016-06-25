using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace wConsole
{
	class clDiretorio
	{
		public IList<clArquivo> Arquivos = new List<clArquivo>();
		private string _Diretorio;
		public string Diretorio
		{
			get { return _Diretorio; }
			set
			{
				_Diretorio = value;
				Arquivos.Clear();
				if (System.IO.Directory.Exists(_Diretorio))
					Atualiza();
			}
		}

		public void Atualiza()
		{
			foreach (var item in Arquivos)
				item.Verificado = false;
			foreach (string file in System.IO.Directory.GetFiles(Diretorio, "*.lua"))
			{
				FileInfo arq = new FileInfo(file);
				if (Arquivos.Where(p => p.Arquivo.Name == arq.Name).Count() == 0)
				{
					clArquivo item = new clArquivo()
					{
						AtualizaoEm = null,
						Arquivo = arq,
						Verificado = true
					};
					Arquivos.Add(item);
				}
				else
				{
					Arquivos.Where(p => p.Arquivo.Name == arq.Name).FirstOrDefault().Verificado = true;
				}
			}
			while (Arquivos.Where(p => p.Verificado == false).Count() != 0)
			{
				Arquivos.Remove(Arquivos.Where(p => p.Verificado == false).FirstOrDefault());
			}

		}


	}
}
