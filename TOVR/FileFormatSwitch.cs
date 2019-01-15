using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class FileFormatSwitch : IFileFormat
	{
		private Byte[] mHeader = null;
		private Byte[] mLZMAProp = null;

		public Byte[] Load(String filename)
		{
			if (System.IO.File.Exists(filename) == false) return null;

			Byte[] buffer = System.IO.File.ReadAllBytes(filename);

			mHeader = new byte[0x230];
			Array.Copy(buffer, mHeader, mHeader.Length);

			int inSize = BitConverter.ToInt32(buffer, 0x230);
			int outSize = BitConverter.ToInt32(buffer, 0x234);
			int outIndex = 0;
			Byte[] raw = new byte[outSize];
			int blockCount = (outSize + 0xFFFF) / 0x10000;

			int blockHeaderOffset = 0x245;
			int dataOffset = blockHeaderOffset + 2 * blockCount;
			int remaining = outSize;

			var decoder = new SevenZip.Compression.LZMA.Decoder();
			mLZMAProp = new byte[5];
			Array.Copy(buffer, 0x240, mLZMAProp, 0, 5);
			decoder.SetDecoderProperties(mLZMAProp);

			for (int count = 0; count < blockCount; count++)
			{
				int offset = blockHeaderOffset + 2 * count;
				short blockSize = BitConverter.ToInt16(buffer, offset);

				using (var inStream = new System.IO.MemoryStream(blockSize))
				{
					inStream.Write(buffer, dataOffset, blockSize);
					inStream.Position = 0;
					using (var outStream = new System.IO.MemoryStream())
					{
						int streamSize = remaining > 0x10000 ? 0x10000 : remaining;
						decoder.Code(inStream, outStream, blockSize, streamSize, null);
						outStream.Position = 0;
						outStream.Read(raw, outIndex, streamSize);
						outIndex += streamSize;
					}
				}
				dataOffset += blockSize;
				remaining -= 0x10000;
			}

			return raw;
		}

		public void Save(String filename, Byte[] buffer)
		{
			int blockCount = (buffer.Length + 0xFFFF) / 0x10000;
			short[] blockSize = new short[blockCount];
			using (var lzmaStream = new System.IO.MemoryStream())
			{
				var encoder = new SevenZip.Compression.LZMA.Encoder();

				for (int count = 0; count < blockCount; count++)
				{
					int streamSize = 0x10000;
					if (count + 1 == blockCount)
					{
						streamSize = buffer.Length % 0x10000;
					}
					using (var inStream = new System.IO.MemoryStream(streamSize))
					{
						inStream.Write(buffer, count * 0x10000, streamSize);
						inStream.Position = 0;
						using (var outStream = new System.IO.MemoryStream())
						{
							encoder.Code(inStream, outStream, -1, -1, null);
							outStream.Position = 0;
							blockSize[count] = (short)outStream.Length;
							lzmaStream.Write(outStream.GetBuffer(), 0, (int)outStream.Length);
						}
					}
				}

				using (var outputStream = new System.IO.MemoryStream())
				{
					outputStream.Write(mHeader, 0, mHeader.Length);
					outputStream.Write(BitConverter.GetBytes(lzmaStream.Length), 0, 4);
					outputStream.Write(BitConverter.GetBytes(buffer.Length), 0, 4);
					for (int i = 0; i < 8; i++) outputStream.WriteByte(0);
					outputStream.Write(mLZMAProp, 0, mLZMAProp.Length);
					foreach (var i in blockSize) outputStream.Write(BitConverter.GetBytes(i), 0, 2);
					outputStream.Write(lzmaStream.GetBuffer(), 0, (int)lzmaStream.Length);
					outputStream.Position = 0;
					System.IO.File.WriteAllBytes(filename, outputStream.GetBuffer());
				}
			}
		}
	}
}
