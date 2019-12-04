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
	class Program
	{
		static void Main(string[] args)
		{
			//Aoc1();
			//AoC2();
			//AoC3();
			AoC4(235741, 706948);
		}

		private static void AoC4(int min, int max)
		{
			int count = 0;
			while (min < max)
			{
				string comp = min.ToString();
				if (comp.Contains("00") || comp.Contains("11") || comp.Contains("22") ||
					comp.Contains("33") || comp.Contains("44") || comp.Contains("55") ||
					comp.Contains("66") || comp.Contains("77") || comp.Contains("88") || comp.Contains("99"))
				{
					var digits = comp.ToCharArray(); //NumbersIn(min).ToArray();
					bool desc = false;
					for (int i = 1; i < digits.Length; i++)
					{
						if (digits[i] - digits[i - 1] < 0)
						{
							desc = true;
							break;
						}

						if (i > 1)
						{
							if ((digits[i] - digits[i - 1] == 0) && (digits[i - 1] - digits[i - 2] == 0))
							{
								desc = true;
								break;
							}
						}
					}

					if (!desc)
						count++;
				}

				min++;
			}
			Console.WriteLine(count);
			Console.ReadKey();
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

		private static List<Point> CalcPath(List<string> moves)
		{
			List<Point> wire = new List<Point>();
			wire.Add(new Point(0, 0));
			foreach (var move in moves)
			{
				var curPoint = wire[wire.Count() - 1];
				var dir = move.Substring(0, 1);
				int distance = Int32.Parse(move.Substring(1));
				switch (dir)
				{
					case "R":
						for (int i = 1; i <= distance; i++)
							wire.Add(new Point(curPoint.X + i, curPoint.Y));
						break;
					case "D":
						for (int i = 1; i <= distance; i++)
							wire.Add(new Point(curPoint.X, curPoint.Y - i));
						break;
					case "L":
						for (int i = 1; i <= distance; i++)
							wire.Add(new Point(curPoint.X - i, curPoint.Y));
						break;
					case "U":
						for (int i = 1; i <= distance; i++)
							wire.Add(new Point(curPoint.X, curPoint.Y + i));
						break;
					default: break;
				}
			}

			return wire;
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
		
		private static void AoC3()
		{
			List<string> wires = new List<string>();
			List<string> moves1 = new List<string>();
			List<string> moves2 = new List<string>();
			List<Point> wire1 = new List<Point>();
			List<Point> wire2 = new List<Point>();
			using (StreamReader file = new StreamReader(@"C:\develop\AdventofCode\inputs\AoC3.txt"))
			{
				while (!file.EndOfStream)
				{
					wires.Add(file.ReadLine());
				}
			}

			moves1 = wires[0].Split(',').ToList();
			moves2 = wires[1].Split(',').ToList();

			wire1 = CalcPath(moves1);
			wire2 = CalcPath(moves2);

			var crosses = wire1.Intersect(wire2).ToList();

			if (crosses.Count > 1)
			{
				crosses.RemoveAt(0);
				var manhattan = crosses.Select(x => Math.Abs(x.X) + Math.Abs(x.Y)).ToList();
				Console.WriteLine(manhattan.Min());

				List<int> steps = new List<int>();
				foreach (var cross in crosses)
				{
					int ind1 = wire1.IndexOf(cross);
					int ind2 = wire2.IndexOf(cross);
					steps.Add(ind1 + ind2);
				}

				Console.WriteLine(steps.Min());
			}

			Console.ReadKey();
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
