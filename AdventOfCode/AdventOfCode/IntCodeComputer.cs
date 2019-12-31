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

	public class Punchcard
	{
		private List<long> punchCards;
		public Punchcard(List<long> code)
		{
			punchCards = code;
		}

		public long GetAddress(long address)
		{
			FillToAddress(address);
			return punchCards[(int)address];
		}

		public void SetAddress(Parameter target, long value)
		{
			long address = target.GetLiteral(this);
			FillToAddress(address);

			punchCards[(int)address] = value;
		}

		private void FillToAddress(long address)
		{
			if (punchCards.Count < address + 1)
				while (punchCards.Count < address + 1)
					punchCards.Add(0);
		}
	}
	class IntCodeComputer
	{
		public long LastOut;
		public Punchcard PC;
		public long Pointer;
		public int RelativeBase;

		public IntCodeComputer(List<long> instructions)
		{
			Pointer = 0;
			PC = new Punchcard(instructions);
		}

		private List<Parameter> GetParameters(Operator op, string opCode)
		{
			List<Parameter> param = new List<Parameter>();
			string modes = string.Empty;
			if (PC.GetAddress(Pointer).ToString().Length > 2)
				modes = opCode.Substring(0, opCode.Length - 2);

			for (int i = 0; i < op.ParamCount; i++)
				param.Add(new Parameter(PC.GetAddress(Pointer + i + 1), RelativeBase));

			if (modes != string.Empty)
			{
				modes = modes.PadLeft(3, '0');
				Stack<int> stack = new Stack<int>();
				for (int x = 0; x < 3; x++)
				{
					int mode;
					if (x < modes.Length)
						mode = modes[x];
					else
						mode = '0';
					//if (op.OpCode == OpCode.EqualTo)
					//	mode = '0';
					stack.Push(mode - '0');
				}

				foreach (var par in param)
					par.SetMode(stack.Pop());
			}

			return param;
		}

		private Operator GetOperator(string opCode)
		{
			if (opCode.Length > 2)
				return Operators.Ops[int.Parse(opCode.Substring(opCode.Length - 2))];

			return Operators.Ops[int.Parse(opCode)];
		}

		public OpCode Run(bool loop)
		{
			do
			{
				string opcode = PC.GetAddress(Pointer).ToString();
				Operator op = GetOperator(opcode);
				List<Parameter> parameters = GetParameters(op, opcode);
				int nextOp = op.ParamCount + 1;
				Pointer += nextOp;

				switch (op.OpCode)
				{
					case OpCode.Add:
						Add(parameters);
						break;
					case OpCode.Multiply:
						Multiply(parameters);
						break;
					case OpCode.Input:
						Input(parameters[0]);
						break;
					case OpCode.Output:
						Output(parameters[0]);
						break;
					case OpCode.JumpTrue:
						JumpTrue(parameters);
						break;
					case OpCode.JumpFalse:
						JumpFalse(parameters);
						break;
					case OpCode.LessThan:
						LessThan(parameters);
						break;
					case OpCode.EqualTo:
						EqualTo(parameters);
						break;
					case OpCode.SetBase:
						SetBase(parameters[0]);
						break;
					case OpCode.Halt:
						//loop = false;
						return op.OpCode;
					default: break;
				}
			}
			while (loop);
			return OpCode.Halt;
			//if (loop)
			//	return Run(loop);
			//else
			//	return op.OpCode;
		}

		public void Add(List<Parameter> param)
		{
			PC.SetAddress(param[2], param[0].GetResult(PC) + param[1].GetResult(PC));
		}

		public void Multiply(List<Parameter> param)
		{
			PC.SetAddress(param[2], param[0].GetResult(PC) * param[1].GetResult(PC));
		}

		public virtual void Input(Parameter param)
		{
			Console.WriteLine("Input");
			string line = Console.ReadLine();
			int val = int.Parse(line);
			Console.WriteLine("YOU ENTERED " + val);
			PC.SetAddress(param, val);
		}

		public virtual void Output(Parameter param)
		{
			Console.WriteLine(param.GetResult(PC));
			LastOut = param.GetResult(PC);
		}

		public void JumpTrue(List<Parameter> param)
		{
			if (param[0].GetResult(PC) != 0)
				Pointer = param[1].GetResult(PC);
		}

		public void JumpFalse(List<Parameter> param)
		{
			var x = param[0].GetResult(PC);
			if (x == 0)
				Pointer = param[1].GetResult(PC);
		}

		public void LessThan(List<Parameter> param)
		{
			if (param[0].GetResult(PC) < param[1].GetResult(PC))
				PC.SetAddress(param[2], 1);
			else
				PC.SetAddress(param[2], 0);
		}

		public void EqualTo(List<Parameter> param)
		{
			if (param[0].GetResult(PC) == param[1].GetResult(PC))
				PC.SetAddress(param[2], 1);
			else
				PC.SetAddress(param[2], 0);
		}

		public void SetBase(Parameter param)
		{
			RelativeBase += (int)param.GetResult(PC);
		}
	}

	class IntCodeComputerQueueInput : IntCodeComputer
	{
		public Queue<int> Inputs;

		public IntCodeComputerQueueInput(List<long> instructions) : base(instructions)
		{ }

		public override void Input(Parameter param)
		{
			int input = Inputs.Dequeue();
			PC.SetAddress(param, input);
		}

		public void Process(Queue<int> inputs)
		{
			Inputs = inputs;
			Run(true);
		}
	}

	class IntCodeComputerLooper : IntCodeComputerQueueInput
	{
		public bool Pause = false;
		public IntCodeComputerLooper(List<long> instructions) : base(instructions)
		{ }

		public override void Output(Parameter param)
		{
			LastOut = param.GetResult(PC);
			Pause = true;
		}
	}

	enum OpCode
	{
		Add,
		Multiply,
		Input,
		Output,
		JumpTrue,
		JumpFalse,
		LessThan,
		EqualTo,
		SetBase,
		Halt
	}

	public enum Mode
	{
		Position = 0,
		Immediate = 1,
		Relative = 2
	}

	class Operator
	{
		public int ParamCount;
		public OpCode OpCode;

		public Operator(OpCode op, int paramcount)
		{
			ParamCount = paramcount;
			OpCode = op;
		}
	}

	public class Parameter
	{
		private Mode Mode;
		public long Value;
		public int RelativeBase;

		public Parameter(Mode mode, long value, int relativeBase)
		{
			Mode = mode;
			Value = value;
			RelativeBase = relativeBase;
		}

		public Parameter(Mode mode, long value)
		{
			Mode = mode;
			Value = value;
		}

		public Parameter(long value, int relativeBase)
		{
			Mode = Mode.Position;
			Value = value;
			RelativeBase = relativeBase;
		}

		public long GetResult(Punchcard pc)
		{
			switch (Mode)
			{
				case Mode.Position:
					return pc.GetAddress(Value);
				case Mode.Immediate:
					return Value;
				case Mode.Relative:
					int newAdd = RelativeBase + (int)Value;
					return pc.GetAddress(newAdd);
				default: return -999;
			}
		}

		public long GetLiteral(Punchcard pc)
		{
			if (Mode == Mode.Position || Mode == Mode.Immediate)
				return Value;
			else
				return RelativeBase + Value;
		}

		public void SetMode(int mode)
		{
			Mode = (Mode)mode;
		}
	}

	class Operators
	{
		public static readonly Dictionary<int, Operator> Ops = new Dictionary<int, Operator>
		{
			{1, new Operator(OpCode.Add, 3)},
			{2, new Operator(OpCode.Multiply, 3)},
			{3, new Operator(OpCode.Input, 1)},
			{4, new Operator(OpCode.Output, 1)},
			{5, new Operator(OpCode.JumpTrue, 2)},
			{6, new Operator(OpCode.JumpFalse, 2)},
			{7, new Operator(OpCode.LessThan, 3)},
			{8, new Operator(OpCode.EqualTo, 3)},
			{9, new Operator(OpCode.SetBase, 1)},
			{99, new Operator(OpCode.Halt, 0)}
		};
	}
}
