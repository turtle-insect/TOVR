using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TOVR
{
	class CommandItemChoice : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			object[] raw = (object[])parameter;
			if (raw == null) return false;
			return raw[0] != null;
		}

		public void Execute(object parameter)
		{
			object[] raw = (object[])parameter;
			if (raw == null) return;

			Charactor ch = raw[0] as Charactor;
			if (ch == null) return;

			var window = new ItemChoiceWindow();
			window.ID = uint.Parse(raw[1].ToString());
			window.Type = raw[2].ToString();
			window.ShowDialog();

			switch (raw[2].ToString())
			{
				case "Main":
					ch.EquipmentMain = window.ID;
					break;

				case "Sub":
					ch.EquipmentSub = window.ID;
					break;

				case "Head":
					ch.EquipmentHead = window.ID;
					break;

				case "Body":
					ch.EquipmentBody = window.ID;
					break;
			}
		}
	}
}
