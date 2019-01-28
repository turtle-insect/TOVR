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
		public ObservableCollection<Item> Tools { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Mains { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Subs { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Heads { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Bodys { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Accessorys { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Materials { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<Item> Synthesises { get; set; } = new ObservableCollection<Item>();
		public ObservableCollection<SaveDataValue<uint>> Party { get; set; } = new ObservableCollection<SaveDataValue<uint>>();

		public SaveDataValue<long> GALD { get; set; } = new SaveDataValue<long>(0xA3D60, 4, 0, 999999999);
		public SaveDataValue<long> MaxGALD { get; set; } = new SaveDataValue<long>(0xA7784, 4, 0, 999999999);
		public SaveDataValue<long> SaveCount { get; set; } = new SaveDataValue<long>(0xA778C, 4, 0, 999999999);
		public SaveDataValue<long> EncountCount { get; set; } = new SaveDataValue<long>(0xA7780, 4, 0, 999999999);
		public SaveDataValue<long> KillCount { get; set; } = new SaveDataValue<long>(0xA779C, 4, 0, 999999999);
		public SaveDataValue<long> MaxHitCount { get; set; } = new SaveDataValue<long>(0xA77B0, 4, 0, 999999999);
		public SaveDataValue<long> MaxDamage { get; set; } = new SaveDataValue<long>(0xA77A8, 4, 0, 999999999);
		public SaveDataValue<long> TotalDamage { get; set; } = new SaveDataValue<long>(0xA77B8, 4, 0, 999999999);
		public SaveDataValue<long> ReceivedDamage { get; set; } = new SaveDataValue<long>(0xA77B4, 4, 0, 999999999);

		public DataContext()
		{
			foreach (var chara in Info.Instance().Member)
			{
				if (chara.Value == 0) continue;
				Charactors.Add(new Charactor((chara.Value - 1) * 0x4010 + 0xA8750) { Name = chara.Name });
			}

			foreach (var info in Info.Instance().Tools)
			{
				Tools.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Mains)
			{
				Mains.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Subs)
			{
				Subs.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Heads)
			{
				Heads.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Bodys)
			{
				Bodys.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Accessorys)
			{
				Accessorys.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Materials)
			{
				Materials.Add(new Item(info, 0xA3D68));
			}
			foreach (var info in Info.Instance().Synthesises)
			{
				Synthesises.Add(new Item(info, 0xA3D68));
			}

			for (uint i = 0; i < 9; i++)
			{
				Party.Add(new SaveDataValue<uint>(0xA3D38 + 4 * i, 4, 0, 9));
			}
		}

		public float GRADE
		{
			get
			{
				Byte[] mem = SaveData.Instance().ReadValue(0xA7708, 4);
				return BitConverter.ToSingle(mem, 0);
			}

			set
			{
				Byte[] mem = BitConverter.GetBytes(value);
				SaveData.Instance().WriteValue(0xA7708, mem);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
