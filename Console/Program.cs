using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console.Hardware.Nes;

namespace Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var system = new NesConsole();

			system.Cartridge = NesCartridge.Load("Test.nes");

			system.Power = true;

			while (system.Power)
			{
				System.Threading.Thread.Sleep(100);
				System.Console.WriteLine(system.Clock.Value);
			}
		}
	}
}
