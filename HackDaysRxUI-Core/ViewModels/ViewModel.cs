using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace HackDaysRxUICore
{
	public class ViewModel : ReactiveObject
	{
		public ViewModel ()
		{
			GitHubService = new GitHubService();
			random = new Random ();

			var canExecute = this.WhenAnyValue(v => v.UserName)
				.Select(s => !string.IsNullOrWhiteSpace(s));
			Search = ReactiveCommand.CreateAsyncTask(canExecute, parameter => GetGitHubUsers(this.UserName));
			_LoadingVisibility = Search.IsExecuting
				.ToProperty(this, s => s.LoadingVisibility, false);
			
			Search.ThrownExceptions.Subscribe(ex => { ShowError = true; });

			this.WhenAnyValue(u => u.UserName)
				.InvokeCommand(Search);
		}

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

		public GitHubService GitHubService { get; set; }

		ObservableAsPropertyHelper<List<GitHubUserInfo>> _SearchResults;
		public List<GitHubUserInfo> SearchResults {
			get { return _SearchResults.Value; }
		}

		private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
		{
			ShowError = false;

			return await GitHubService.GetUserByName(UserName);
		}
	}
}