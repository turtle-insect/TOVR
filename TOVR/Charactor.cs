using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TOVR
{
	class Charactor
	{
		public Charactor(uint address)
		{
			mAddress = address;
		}

		public String Name { get; set; }

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 8, 4); }
			set { Util.WriteNumber(mAddress + 8, 4, value, 1, 200); }
		}

		public uint HP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 12, 4); }
			set { Util.WriteNumber(mAddress + 12, 4, value, 0, 9999); }
		}

		public uint TP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 16, 4); }
			set { Util.WriteNumber(mAddress + 16, 4, value, 0, 999); }
		}

		public uint MaxHP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 20, 4); }
			set { Util.WriteNumber(mAddress + 20, 4, value, 0, 9999); }
		}

		public uint MaxTP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 24, 4); }
			set { Util.WriteNumber(mAddress + 24, 4, value, 0, 999); }
		}

		public uint SP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 9400, 4); }
			set { Util.WriteNumber(mAddress + 9400, 4, value, 0, 999); }
		}

		public uint MaxSP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 9404, 4); }
			set { Util.WriteNumber(mAddress + 9404, 4, value, 0, 999); }
		}

		private readonly uint mAddress;
	}
}
