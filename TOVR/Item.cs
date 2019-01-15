using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class Item
	{
		public Item(NameValueInfo info)
		{
			mInfo = info;
		}

		public String Name
		{
			get { return mInfo.Name; }
		}

		public uint Count
		{
			get { return SaveData.Instance().ReadNumber(0xA3D6C + mInfo.Value * 4, 4); }
			set { Util.WriteNumber(0xA3D6C + mInfo.Value * 4, 4, value, 0, 99); }
		}

		private NameValueInfo mInfo;
	}
}
