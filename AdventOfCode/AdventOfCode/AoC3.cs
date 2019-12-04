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
	}
}
