using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class FileFormatPS4 : IFileFormat
	{
		private Byte[] mHeader;

		public Byte[] Load(String filename)
		{
			if (System.IO.File.Exists(filename) == false) return null;

			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			int size = 0x308;
			mHeader = new Byte[size];
			Array.Copy(buffer, 0, mHeader, 0, size);
			Byte[] raw = new Byte[buffer.Length - size];
			Array.Copy(buffer, size, raw, 0, raw.Length);
			return raw;
		}

		public void Save(String filename, Byte[] buffer)
		{
			Byte[] total = new Byte[buffer.Length + mHeader.Length];
			mHeader.CopyTo(total, 0);
			buffer.CopyTo(total, mHeader.Length);
			System.IO.File.WriteAllBytes(filename, total);
		}
	}
}
