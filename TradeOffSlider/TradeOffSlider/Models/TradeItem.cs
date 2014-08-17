using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace TradeOffSlider.Models
{
	public class TradeItem : NotificationObject
	{
		public TradeItem()
		{
			_Name = "項目";
			_Value = 0;
		}

		#region Name変更通知プロパティ
		private string _Name;

		public string Name
		{
			get
			{ return _Name; }
			set
			{ 
				if (_Name == value || string.IsNullOrWhiteSpace(value))
					return;
				_Name = value;
				RaisePropertyChanged();
			}
		}
		#endregion


		#region Value変更通知プロパティ
		private int _Value;

		public int Value
		{
			get
			{ return _Value; }
			set
			{ 
				if (_Value == value)
					return;
				_Value = value;
				RaisePropertyChanged();
			}
		}
		#endregion

	}
}
