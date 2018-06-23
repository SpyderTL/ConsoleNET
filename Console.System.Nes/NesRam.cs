using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware.Nes
{
	public class NesRam : Component
	{
		public Bus Bus;
		public byte[] Data;

		private Thread thread;

		public NesRam(Bus bus)
		{
			Bus = bus;

			Data = new byte[0x800];

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
					Bus.Address > 0x1fff)
					Thread.Sleep(10);

				if (Bus.Write)
					Data[Bus.Address & 0x7ff] = (byte)Bus.Value;
				else
					Bus.Value = Data[Bus.Address & 0x7ff];

				Bus.Active = false;
			}
		}
	}
}
