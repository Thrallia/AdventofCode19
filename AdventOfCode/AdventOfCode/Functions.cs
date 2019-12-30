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
	public static class Functions
	{
		//from day 12
		public static long LCM(long a, long b)
		{
			return a * b / GCD(a, b);
		}

		public static long GCD(long a, long b)
		{
			a = Math.Abs(a);
			b = Math.Abs(b);

			// Pull out remainders.
			for (; ; )
			{
				long remainder = a % b;
				if (remainder == 0) return b;
				a = b;
				b = remainder;
			};
		}

		//Got from Stackoverflow
		//from day 8
		public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
		{
			if (length == 1) return list.Select(t => new T[] { t });

			return GetPermutations(list, length - 1)
				.SelectMany(t => list.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new T[] { t2 }));
		}

		//from day 6
		public static void GetOrbits(Orb orb, List<Orb> orbitals, ref int count, string target = null)
		{
			if (target == null)     //Part 1
			{
				if (orb.primary != null)
				{
					count++;
					Orb next = orbitals.Single(x => x.name == orb.primary);
					GetOrbits(next, orbitals, ref count);
				}
			}
			else    //Part 2
			{
				if (orb.name != target)
				{
					count++;
					Orb next = orbitals.Single(x => x.name == orb.primary);
					GetOrbits(next, orbitals, ref count, target);
				}
			}
		}

		//from day 6
		public static void GetPrimaries(Orb orb, List<Orb> orbitals, ref List<Orb> primaries)
		{
			if (orb.primary != null)
			{
				Orb next = orbitals.Single(x => x.name == orb.primary);
				primaries.Add(next);
				GetPrimaries(next, orbitals, ref primaries);
			}
		}

		//from day 4
		public static bool calcPassGroups(char[] digits)
		{
			var counts = digits.GroupBy(x => x).Select(x => x.Count());
			if (counts.Contains(2)) //if(counts.Max > 1) //Part 1
				return true;
			else
				return false;
		}

		//from day 4
		public static bool calcPassAsc(char[] digits)
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

		//from day 3
		public static List<Point> CalcPath(List<string> moves)
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

		//from day 2
		public static List<long> IntCodeProgram(string path)
		{

			List<long> positions = new List<long>();
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

		//from day 1
		public static int calcFuel(double mass)
		{
			int fuel = (int)Math.Floor(mass / 3) - 2;
			if (fuel < 0)
				return 0;
			else
				return fuel;
		}
	}
}
