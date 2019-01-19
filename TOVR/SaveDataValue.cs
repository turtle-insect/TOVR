using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class SaveDataValue
	{
		public SaveDataValue(uint address, uint size)
		{
			mAddress = address;
			mSize = size;
		}

		public SaveDataValue(uint address, uint size, uint min, uint max)
			: this(address, size)
		{
			mMinValue = min;
			mMaxValue = max;
		}

		public uint Value
		{
			get { return SaveData.Instance().ReadNumber(mAddress, mSize); }
			set { Util.WriteNumber(mAddress, mSize, value, mMinValue, mMaxValue); }
		}

		private readonly uint mAddress;
		private readonly uint mSize;
		private readonly uint mMinValue = 0;
		private readonly uint mMaxValue = uint.MaxValue;
	}
}
