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
	}
}
