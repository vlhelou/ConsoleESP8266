using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Publicador2.Model
{
	public class ArquivoModel 
	{
		public ArquivoModel(FileInfo arquivo)
		{
			Arquivo = arquivo;
		}
		public System.IO.FileInfo Arquivo { get; set; }
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
	}
}
