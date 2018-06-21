using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console.Hardware.Processors.Mos;

namespace Console.Hardware.Nes
{
	public class NesConsole : Console
	{
		public NesCartridge Cartridge;
		public Clock Clock;
		public Mos6502 Cpu;
		public Bus Bus;

		private bool power;

		public bool Power
		{
			get
			{
				return power;
			}
			set
			{
				power = value;

				if (power)
					TurnOn();
				else
					TurnOff();
			}
		}

		private void TurnOn()
		{
			Bus = new Bus();
			Clock = new Clock(179000);
			Cpu = new Mos6502(Bus, Clock);

			if (Cartridge != null)
				Cartridge.Bus = Bus;

			Clock.Active = true;
		}

		private void TurnOff()
		{
			Clock.Active = false;
		}
	}
}
