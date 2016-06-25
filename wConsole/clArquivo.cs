using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wConsole
{
	class clArquivo
	{
		public DateTime? AtualizaoEm { get; set; }
		public FileInfo Arquivo { get; set; }
		public bool Verificado { get; set; }
		public string Nome => Arquivo.Name;
		public bool Publicavel
		{
			get
			{
				//return true;
				if (AtualizaoEm == null)
					return true;
				if (File.GetLastWriteTime(Arquivo.FullName) > AtualizaoEm)
					return true;
				return false;

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
			AtualizaoEm = DateTime.Now;
			return retorno;
		}

	}
}
