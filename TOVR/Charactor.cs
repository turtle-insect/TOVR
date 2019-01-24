using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace TOVR
{
	class Charactor
	{
		public Charactor(uint address)
		{
			mAddress = address;

			foreach (var info in Info.Instance().Skill)
			{
				Skills.Add(new Skill(info, address));
			}
		}

		public ObservableCollection<Skill> Skills { get; set; } = new ObservableCollection<Skill>();

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

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 32, 4); }
			set { Util.WriteNumber(mAddress + 32, 4, value, 0, 99999999); }
		}

		public uint PhysicsAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 248, 4); }
			set { Util.WriteNumber(mAddress + 248, 4, value, 0, 9999); }
		}

		public uint MagicAttack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 252, 4); }
			set { Util.WriteNumber(mAddress + 252, 4, value, 0, 9999); }
		}

		public uint PhysicsDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 256, 4); }
			set { Util.WriteNumber(mAddress + 256, 4, value, 0, 9999); }
		}

		public uint MagicDefense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 260, 4); }
			set { Util.WriteNumber(mAddress + 260, 4, value, 0, 9999); }
		}

		public uint Speed
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 268, 4); }
			set { Util.WriteNumber(mAddress + 268, 4, value, 0, 9999); }
		}

		public uint Lucky
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 272, 4); }
			set { Util.WriteNumber(mAddress + 272, 4, value, 0, 9999); }
		}

		public uint SP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 9400, 4); }
			set { Util.WriteNumber(mAddress + 9400, 4, value, 0, 500); }
		}

		public uint MaxSP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 9404, 4); }
			set { Util.WriteNumber(mAddress + 9404, 4, value, 0, 500); }
		}

		public uint EquipmentMain
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 9368, 4); }
			set { SaveData.Instance().WriteNumber(mAddress + 9368, 4, value); }
		}

		private readonly uint mAddress;
	}
}
