using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOVR
{
	class DataContext
	{
		public uint GALD
		{
			get { return SaveData.Instance().ReadNumber(0xA3D60, 4); }
			set { Util.WriteNumber(0xA3D60, 4, value, 0, 999999999); }
		}
	}
}
