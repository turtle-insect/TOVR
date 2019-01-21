using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TOVR
{
	class EquipmentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var list = GetList(parameter);

			int index = -1;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Value == (uint)value)
					{
						index = i;
						break;
					}
				}
			}

			return index;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var list = GetList(parameter);
			return list[(int)value].Value;
		}

		private List<NameValueInfo> GetList(object obj)
		{
			List<NameValueInfo> list = null;
			switch (obj.ToString())
			{
				case "Main":
					list = Info.Instance().Mains;
					break;
			}

			return list;
		}
	}
}
