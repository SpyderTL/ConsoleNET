using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware.Nes
{
	public class NesCartridge : Component
	{
		public Bus Bus;
		public byte[] Rom;

		private Thread thread;

		public NesCartridge()
		{
			thread = new Thread(Start);
			thread.Start();
		}

		private void Start()
		{
			while (Bus == null)
				Thread.Sleep(100);

			while (true)
			{
				while (!Bus.Active)
					Thread.Sleep(10);

				Bus.Value = Rom[Bus.Address];

				Bus.Active = false;
			}
		}
	}
}
