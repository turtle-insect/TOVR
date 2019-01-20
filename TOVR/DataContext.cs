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
		public ObservableCollection<SaveDataValue<uint>> Party { get; set; } = new ObservableCollection<SaveDataValue<uint>>();

		public SaveDataValue<long> GALD { get; set; } = new SaveDataValue<long>(0xA3D60, 4, 0, 999999999);
		public SaveDataValue<long> MaxGALD { get; set; } = new SaveDataValue<long>(0xA7784, 4, 0, 999999999);
		public SaveDataValue<long> GRADE { get; set; } = new SaveDataValue<long>(0xA7718, 4, 0, 999999999);
		public SaveDataValue<long> SaveCount { get; set; } = new SaveDataValue<long>(0xA778C, 4, 0, 999999999);
		public SaveDataValue<long> EncountCount { get; set; } = new SaveDataValue<long>(0xA7780, 4, 0, 999999999);
		public SaveDataValue<long> KillCount { get; set; } = new SaveDataValue<long>(0xA779C, 4, 0, 999999999);

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
				Items.Add(new Item(info, 0xA3D68));
			}

			for(uint i = 0; i < 9; i++)
			{
				Party.Add(new SaveDataValue<uint>(0xA3D38 + 4 * i, 4, 0, 9));
			}

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GALD)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxGALD)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GRADE)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SaveCount)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EncountCount)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KillCount)));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
