﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publicador2.Model
{
	public class ComandosModel
	{
		public string Nome { get; set; }
		public string Valor { get; set; }

		public string Resumo => $"{Nome} ({Valor})";
	}
}
