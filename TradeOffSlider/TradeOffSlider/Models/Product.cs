using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Livet;

namespace TradeOffSlider.Models
{
	public class Product : NotificationObject
	{
		public Product()
		{
			_Name = "Product1";
			TradeItems = new ObservableCollection<TradeItem>();
			TradeItems.Add(new TradeItem() { Name = "予算", Value = 80 });
			TradeItems.Add(new TradeItem() { Name = "時間", Value = 60 });
			TradeItems.Add(new TradeItem() { Name = "品質", Value = 90 });
			TradeItems.Add(new TradeItem() { Name = "スコープ", Value = 10 });
		}

		#region Name変更通知プロパティ
		private string _Name;

		public string Name
		{
			get
			{ return _Name; }
			set
			{ 
				if (_Name == value)
					return;
				_Name = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		public ObservableCollection<TradeItem> TradeItems { get; private set; }
	}
}
