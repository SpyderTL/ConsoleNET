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
		public byte A;
		public byte X;
		public byte Y;
		public ushort Program;
		public byte Stack;
		public byte Flags;
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
			WaitForClock();
			Reset();

			while (true)
			{
				WaitForClock();
				Execute();
			}
		}

		private void Reset()
		{
			A = 0;
			X = 0;
			Y = 0;
			Flags = 0;
			Stack = 0;

			Program = (ushort)Read(0xfffc);
			Program |= (ushort)(Read(0xfffd) << 8);
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
			var instruction = (byte)Read(Program);

			Execute(instruction);
		}

		private void Execute(byte instruction)
		{
			switch (instruction)
			{
				case 0x00:
					// Break
					Program++;
					break;

				case 0x09:
					// Or Accumulator With Immediate8
					A |= (byte)Read(Program + 1UL);

					Program += 2;
					break;

				case 0x10:
					// Branch To Relative8 If Positive
					if ((Flags & 0x80) != 0)
						Program = (ushort)(Program + (int)Read(Program + 1UL));
					else
						Program += 2;
					break;

				case 0x20:
					// Call Immediate16 Address
					var address = (ushort)Read(Program + 1UL);
					address |= (ushort)(Read(Program + 2UL) << 8);

					Stack -= 2;

					Write((ushort)(Stack + 0x100), (byte)((Program + 3) & 0xff));
					Write((ushort)(Stack + 0x101), (byte)((Program + 3) >> 8));

					Program = address;
					break;

				case 0x4c:
					// Jump To Immediate16 Address
					address = (ushort)Read(Program + 1UL);
					address |= (ushort)(Read(Program + 2UL) << 8);

					Program = address;
					break;

				case 0x58:
					// Clear Interrupt Disable Flag
					Flags &= 0xfb;
					Program++;
					break;

				case 0x6e:
					// Roll Immediate16 Address Right
					address = (ushort)Read(Program + 1UL);
					address |= (ushort)(Read(Program + 2UL) << 8);

					var value = (byte)Read(address);

					// TODO

					Program += 3;
					break;

				case 0x78:
					// Set Interrupt Disable Flag
					Flags |= 0x04;
					Program++;
					break;

				case 0x8d:
					// Copy Accumulator To Immediate16 Address
					address = (ushort)Read(Program + 1UL);
					address |= (ushort)(Read(Program + 2UL) << 8);

					Write(address, A);

					Program += 3;
					break;

				case 0x8e:
					// Copy Immediate8 To XIndex
					X = (byte)Read(Program + 1UL);
					Program += 2;
					break;

				case 0xa2:
					// Copy XIndex To Immediate16 Address
					address = (ushort)Read(Program + 1UL);
					address |= (ushort)(Read(Program + 2UL) << 8);

					Write(address, X);

					Program += 3;
					break;

				case 0xa9:
					// Copy Immediate8 To Accumulator
					A = (byte)Read(Program + 1UL);
					Program += 2;
					break;

				case 0xaa:
					// Copy Accumulator To XIndex
					X = A;

					Program++;
					break;

				case 0xad:
					// Copy Immediate16 Address To Accumulator
					address = (ushort)Read(Program + 1UL);
					address |= (ushort)(Read(Program + 2UL) << 8);

					A = (byte)Read(address);

					Program += 3;
					break;

				case 0xc5:
					// Compare Accumulator To Immediate8 Address
					address = (ushort)Read(Program + 1UL);

					int value2 = (byte)(A - (int)Read(address));

					if (value2 < 0)
						Flags |= 0x01;
					else
						Flags &= 0xfe;

					if (value2 == 0)
						Flags |= 0x02;
					else
						Flags &= 0xfd;

					Program += 2;
					break;

				case 0xd8:
					// Clear Decimal Flag
					Flags &= 0xf7;
					Program++;
					break;

				default:
					System.Diagnostics.Debugger.Break();
					break;
			}
		}

		private ulong Read(ulong address)
		{
			Bus.Address = address;
			Bus.Write = false;
			Bus.Active = true;

			while (Bus.Active)
				Thread.Sleep(10);

			return Bus.Value;
		}

		private void Write(ushort address, byte value)
		{
			Bus.Address = address;
			Bus.Value = value;
			Bus.Write = true;
			Bus.Active = true;

			while (Bus.Active)
				Thread.Sleep(10);
		}
	}
}
