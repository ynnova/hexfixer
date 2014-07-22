using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexFixer
{
	class Program
	{
		static void Main(string[] args)
		{
			var hexRows = File.ReadAllLines("C:\\ez_326_201.hex");
			var bytes = new List<byte>();

			var rowIndex = 1;

			foreach (var row in hexRows)
			{
				var code = row.Substring(7, 2);

				if (code == "00")
				{
					var r = row.Substring(1);

					var singleBytesStrings = Enumerable
						.Range(0, r.Length / 2)
						.Select(i => r.Substring(i * 2, 2));

					var singleBytes = singleBytesStrings
						.Select(b => Convert.ToByte(b, 16))
						.ToArray();

					byte rowChecksum = 0;
					for (int i = 0; i < 20; i++)
					{
						if (i != 3)
							rowChecksum += singleBytes[i];
					}

					for (int i = 4; i < 20; i++)
					{
						bytes.Add(singleBytes[i]);
					}

					if ((byte)(~rowChecksum + 1) != singleBytes[20])
						Console.WriteLine("Checksum di riga errato alla riga {0}", rowIndex);
				}

				rowIndex++;
			}

			var image = bytes.ToArray();

			ushort totalChecksum = 0;
			for (int i = 0; i < image.Length - 2; i++)
				totalChecksum += image[i];

			Console.WriteLine("Nuovo checksum: {0}", totalChecksum);
		}
	}
}
