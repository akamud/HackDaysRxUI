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

		private bool _showError;

		public bool ShowError {
			get { return _showError; }
			set { this.RaiseAndSetIfChanged(ref _showError, value); }
		}

		public Random random { get; set; }

		public ViewModel ()
		{
			Search = ReactiveCommand.CreateAsyncTask(parameter => GetGitHubUsers(this.UserName));
			random = new Random ();

			_LoadingVisibility = Search.IsExecuting
				.ToProperty(this, s => s.LoadingVisibility, false);

			this.WhenAnyValue(u => u.UserName)
				.InvokeCommand(Search);

			Search.ThrownExceptions.Subscribe(ex => { ShowError = true; });
		}

		private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
		{
			ShowError = false;
			await Task.Delay (1000);
			int num = random.Next (5);

			if (num == 3)
				throw new Exception ("Deu ruim");

			return new List<GitHubUserInfo>()	;
		}
	}
}