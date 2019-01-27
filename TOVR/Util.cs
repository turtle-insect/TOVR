using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class Util
	{
		public static void WriteNumber(uint address, uint size, long value, uint min, uint max)
		{
			if (value < min) value = min;
			if (value > max) value = max;
			SaveData.Instance().WriteNumber(address, size, (uint)value);
		}

		public static List<NameValueInfo> GetItemList(String type)
		{
			List<NameValueInfo> list = null;
			switch (type)
			{
				case "Main":
					list = Info.Instance().Mains;
					break;

				case "Sub":
					list = Info.Instance().Subs;
					break;

				case "Head":
					list = Info.Instance().Heads;
					break;

				case "Body":
					list = Info.Instance().Bodys;
					break;

				case "Accessory":
					list = Info.Instance().Accessorys;
					break;
			}

			return list;
		}
	}
}
