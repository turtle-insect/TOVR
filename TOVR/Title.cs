using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
    class Title
    {
		public Title(NameValueInfo Info, uint address)
		{
			mInfo = Info;
			mAddress = address;
		}

		public String Name
		{
			get { return mInfo.Name; }
		}

		public bool Enable
		{
			get { return SaveData.Instance().ReadBit(mAddress + 15437 + mInfo.Value / 8, mInfo.Value % 8); }
			set { SaveData.Instance().WriteBit(mAddress + 15437 + mInfo.Value / 8, mInfo.Value % 8, value); }
		}

		private readonly NameValueInfo mInfo;
		private readonly uint mAddress;
	}
}
