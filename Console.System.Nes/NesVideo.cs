using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware.Nes
{
	public class NesVideo : Component
	{
		public Bus Bus;
		public byte[] Registers = new byte[8];

		private Thread thread;

		public NesVideo(Bus bus)
		{
			Bus = bus;

			thread = new Thread(Start);
			thread.Start();
		}

		private void Start()
		{
			while (Bus == null)
				Thread.Sleep(100);

			while (true)
			{
				while (!Bus.Active ||
					Bus.Address < 0x2000 ||
					Bus.Address > 0x3fff)
					Thread.Sleep(10);

				if (Bus.Write)
					Registers[Bus.Address & 0x0007] = (byte)Bus.Value;
				else
					Bus.Value = Registers[Bus.Address & 0x0007];

				Bus.Active = false;
			}
		}
	}
}
