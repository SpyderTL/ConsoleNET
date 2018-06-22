using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Hardware
{
	public class Bus : Component
	{
		public ulong Address;
		public ulong Value;
		public bool Write;
		public bool Active;
	}
}
