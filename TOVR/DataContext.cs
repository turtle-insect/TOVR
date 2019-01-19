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
		public Info Info { get; set; } = Info.Instance();

		public ObservableCollection<Charactor> Charactors { get; set; } = new ObservableCollection<Charactor>();
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<SaveDataValue> Party { get; set; } = new ObservableCollection<SaveDataValue>();

		public SaveDataValue GALD { get; set; } = new SaveDataValue(0xA3D60, 4, 0, 99999999);
		public SaveDataValue GRADE { get; set; } = new SaveDataValue(0xA7718, 4, 0, 99999999);

		public void Init()
		{
			Charactors.Clear();
			Items.Clear();
			Party.Clear();

			foreach (var chara in Info.Instance().Member)
			{
				if (chara.Value == 0) continue;
				Charactors.Add(new Charactor((chara.Value - 1) * 0x4010 + 0xA8750) { Name = chara.Name });
			}

			foreach (var info in Info.Instance().Items)
			{
				Items.Add(new Item(info));
			}

			for(uint i = 0; i < 9; i++)
			{
				Party.Add(new SaveDataValue(0xA3D38 + 4 * i, 4, 0, 9));
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GALD)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GRADE)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
