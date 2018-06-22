using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console.Hardware.Nes;
using Console.Hardware.Nes.Files;

namespace Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var system = new NesConsole();

			system.Cartridge = NesFile.Load("Metroid (U) [!].nes");

			system.Power = true;

			while (system.Power)
			{
				System.Threading.Thread.Sleep(100);
				System.Console.WriteLine(system.Cpu.Program.ToString("X4"));
			}
		}
	}
}
