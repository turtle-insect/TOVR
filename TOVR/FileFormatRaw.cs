using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class FileFormatRaw : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			if (System.IO.File.Exists(filename) == false) return null;

			return System.IO.File.ReadAllBytes(filename);
		}

		public void Save(String filename, Byte[] buffer)
		{
			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
