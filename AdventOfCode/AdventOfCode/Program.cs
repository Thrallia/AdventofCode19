using System;
using System.Collections;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AdventOfCode
{
	class Program
	{
		static void Main(string[] args)
		{
			//Aoc1(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC1.txt");
			//AoC2(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC2.txt");
			//AoC3(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC3.txt");
			//AoC4(235741, 706948);
			//AoC5(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC5.txt");
			//AoC6(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC6.txt");
			//AoC7(@"C:\develop\AdventofCode\inputs\AoC7.txt");
			//AoC8(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC8.txt");
			AoC9(@"C:\develop\AdventofCode\inputs\AoC9test2.txt");
			//AoC12(@"C:\Users\Thrallia\Documents\Github\AdventofCode19\AdventOfCode\inputs\AoC12.txt");
		}

		private static void AoC9(string path)
		{
			List<long> positions = Functions.IntCodeProgram(path);
			IntCodeComputer PC = new IntCodeComputer(positions);

			PC.Run(true);
			var output = PC.LastOut;

			Console.WriteLine(output);
			Console.ReadKey();
		}

		private static void AoC12(string path)
		{
			const int steps = 1000;

			int count = 1;
			int xConsonance = 0;
			int yConsonance = 0;
			int zConsonance = 0;

			Moon Io;
			Moon Europa;
			Moon Ganymede;
			Moon Callisto;

			Queue<Point3D> points = new Queue<Point3D>();
			using (StreamReader file = new StreamReader(path))
			{
				string line = string.Empty;

				while (!file.EndOfStream)
				{
					line = file.ReadLine();
					var split = line.Split('=', ',', '<', '>');
					points.Enqueue(new Point3D(Double.Parse(split[2]), Double.Parse(split[4]), Double.Parse(split[6])));
				}
			}

			//init moons
			Io = new Moon(points.Dequeue());
			Europa = new Moon(points.Dequeue());
			Ganymede = new Moon(points.Dequeue());
			Callisto = new Moon(points.Dequeue());

			//part 2
			while (xConsonance == 0 || yConsonance == 0 || zConsonance == 0)
			{
				//apply gravity
				Io.CalcVelocities(Europa);
				Io.CalcVelocities(Ganymede);
				Io.CalcVelocities(Callisto);
				Europa.CalcVelocities(Io);
				Europa.CalcVelocities(Ganymede);
				Europa.CalcVelocities(Callisto);
				Ganymede.CalcVelocities(Europa);
				Ganymede.CalcVelocities(Io);
				Ganymede.CalcVelocities(Callisto);
				Callisto.CalcVelocities(Europa);
				Callisto.CalcVelocities(Ganymede);
				Callisto.CalcVelocities(Io);

				//time step
				Io.TimeStep();
				Europa.TimeStep();
				Ganymede.TimeStep();
				Callisto.TimeStep();

				if (xConsonance == 0)
					if (Io.GetVelocityByAxis("X") == 0 && Europa.GetVelocityByAxis("X") == 0 &&
						Ganymede.GetVelocityByAxis("X") == 0 && Callisto.GetVelocityByAxis("X") == 0)
						xConsonance = count;
				if (yConsonance == 0)
					if (Io.GetVelocityByAxis("Y") == 0 && Europa.GetVelocityByAxis("Y") == 0 &&
						Ganymede.GetVelocityByAxis("Y") == 0 && Callisto.GetVelocityByAxis("Y") == 0)
						yConsonance = count;
				if (zConsonance == 0)
					if (Io.GetVelocityByAxis("Z") == 0 && Europa.GetVelocityByAxis("Z") == 0 &&
						Ganymede.GetVelocityByAxis("Z") == 0 && Callisto.GetVelocityByAxis("Z") == 0)
						zConsonance = count;

				count++;
			}

			long lcm = Functions.LCM(xConsonance, Functions.LCM(yConsonance, zConsonance)) * 2;

			Console.WriteLine(lcm);

			//part 1
			for (int i = 0; i < steps; i++)
			{
				//apply gravity
				Io.CalcVelocities(Europa);
				Io.CalcVelocities(Ganymede);
				Io.CalcVelocities(Callisto);
				Europa.CalcVelocities(Io);
				Europa.CalcVelocities(Ganymede);
				Europa.CalcVelocities(Callisto);
				Ganymede.CalcVelocities(Europa);
				Ganymede.CalcVelocities(Io);
				Ganymede.CalcVelocities(Callisto);
				Callisto.CalcVelocities(Europa);
				Callisto.CalcVelocities(Ganymede);
				Callisto.CalcVelocities(Io);

				//time step
				Io.TimeStep();
				Europa.TimeStep();
				Ganymede.TimeStep();
				Callisto.TimeStep();
			}

			var IoTotal = Io.CalcPotentialEnergy() * Io.CalcKineticEnergy();
			var EurTotal = Europa.CalcPotentialEnergy() * Europa.CalcKineticEnergy();
			var GanyTotal = Ganymede.CalcPotentialEnergy() * Ganymede.CalcKineticEnergy();
			var CallTotal = Callisto.CalcPotentialEnergy() * Callisto.CalcKineticEnergy();

			var TotalEnergy = IoTotal + EurTotal + GanyTotal + CallTotal;

			Console.WriteLine(TotalEnergy);
			Console.ReadKey();
		}

		private static void AoC8(string path)
		{
			const int width = 25;
			const int height = 6;
			//List<int> layer = new List<int>();
			List<List<int>> image = new List<List<int>>();

			var line = "";// "0222112222120000"; //test
			using (StreamReader file = new StreamReader(path))
			{

				while (!file.EndOfStream)
				{
					line = file.ReadLine();
				}
			}
			var buffer = line.ToCharArray().ToList();
			while (buffer.Count > 0)
			{
				List<int> layer = new List<int>();
				for (int i = 0; i < (width * height); i++)
				{
					var dig = buffer[0].ToString();
					layer.Add(Int32.Parse(dig));
					buffer.RemoveAt(0);
				}
				image.Add(layer);
			}

			List<int> pixels = new List<int>();
			for (int i = 0; i < (width * height); i++)
			{
				foreach (var layer in image)
				{
					if (layer[i] == 2)
					{
						continue;
					}
					else
					{
						pixels.Add(layer[i]);
						break;
					}
				}
			}

			string output = "";
			int j = 0;
			while (j < (height * width))
			{
				for (int k = 0; k < width; k++)
				{
					Console.Write(pixels[k + j]);
					output += pixels[k + j];
				}
				j += width;
				Console.Write("\n");
			}

			//part 1
			//var minZ = image.Select(x => x.Count(y => y == 0)).ToList();

			//int layerZ = minZ.IndexOf(minZ.Min());

			//var ones = image[layerZ].Count(x=> x==1);
			//var twos = image[layerZ].Count(x => x == 2);
			//Console.WriteLine(ones * twos);
			Console.ReadKey();
		}

		private static void AoC7(string path)
		{
			//Console.WriteLine(AoC7Part1(path));

			var amps = Functions.GetPermutations<int>(Enumerable.Range(5, 5), 5).ToList();
			List<long> thrusts = new List<long>();

			List<Amplifier> amplifiers;

			foreach (var amp in amps)
			{
				amplifiers=new List<Amplifier>();

				List<long> positions = Functions.IntCodeProgram(path);
				foreach (var input in amp)
				{
					Queue<int> inputs = new Queue<int>();
					inputs.Enqueue(input);
					amplifiers.Add(new Amplifier(inputs, positions));
				}

				long signal = 0;

				while (amplifiers.Last().Completed == false)
				{
					foreach (var amplifier in amplifiers)
					{
						amplifier.Continue((int)signal);

						while (!amplifier.Completed && !amplifier.PC.Pause)
						{
							OpCode lastOp = amplifier.PC.Run(false);

							if (lastOp == OpCode.Halt)
								amplifier.Completed = true;
						}

						signal = amplifier.PC.LastOut;
					}
				}

				thrusts.Add(signal);
			}

			Console.WriteLine(thrusts.Max());

			Console.ReadKey();
		}

		private static long AoC7Part1(string path)
		{
			var amps = Functions.GetPermutations<int>(Enumerable.Range(0, 5), 5).ToList();
			List<long> thrusts = new List<long>();

			foreach (var amp in amps)
			{
				List<long> positions = Functions.IntCodeProgram(path);
				long signal = 0;
				foreach (var input in amp)
				{
					IntCodeComputerQueueInput comp = new IntCodeComputerQueueInput(positions);
					Queue<int> inputs = new Queue<int>();
					inputs.Enqueue(input);
					inputs.Enqueue((int)signal);
					comp.Process(inputs);
					signal = comp.LastOut;
				}

				thrusts.Add(signal);
			}

			return thrusts.Max();
		}

		private static int IntCodeComputer(List<int> positions, List<string> input = null)
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
						if (input == null)
						{
							var read = output.ToString();   //Console.ReadLine();
							positions[address] = Int32.Parse(read);
						}
						else
						{
							positions[address] = Int32.Parse(input.First());
							input.RemoveAt(0);
							if (input.Count == 0)
								input = null;
						}
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

			return output;//positions[0];
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

					orbitals.Add(new Orb(orbs[1], orbs[0]));
				}

				foreach (var orb in orbitals) //Part 1
				{
					Functions.GetOrbits(orb, orbitals, ref orbits);
				}

				Orb you = orbitals.Single(x => x.name == "YOU");
				Orb santa = orbitals.Single(x => x.name == "SAN");
				List<Orb> yPrimaries = new List<Orb>();
				List<Orb> sPrimaries = new List<Orb>();
				int yOrbits = 0;
				int sOrbits = 0;

				Functions.GetPrimaries(you, orbitals, ref yPrimaries);
				Functions.GetPrimaries(santa, orbitals, ref sPrimaries);

				var intersects = yPrimaries.Intersect(sPrimaries);

				Orb commonPrimary = intersects.First();

				Functions.GetOrbits(you, orbitals, ref yOrbits, commonPrimary.name);
				Functions.GetOrbits(santa, orbitals, ref sOrbits, commonPrimary.name);

				Console.WriteLine(yOrbits + sOrbits - 2);
				Console.ReadKey();
			}
		}

		private static void AoC5(string path)
		{
			List<long> positions = Functions.IntCodeProgram(path);
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

				bool dub = Functions.calcPassGroups(digits);
				if (dub)
				{
					bool desc = Functions.calcPassAsc(digits);
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

			wire1 = Functions.CalcPath(moves1);
			wire2 = Functions.CalcPath(moves2);

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
			List<long> positions = Functions.IntCodeProgram(path);

			int output = IntCodeComputer(positions);

			long grav = 100 * positions[1] + positions[2];
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
					int fuel = Functions.calcFuel(mass);
					total += fuel;
					Console.WriteLine(fuel);
					while (fuel > 0)    //part 2
					{
						var extra = Functions.calcFuel((double)fuel);
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
