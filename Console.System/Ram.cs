using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware
{
	public class Ram : Component
	{
		public Bus Bus;
		public byte[] Data;
		public ulong Address;

		private Thread thread;

		public Ram(ulong size, ulong address, Bus bus)
		{
			Bus = bus;
			Data = new byte[size];
			Address = address;

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
					Bus.Address < Address ||
					Bus.Address >= (Address + (ulong)Data.LongLength))
					Thread.Sleep(10);

				if (Bus.Write)
					Data[Bus.Address - Address] = (byte)Bus.Value;
				else
					Bus.Value = Data[Bus.Address - Address];

				Bus.Active = false;
			}
		}
	}
}
