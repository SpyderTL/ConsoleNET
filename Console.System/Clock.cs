using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Console.Hardware
{
	public class Clock
	{
		public bool Active;
		public bool Value;
		public int Frequency;

		private Thread thread;

		public Clock(int frequency)
		{
			Frequency = frequency;

			thread = new Thread(Start);

			thread.Start();
		}

		private void Start()
		{
			while (true)
			{
				Thread.Sleep(1000 / (Frequency * 2));

				if (Active)
					Value = !Value;
			}
		}
	}
}
