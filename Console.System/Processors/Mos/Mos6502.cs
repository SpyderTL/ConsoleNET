using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware.Processors.Mos
{
	public class Mos6502 : Component
	{
		public Bus Bus;
		public Clock Clock;

		private byte a;
		private byte x;
		private byte y;
		private ushort program;
		private byte stack;
		private byte flags;
		private Thread thread;

		public Mos6502(Bus bus, Clock clock)
		{
			Bus = bus;
			Clock = clock;

			thread = new Thread(Start);
			thread.Start();
		}

		private void Start()
		{
			while (true)
			{
				WaitForClock();
				Reset();
			}

			while (true)
			{
				WaitForClock();
				Execute();
			}
		}

		private void Reset()
		{
			Bus.Address = 0xfffc;
			Bus.Active = true;

			WaitForBus();

			program = (ushort)Bus.Value;
		}

		private void WaitForBus()
		{
			while (Bus.Active)
				Thread.Sleep(10);
		}

		private void WaitForClock()
		{
			while (Clock.Value)
				Thread.Sleep(10);

			while (!Clock.Value)
				Thread.Sleep(10);
		}

		private void Execute()
		{
			Bus.Address = program;
			Bus.Active = true;

			WaitForBus();

			var instruction = (byte)Bus.Value;

			Execute(instruction);
		}

		private void Execute(byte instruction)
		{
			switch (instruction)
			{
				case 0x00:
					break;
			}
		}
	}
}
