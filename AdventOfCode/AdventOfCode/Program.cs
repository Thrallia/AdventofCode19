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
			//Aoc1(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC1.txt");
			//AoC2(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC2.txt");
			//AoC3(@"C:\develop\AdventofCode\inputs\AoC3.txt");
			//AoC4(235741, 706948);
			AoC5(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC5.txt");
		}

		private static void AoC5(string path)
		{
			List<int> positions = IntCode(path);

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
						positions[address] = positions[noun] + positions[verb];
						break;
					case 2:
						positions[address] = positions[noun] * positions[verb];
						break;
					case 3:
						break;
					case 4:
						break;
					case 99: break;
					default: break;
				}
			}
			while (positions[index] != 99);
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

		private static bool calcPassGroups(char[] digits)
		{
			var counts = digits.GroupBy(x => x).Select(x => x.Count());
			if (counts.Contains(2)) //if(counts.Max > 1) //Part 1
				return true;
			else
				return false;
		}

		private static bool calcPassAsc(char[] digits)
		{
			for (int i = 1; i < digits.Length; i++)
			{
				if (digits[i] - digits[i - 1] < 0)
				{
					return true;
				}
			}
			return false;
		}

		public static int CountPatterns(string text, string pattern)
		{
			// Loop through all instances of the string 'text'.
			int count = 0;
			int i = 0;
			while ((i = text.IndexOf(pattern, i)) != -1)
			{
				i += pattern.Length;
				count++;
			}
			return count;
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

		private static void AoC4(int min, int max)
		{
			int count = 0;
			while (min < max)
			{
				string comp = min.ToString();

				var digits = comp.ToCharArray();

				bool dub = calcPassGroups(digits);
				if (dub)
				{
					bool desc = calcPassAsc(digits);
					if (!desc)
						count++;
				}

				min++;
			}
			Console.WriteLine(count);
			Console.ReadKey();
		}

		private static void AoC3(string path)
		{
			List<string> wires = new List<string>();
			List<string> moves1 = new List<string>();
			List<string> moves2 = new List<string>();
			List<Point> wire1 = new List<Point>();
			List<Point> wire2 = new List<Point>();
			using (StreamReader file = new StreamReader(path))
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

		private static void AoC2(string path)
		{
			List<int> positions = IntCode(path);

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

		private static void Aoc1(string path)
		{
			int total = 0;
			using (StreamReader file = new StreamReader(path))
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
