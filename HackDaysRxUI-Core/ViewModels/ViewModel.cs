using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace HackDaysRxUICore
{
	public class ViewModel : ReactiveObject
	{
		private string _userName;

		public string UserName {
			get { return _userName; }
			set { this.RaiseAndSetIfChanged(ref _userName, value); }
		}

		public ReactiveCommand<List<GitHubUserInfo>> Search { get; protected set; }

		ObservableAsPropertyHelper<bool> _LoadingVisibility;
		public bool LoadingVisibility  {
			get { return _LoadingVisibility.Value; }
		}


		public ViewModel ()
		{
			Search = ReactiveCommand.CreateAsyncTask(parameter => GetGitHubUsers(this.UserName));

			_LoadingVisibility = Search.IsExecuting
				.ToProperty(this, s => s.LoadingVisibility, false);

			this.WhenAnyValue(u => u.UserName)
				.InvokeCommand(Search);
		}

		private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
		{
			await Task.Delay (1000);
			return new List<GitHubUserInfo>()	;
		}
	}
}