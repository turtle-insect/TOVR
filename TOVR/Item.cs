using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class Item
	{
		public Item(NameValueInfo info, uint address)
		{
			mInfo = info;
			mAddress = address;
		}

		public String Name
		{
			get { return mInfo.Name; }
		}

		public uint Count
		{
			get { return SaveData.Instance().ReadNumber(mAddress + mInfo.Value * 4, 4); }
			set { Util.WriteNumber(mAddress + mInfo.Value * 4, 4, value, 0, 99); }
		}

		private readonly NameValueInfo mInfo;
		private readonly uint mAddress;
	}
}
