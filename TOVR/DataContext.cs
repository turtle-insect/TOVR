using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TOVR
{
	class DataContext : INotifyPropertyChanged
	{
		public ObservableCollection<Charactor> Party { get; set; } = new ObservableCollection<Charactor>();
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

		public void Init()
		{
			Party.Clear();
			Items.Clear();

			Party.Add(new Charactor(0xA8750) { Name = "ユーリ" });
			Party.Add(new Charactor(0xAC760) { Name = "エステル" });
			Party.Add(new Charactor(0xC07B0) { Name = "ラピッド" });

			foreach (var info in Info.Instance().Items)
			{
				Items.Add(new Item(info));
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GALD)));
		}
		public long GALD
		{
			get { return SaveData.Instance().ReadNumber(0xA3D60, 4); }
			set { Util.WriteNumber(0xA3D60, 4, value, 0, 999999999); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
