using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using TradeOffSlider.Models;

namespace TradeOffSlider.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		/* コマンド、プロパティの定義にはそれぞれ 
		 * 
		 *  lvcom   : ViewModelCommand
		 *  lvcomn  : ViewModelCommand(CanExecute無)
		 *  llcom   : ListenerCommand(パラメータ有のコマンド)
		 *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
		 *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
		 *  
		 * を使用してください。
		 * 
		 * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
		 * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
		 * LivetCallMethodActionなどから直接メソッドを呼び出してください。
		 * 
		 * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
		 * 同様に直接ViewModelのメソッドを呼び出し可能です。
		 */

		/* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
		 * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
		 */

		/* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
		 * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
		 * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
		 * 
		 * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
		 * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
		 * 
		 * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
		 * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
		 * 
		 * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
		 */

		/* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
		 * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
		 * 
		 * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
		 * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
		 */

		public MainWindowViewModel()
		{
			CurrentProduct = new Product();
		}

		public void Initialize()
		{
		}

		#region CurrentProduct変更通知プロパティ
		private Product _CurrentProduct;

		public Product CurrentProduct
		{
			get
			{ return _CurrentProduct; }
			set
			{ 
				if (_CurrentProduct == value)
					return;
				_CurrentProduct = value;
				RaisePropertyChanged();
				CreateReadOnlyTradeItemCollection();
			}
		}
		#endregion

		void CreateReadOnlyTradeItemCollection()
		{
			if (TradeItems != null)
			{
				TradeItems.Dispose();
			}

			TradeItems = ViewModelHelper.CreateReadOnlyDispatcherCollection(
				CurrentProduct.TradeItems,
				x =>
				{
					var vm = new TradeItemViewModel();
					vm.Initialize(x);
					return vm;
				},
				DispatcherHelper.UIDispatcher);
		}

		#region TradeItems変更通知プロパティ
		private ReadOnlyDispatcherCollection<TradeItemViewModel> _TradeItems;

		public ReadOnlyDispatcherCollection<TradeItemViewModel> TradeItems
		{
			get
			{ return _TradeItems; }
			set
			{ 
				if (_TradeItems == value)
					return;
				_TradeItems = value;
				RaisePropertyChanged();
			}
		}
		#endregion


		#region AddCommand
		private ViewModelCommand _AddCommand;

		public ViewModelCommand AddCommand
		{
			get
			{
				if (_AddCommand == null)
				{
					_AddCommand = new ViewModelCommand(Add);
				}
				return _AddCommand;
			}
		}

		public void Add()
		{
			CurrentProduct.TradeItems.Add(new TradeItem() { Name = "Item" + CurrentProduct.TradeItems.Count });
		}
		#endregion



		#region RemoveItemCommand
		private ListenerCommand<TradeItemViewModel> _RemoveItemCommand;

		public ListenerCommand<TradeItemViewModel> RemoveItemCommand
		{
			get
			{
				if (_RemoveItemCommand == null)
				{
					_RemoveItemCommand = new ListenerCommand<TradeItemViewModel>(RemoveItem);
				}
				return _RemoveItemCommand;
			}
		}

		public void RemoveItem(TradeItemViewModel parameter)
		{
			Debug.Print("RemoveItem");

			CurrentProduct.TradeItems.Remove(parameter.Model);
		}
		#endregion


	}
}
