using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware.Nes
{
	public class NesAudio : Component
	{
		public Bus Bus;
		public byte[] Registers = new byte[32];

		private Thread thread;

		public NesAudio(Bus bus)
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
					Bus.Address < 0x4000 ||
					Bus.Address > 0x401f)
					Thread.Sleep(10);

				if (Bus.Write)
					Registers[Bus.Address & 0x001f] = (byte)Bus.Value;
				else
					Bus.Value = Registers[Bus.Address & 0x001f];

				Bus.Active = false;
			}
		}
	}
}
