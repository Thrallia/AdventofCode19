using System;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class IntCodeComputer
	{
		List<int> punchCards;
		public IntCodeComputer(string path)
		{
			string line = string.Empty;
			using (StreamReader file = new StreamReader(path))
			{
				line = file.ReadToEnd();
			}
			var pos = line.Split(',');
			foreach (string p in pos)
			{
				punchCards.Add(Int32.Parse(p));
				//Console.WriteLine(p);
			}
		}

		public void PrintCard()
		{
			foreach (var line in punchCards)
				Console.WriteLine(line);
		}
	}
}
