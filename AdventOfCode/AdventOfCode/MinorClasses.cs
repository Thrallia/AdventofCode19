using System;
using System.IO;
//using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace AdventOfCode
{
	//from day 12
	public class Moon
	{
		Point3D location { get; set; }
		Vector3D velocity { get; set; }

		public Moon(int x, int y, int z)
		{
			location = new Point3D(x, y, z);
			velocity = new Vector3D(0, 0, 0);
		}

		public Moon(Point3D loc)
		{
			location = loc;
			velocity = new Vector3D(0, 0, 0);
		}

		public void SetVelocity(int x, int y, int z)
		{
			velocity = new Vector3D(x, y, z);
		}

		public void SetVelocity(Vector3D v)
		{
			velocity = v;
		}

		public double CalcPotentialEnergy()
		{
			double total = 0;
			total = Math.Abs(location.X) + Math.Abs(location.Y) + Math.Abs(location.Z);

			return total;
		}

		public double CalcKineticEnergy()
		{
			double total = 0;
			total = Math.Abs(velocity.X) + Math.Abs(velocity.Y) + Math.Abs(velocity.Z);

			return total;
		}

		public double GetVelocityByAxis(string axis)
		{
			switch (axis)
			{
				case "X":
					return velocity.X;
				case "Y":
					return velocity.Y;
				case "Z":
					return velocity.Z;
				default:
					return .1;
			}
		}

		public void TimeStep()
		{
			location = Point3D.Add(location, velocity);
		}

		public void CalcVelocities(Moon moon)
		{
			var x = velocity.X;
			var y = velocity.Y;
			var z = velocity.Z;
			List<int> deltas = new List<int>();

			deltas.Add(this.location.X.CompareTo(moon.location.X));
			deltas.Add(this.location.Y.CompareTo(moon.location.Y));
			deltas.Add(this.location.Z.CompareTo(moon.location.Z));

			if (deltas[0] < 0)
				x++;
			else if (deltas[0] > 0)
				x--;

			if (deltas[1] < 0)
				y++;
			else if (deltas[1] > 0)
				y--;

			if (deltas[2] < 0)
				z++;
			else if (deltas[2] > 0)
				z--;

			SetVelocity(new Vector3D(x, y, z));
		}
	}

	//from day 11
	class Panel
	{
		Point coordinates;
		bool painted;
		PaintColors Color;
		enum PaintColors
		{
			Black,
			White
		}

		public Panel(Point coord, bool paint, int color)
		{
			coordinates = coord;
			painted = paint;
			Color = (PaintColors)color;
		}

		public Panel(Point coord, bool paint)
		{
			coordinates = coord;
			painted = paint;
			Color = PaintColors.Black;
		}

		public Panel(Point coord)
		{
			coordinates = coord;
			painted = false;
			Color = PaintColors.Black;
		}
	}

	class Robot
	{
		enum Direction
		{
			Up,
			Right,
			Down,
			Left
		}
		Vector direction;
		IntCodeComputerLooper cpu;
		Panel currentPanel;
		Point location;

		public Robot(List<long> instructions)
		{
			cpu = new IntCodeComputerLooper(instructions);
		}
	}

	//from day 7
	class Amplifier
	{
		public Queue<int> Inputs;
		public IntCodeComputerLooper PC;
		public bool Completed = false;

		public Amplifier(Queue<int> inputs)
		{
			Inputs = inputs;
		}

		public Amplifier(Queue<int> inputs, List<long> instructions)
		{
			Inputs = inputs;
			PC = new IntCodeComputerLooper(instructions);
			PC.Inputs = inputs;
		}

		public Queue<int> GetInputs()
		{
			return Inputs;
		}

		public void Continue(int input)
		{
			PC.Pause = false;
			Inputs.Enqueue(input);
		}
	}

	//from day 6
	public class Orb
	{
		public string name;
		public string primary;

		public Orb(string name, string primary = null)
		{
			this.name = name;
			this.primary = primary;
		}
	}
}
