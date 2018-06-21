using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Hardware.Nes
{
	public class NesCartridge : Component
	{
		public Bus Bus;

		public static NesCartridge Load(string path)
		{
			return new NesCartridge();
		}
	}
}
