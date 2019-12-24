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
	//https://github.com/chuckries/AdventOfCode/blob/master/AdventOfCode.2019/IntCode.cs
	class IntCodeComputer
	{
		List<int> punchCards;
		private int pointer;
		private int noun;
		private int verb;
		private int opcode;
		private int address;
		public bool IsHalt;
		

		enum OpCode
		{
			Add=1,
			Multiply=2,
			Input=3,
			Output=4,
			JumpTrue=5,
			JumpFalse=6,
			LessThan=7,
			EqualTo=8,
			Halt=99
		}

		enum Mode
		{
			Position=0,
			Immediate=1
		}

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

			pointer = 0;
			noun = 0;
			verb = 0;
			opcode = 0;
			address = 0;
			IsHalt = false;
		}

		public int Run(string input = null)
		{
			int output = 0;

			do
			{
				opcode = punchCards[pointer];
				noun = punchCards[pointer + 1];
				verb = punchCards[pointer + 2];

				switch (opcode)
				{
					case 1001:
						address = punchCards[pointer + 3];
						Add(punchCards[noun], verb, punchCards[address]);
						pointer += 4;
						break;
					case 1101:
						address = punchCards[pointer + 3];
						Add(noun, verb, punchCards[address]);
						pointer += 4;
						break;
					case 1002:
						address = punchCards[pointer + 3];
						Multiply(punchCards[noun], verb, punchCards[address]);
						pointer += 4;
						break;
					case 1102:
						address = punchCards[pointer + 3];
						Multiply(noun, verb, punchCards[address]);
						pointer += 4;
						break;
					case 101:
						address = punchCards[pointer + 3];
						Add(noun, punchCards[verb], punchCards[address]);
						pointer += 4;
						break;
					case 102:
						address = punchCards[pointer + 3];
						Multiply(noun, punchCards[verb], punchCards[address]);
						pointer += 4;
						break;
					case 1:
						address = punchCards[pointer + 3];
						Add(punchCards[noun], punchCards[verb], punchCards[address]);
						pointer += 4;
						break;
					case 2:
						address = punchCards[pointer + 3];
						Multiply(punchCards[noun], punchCards[verb], punchCards[address]);
						pointer += 4;
						break;
					case 3:
						address = punchCards[pointer + 1];
						if (input == null)
						{
							var read = output.ToString();   //Console.ReadLine();
							Input(Int32.Parse(read), punchCards[address]);
						}
						else
						{
							Input(Int32.Parse(input), punchCards[address]);
							input = "";
						}
						pointer += 2;
						break;
					case 4:
						address = punchCards[pointer + 1];
						output = Output(punchCards[address]);
						Console.WriteLine(output);
						pointer += 2;
						break;
					case 104:
						address = punchCards[pointer + 1];
						output = Output(address);
						Console.WriteLine(output);
						pointer += 2;
						break;
					case 5:
						JumpTrue(punchCards[noun],punchCards[verb]);
						break;
					case 105:
						JumpTrue(noun ,punchCards[verb]);
						break;
					case 1005:
						JumpTrue(punchCards[noun],verb);
						break;
					case 1105:
						JumpTrue(noun,verb);
						break;
					case 6:
						JumpFalse(punchCards[noun],punchCards[verb]);
						break;
					case 106:
						JumpFalse(noun, punchCards[verb]);
						break;
					case 1006:
						JumpFalse(punchCards[noun], verb);
						break;
					case 1106:
						JumpFalse(noun, verb);
						break;
					case 7:
						address = punchCards[pointer + 3];
						LessThan(punchCards[noun], punchCards[verb], address);
						pointer += 4;
						break;
					case 107:
						address = punchCards[pointer + 3];
						LessThan(noun, punchCards[verb], address);
						pointer += 4;
						break;
					case 1007:
						address = punchCards[pointer + 3];
						LessThan(punchCards[noun], verb, address);
						pointer += 4;
						break;
					case 1107:
						address = punchCards[pointer + 3];
						LessThan(noun, verb, address);
						pointer += 4;
						break;
					case 8:
						address = punchCards[pointer + 3];
						EqualTo(punchCards[noun], punchCards[verb], address);
						pointer += 4;
						break;
					case 108:
						address = punchCards[pointer + 3];
						EqualTo(noun, punchCards[verb], address);
						pointer += 4;
						break;
					case 1008:
						address = punchCards[pointer + 3];
						EqualTo(punchCards[noun], verb, address);
						pointer += 4;
						break;
					case 1108:
						address = punchCards[pointer + 3];
						EqualTo(noun, verb, address);
						pointer += 4;
						break;
					case 99: break;
					default: break;
				}
			}
			while (punchCards[pointer] != 99);

			return output;//punchCards[0];
		}

		public void Add(int x, int y, int location)
		{
			punchCards[location] = x + y;
		}

		public void Multiply(int x, int y, int location)
		{
			punchCards[location] = x * y;
		}

		public void Input(int x, int location)
		{
			punchCards[location] = x;
		}

		public int Output(int location)
		{
			return punchCards[location];
		}

		public void JumpTrue(int x, int location)
		{
			if (x != 0)
				pointer = location;
			else
				pointer += 3;
		}

		public void JumpFalse(int x, int location)
		{
			if (x == 0)
				pointer = location;
			else
				pointer += 3;
		}

		public void LessThan(int x, int y, int location)
		{
			if (x < y)
				punchCards[location] = 1;
			else
				punchCards[location] = 0;
		}

		public void EqualTo(int x, int y, int location)
		{
			if (x == y)
				punchCards[location] = 1;
			else
				punchCards[location] = 0;
		}

		public void PrintCard()
		{
			foreach (var line in punchCards)
				Console.WriteLine(line);
		}
	}
}
