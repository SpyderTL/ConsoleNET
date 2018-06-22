using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console.Hardware.Nes;

namespace Console.Hardware.Nes.Files
{
	public class NesFile
	{
		public string Path;

		public static NesCartridge Load(string path)
		{
			var cartridge = new NesCartridge();

			using (var stream = File.OpenRead(path))
			using (var reader = new BinaryReader(stream))
			{
				var signature = reader.ReadChars(4);

				var programSize = reader.ReadByte();
				var characterSize = reader.ReadByte();
				var flags = reader.ReadByte();
				var flags2 = reader.ReadByte();
				var programRamSize = reader.ReadByte();
				var flags3 = reader.ReadByte();
				var flags4 = reader.ReadByte();
				var reserved = reader.ReadBytes(5);

				var mapper = (flags >> 4) | (flags2 & 0xf0);

				var programData = reader.ReadBytes(programSize * 16 * 1024);
				var characterData = reader.ReadBytes(characterSize * 8 * 1024);

				cartridge.Rom = programData;
			}

			return cartridge;
		}
	}
}
