using System;
using System.Collections.Generic;

namespace TOVR
{
	class SaveData
	{
		private static SaveData mThis;
		private String mFileName = null;
		private Byte[] mHeader = null;
		private Byte[] mLZMAProp = null;
		private Byte[] mBuffer = null;
		private readonly System.Text.Encoding mEncode = System.Text.Encoding.UTF8;
		public uint Adventure { private get; set; } = 0;

		private SaveData()
		{}

		public static SaveData Instance()
		{
			if (mThis == null) mThis = new SaveData();
			return mThis;
		}

		public bool Open(String filename)
		{
			mFileName = filename;
			mBuffer = System.IO.File.ReadAllBytes(mFileName);

			String header = "TLZC";
			for (int i = 0; i < header.Length; i++)
			{
				if (mBuffer[i + 0x228] != header[i])
				{
					mFileName = null;
					mBuffer = null;
					mHeader = null;
					mLZMAProp = null;
					return false;
				}
			}

			mHeader = new Byte[560];
			Array.Copy(mBuffer, 0, mHeader, 0, 560);
			Decomp();

			Backup();
			return true;
		}

		public bool Save()
		{
			if (mFileName == null || mBuffer == null || mHeader==null || mLZMAProp==null) return false;

			int blockCount = (mBuffer.Length + 0xFFFF) / 0x10000;
			short[] blockSize = new short[blockCount];
			using (var lzmaStream = new System.IO.MemoryStream())
			{
				var encoder = new SevenZip.Compression.LZMA.Encoder();

				for (int count = 0; count < blockCount; count++)
				{
					int streamSize = 0x10000;
					if (count + 1 == blockCount)
					{
						streamSize = mBuffer.Length % 0x10000;
					}
					using (var inStream = new System.IO.MemoryStream(streamSize))
					{
						inStream.Write(mBuffer, count * 0x10000, streamSize);
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
					outputStream.Write(BitConverter.GetBytes(mBuffer.Length), 0, 4);
					for (int i = 0; i < 8; i++) outputStream.WriteByte(0);
					outputStream.Write(mLZMAProp, 0, mLZMAProp.Length);
					foreach (var i in blockSize) outputStream.Write(BitConverter.GetBytes(i), 0, 2);
					outputStream.Write(lzmaStream.GetBuffer(), 0, (int)lzmaStream.Length);
					outputStream.Position = 0;
					System.IO.File.WriteAllBytes(mFileName, outputStream.GetBuffer());
				}
			}
			
			return true;
		}

		public bool SaveAs(String filename)
		{
			if (mBuffer == null) return false;
			mFileName = filename;
			return Save();
		}

		public uint ReadNumber(uint address, uint size)
		{
			if (mBuffer == null) return 0;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return 0;
			uint result = 0;
			for(int i = 0; i < size; i++)
			{
				result += (uint)(mBuffer[address + i]) << (i * 8);
			}
			return result;
		}

		// 0 to 7.
		public bool ReadBit(uint address, uint bit)
		{
			if (bit < 0) return false;
			if (bit > 7) return false;
			if (mBuffer == null) return false;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return false;
			Byte mask = (Byte)(1 << (int)bit);
			Byte result = (Byte)(mBuffer[address] & mask);
			return result != 0;
		}

		public String ReadText(uint address, uint size)
		{
			if (mBuffer == null) return "";
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return "";

			Byte[] tmp = new Byte[size];
			for(uint i = 0; i < size; i++)
			{
				if (mBuffer[address + i] == 0) break;
				tmp[i] = mBuffer[address + i];
			}
			return mEncode.GetString(tmp).Trim('\0');
		}

		public void WriteNumber(uint address, uint size, uint value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = (Byte)(value & 0xFF);
				value >>= 8;
			}
		}

		// 0 to 7.
		public void WriteBit(uint address, uint bit, bool value)
		{
			if (bit < 0) return;
			if (bit > 7) return;
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return;
			Byte mask = (Byte)(1 << (int)bit);
			if (value) mBuffer[address] = (Byte)(mBuffer[address] | mask);
			else mBuffer[address] = (Byte)(mBuffer[address] & ~mask);
		}

		public void WriteText(uint address, uint size, String value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			Byte[] tmp = mEncode.GetBytes(value);
			Array.Resize(ref tmp, (int)size);
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = tmp[i];
			}
		}

		public void WriteValue(uint address, Byte[] buffer)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + buffer.Length > mBuffer.Length) return;

			for (uint i = 0; i < buffer.Length; i++)
			{
				mBuffer[address + i] = buffer[i];
			}
		}

		public void Fill(uint address, uint size, Byte number)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = number;
			}
		}

		public void Copy(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for(uint i = 0; i < size; i++)
			{
				mBuffer[to + i] = mBuffer[from + i];
			}
		}

		public void Swap(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				Byte tmp = mBuffer[to + i];
				mBuffer[to + i] = mBuffer[from + i];
				mBuffer[from + i] = tmp;
			}
		}

		public List<uint> FindAddress(String name, uint index)
		{
			List<uint> result = new List<uint>();
			for (; index < mBuffer.Length; index++)
			{
				if (mBuffer[index] != name[0]) continue;

				int len = 1;
				for (; len < name.Length; len++)
				{
					if (mBuffer[index + len] != name[len]) break;
				}
				if (len >= name.Length) result.Add(index);
				index += (uint)len;
			}
			return result;
		}

		private uint CalcAddress(uint address)
		{
			return address;
		}

		private void Decomp()
		{
			int inSize = (int)ReadNumber(0x230, 4);
			int outSize = (int)ReadNumber(0x234, 4);
			int outIndex = 0;
			Byte[] outputBuffer = new byte[outSize];
			int blockCount = (outSize + 0xFFFF) / 0x10000;

			int blockHeaderOffset = 0x245;
			int dataOffset = blockHeaderOffset + 2 * blockCount;
			int remaining = outSize;

			var decoder = new SevenZip.Compression.LZMA.Decoder();
			mLZMAProp = new byte[5];
			Array.Copy(mBuffer, 0x240, mLZMAProp, 0, 5);
			decoder.SetDecoderProperties(mLZMAProp);

			for (int count = 0; count < blockCount; count++)
			{
				int offset = blockHeaderOffset + 2 * count;
				int blockSize = (int)ReadNumber((uint)offset, 2);

				using (var inStream = new System.IO.MemoryStream(blockSize))
				{
					inStream.Write(mBuffer, dataOffset, blockSize);
					inStream.Position = 0;
					using (var outStream = new System.IO.MemoryStream())
					{
						int streamSize = remaining > 0x10000 ? 0x10000 : remaining;
						decoder.Code(inStream, outStream, blockSize, streamSize, null);
						outStream.Position = 0;
						outStream.Read(outputBuffer, outIndex, streamSize);
						outIndex += streamSize;
					}
				}
				dataOffset += blockSize;
				remaining -= 0x10000;
			}

			mBuffer = outputBuffer;
		}

		private void Backup()
		{
			DateTime now = DateTime.Now;
			String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			path = System.IO.Path.Combine(path, "backup");
			if(!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			path = System.IO.Path.Combine(path, 
				String.Format("{0:0000}-{1:00}-{2:00} {3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute));
			System.IO.File.Copy(mFileName, path, true);
		}
	}
}
