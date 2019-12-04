using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class Program
	{
		static void Main(string[] args)
		{
			//Aoc1();
			//AoC2();
			AoC4(235741, 706948);
		}

		private static List<int> IntCode(string path)
		{

			List<int> positions = new List<int>();
			using (StreamReader file = new StreamReader(path))
			{
				string line = file.ReadToEnd();
				var pos = line.Split(',');
				foreach (string p in pos)
				{
					positions.Add(Int32.Parse(p));
					//Console.WriteLine(p);
				}
			}
			return positions;
		}

		static int calcFuel(double mass)
		{
			int fuel = (int)Math.Floor(mass / 3) - 2;
			if (fuel < 0)
				return 0;
			else
				return fuel;
		}

		private static Stack<int> NumbersIn(int value)
		{
			if (value == 0) return new Stack<int>();

			var numbers = NumbersIn(value / 10);

			numbers.Push(value % 10);

			return numbers;
		}

		private static void AoC4(int min, int max)
		{
			int count = 0;
			while (min < max)
			{
				var digits = NumbersIn(min).ToArray();
				min++;
			}

		}

		private static void AoC2()
		{

			string file = @"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC2.txt";
			List<int> positions = IntCode(file);

			int index = 0;
			int noun;
			int verb;
			int address;
			int op;
			int output = 0;
			int grav = 0;

			do
			{
				op = positions[index];
				noun = positions[index + 1];
				verb = positions[index + 2];
				address = positions[index + 3];

				switch (op)
				{
					case 1:
						output = positions[noun] + positions[verb];
						break;
					case 2:
						output = positions[noun] * positions[verb];
						break;
					case 99: break;
					default: break;
				}
				positions[address] = output;
				index += 4;


			}
			while (positions[index] != 99);

			grav = 100 * positions[1] + positions[2];
			if (positions[0] == 19690720)
				Console.WriteLine(grav);


			Console.WriteLine("address 0: " + positions[0]);
			Console.WriteLine("gravity assist: " + grav);
			Console.ReadKey();
		}

		private static void Aoc1()
		{
			int total = 0;
			using (StreamReader file = new StreamReader(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC1.txt"))
			{
				string line = "";
				while ((line = file.ReadLine()) != null)
				{
					double mass = double.Parse(line);
					int fuel = calcFuel(mass);
					total += fuel;
					Console.WriteLine(fuel);
					while (fuel > 0)
					{
						var extra = calcFuel((double)fuel);
						total += extra;
						fuel = extra;
					}


				}
				Console.WriteLine(total);
			}

			Console.ReadKey();
		}
	}
}
