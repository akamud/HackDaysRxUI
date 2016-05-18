using System;
using ReactiveUI;

namespace HackDaysRxUICore
{
	public class ViewModel : ReactiveObject
	{
		private string _userName;

		public string UserName {
			get { return _userName; }
			set { this.RaiseAndSetIfChanged(ref _userName, value); }
		}
	}
}

