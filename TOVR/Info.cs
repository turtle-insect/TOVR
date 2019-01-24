using System;
using System.Collections.Generic;

namespace TOVR
{
	class Info
	{
		private static Info mThis;
		public List<NameValueInfo> Tools { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Mains { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Subs { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Heads { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Bodys { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Materials { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Synthesises { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Accessorys { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Member { get; private set; } = new List<NameValueInfo>();
		public List<NameValueInfo> Skill { get; private set; } = new List<NameValueInfo>();

		private Info() { }

		public static Info Instance()
		{
			if (mThis == null)
			{
				mThis = new Info();
				mThis.Init();
			}
			return mThis;
		}

		public NameValueInfo Search<Type>(List<Type> list, uint id)
			where Type : NameValueInfo, new()
		{
			int min = 0;
			int max = list.Count;
			for (; min < max;)
			{
				int mid = (min + max) / 2;
				if (list[mid].Value == id) return list[mid];
				else if (list[mid].Value > id) max = mid;
				else min = mid + 1;
			}
			return null;
		}

		private void Init()
		{
			AppendList("info\\tool.txt", Tools);
			AppendList("info\\main.txt", Mains);
			AppendList("info\\sub.txt", Subs);
			AppendList("info\\head.txt", Heads);
			AppendList("info\\body.txt", Bodys);
			AppendList("info\\accessory.txt", Accessorys);
			AppendList("info\\material.txt", Materials);
			AppendList("info\\synthesis.txt", Synthesises);
			AppendList("info\\member.txt", Member);
			AppendList("info\\skill.txt", Skill);

			Mains.Sort();
			Subs.Sort();
			Heads.Sort();
			Bodys.Sort();
			Accessorys.Sort();
			Materials.Sort();
			Synthesises.Sort();
		}

		private void AppendList<Type>(String filename, List<Type> items)
			where Type : ILineAnalysis, new()
		{
			if (!System.IO.File.Exists(filename)) return;
			String[] lines = System.IO.File.ReadAllLines(filename);
			foreach (String line in lines)
			{
				if (line.Length < 3) continue;
				if (line[0] == '#') continue;
				String[] values = line.Split('\t');
				if (values.Length < 2) continue;
				if (String.IsNullOrEmpty(values[0])) continue;
				if (String.IsNullOrEmpty(values[1])) continue;

				Type type = new Type();
				if(type.Line(values))
				{
					items.Add(type);
				}
			}
		}
	}
}
