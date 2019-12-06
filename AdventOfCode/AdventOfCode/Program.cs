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
			//AoC5(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC5.txt");
			AoC6(@"C:\develop\AdventofCode\inputs\AoC6.txt");
		}

		private static void AoC6(string path)
		{
			int orbits = 0;
			using (StreamReader file = new StreamReader(path))
			{
				string line = "";

				List<Orb> orbitals = new List<Orb>();
				orbitals.Add(new Orb("COM"));
				while (!file.EndOfStream)
				{
					line = file.ReadLine();

					var orbs = line.Split(')');

					orbitals.Add(new Orb(orbs[1],orbs[0]));
				}

				Console.ReadKey();
			}
		}

		internal class Orb
		{
			public string name;
			public string primary;

			public Orb(string name, string primary = null)
			{
				this.name = name;
				this.primary = primary;
			}
		}

		private static int Orbits(Orb orb)
		{

		}

		private static int IntCodeComputer(List<int> positions)
		{
			int index = 0;
			int noun;
			int verb;
			int address;
			int opcode;

			int output = 0;

			do
			{
				opcode = positions[index];
				noun = positions[index + 1];
				verb = positions[index + 2];

				switch (opcode)
				{
					case 1001:
						address = positions[index + 3];
						positions[address] = positions[noun] + verb;
						index += 4;
						break;
					case 1101:
						address = positions[index + 3];
						positions[address] = noun + verb;
						index += 4;
						break;
					case 1002:
						address = positions[index + 3];
						positions[address] = positions[noun] * verb;
						index += 4;
						break;
					case 1102:
						address = positions[index + 3];
						positions[address] = noun * verb;
						index += 4;
						break;
					case 101:
						address = positions[index + 3];
						positions[address] = noun + positions[verb];
						index += 4;
						break;
					case 102:
						address = positions[index + 3];
						positions[address] = noun * positions[verb];
						index += 4;
						break;
					case 1:
						address = positions[index + 3];
						positions[address] = positions[noun] + positions[verb];
						index += 4;
						break;
					case 2:
						address = positions[index + 3];
						positions[address] = positions[noun] * positions[verb];
						index += 4;
						break;
					case 3:
						address = positions[index + 1];
						var input = Console.ReadLine();
						positions[address] = Int32.Parse(input);
						index += 2;
						break;
					case 4:
						address = positions[index + 1];
						output = positions[address];
						Console.WriteLine(output);
						index += 2;
						break;
					case 104:
						address = positions[index + 1];
						output = address;
						Console.WriteLine(output);
						index += 2;
						break;
					case 5:
						if (positions[noun] != 0)
							index = positions[verb];
						else
							index += 3;
						break;
					case 105:
						if (noun != 0)
							index = positions[verb];
						else
							index += 3;
						break;
					case 1005:
						if (positions[noun] != 0)
							index = verb;
						else
							index += 3;
						break;
					case 1105:
						if (noun != 0)
							index = verb;
						else
							index += 3;
						break;
					case 6:
						if (positions[noun] == 0)
							index = positions[verb];
						else
							index += 3;
						break;
					case 106:
						if (noun == 0)
							index = positions[verb];
						else
							index += 3;
						break;
					case 1006:
						if (positions[noun] == 0)
							index = verb;
						else
							index += 3;
						break;
					case 1106:
						if (noun == 0)
							index = verb;
						else
							index += 3;
						break;
					case 7:
						address = positions[index + 3];
						if (positions[noun] < positions[verb])
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 107:
						address = positions[index + 3];
						if (noun < positions[verb])
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 1007:
						address = positions[index + 3];
						if (positions[noun] < verb)
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 1107:
						address = positions[index + 3];
						if (noun < verb)
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 8:
						address = positions[index + 3];
						if (positions[noun] == positions[verb])
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 108:
						address = positions[index + 3];
						if (noun == positions[verb])
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 1008:
						address = positions[index + 3];
						if (positions[noun] == verb)
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 1108:
						address = positions[index + 3];
						if (noun == verb)
							positions[address] = 1;
						else
							positions[address] = 0;
						index += 4;
						break;
					case 99: break;
					default: break;
				}
			}
			while (positions[index] != 99);

			return positions[0];
		}

		private static List<int> IntCodeInput(string path)
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

		private static void AoC5(string path)
		{
			List<int> positions = IntCodeInput(path);
			IntCodeComputer(positions);

			Console.ReadKey();
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
			List<int> positions = IntCodeInput(path);

			int output = IntCodeComputer(positions);

			int grav = 100 * positions[1] + positions[2];
			if (output == 19690720)
				Console.WriteLine(grav);

			Console.WriteLine("address 0: " + output);
			Console.WriteLine("gravity assist: " + grav);
			Console.ReadKey();
		}

		private static void Aoc1(string path)
		{
			int total = 0;
			using (StreamReader file = new StreamReader(path))
			{
				string line = "";
				while (!file.EndOfStream)
				{
					line = file.ReadLine();
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
