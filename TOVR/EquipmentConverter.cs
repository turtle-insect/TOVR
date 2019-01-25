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
			return Info.Instance().Search(list, (uint)value)?.Name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		private List<NameValueInfo> GetList(object obj)
		{
			List<NameValueInfo> list = null;
			switch (obj.ToString())
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
