using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

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

			// Executing
			_LoadingVisibility = Search.IsExecuting
				.ToProperty(this, s => s.LoadingVisibility, false);

			// Erros
			Search.ThrownExceptions.Subscribe(ex => { ShowError = true; Debug.WriteLine("Erro buscando por: " + this.UserName); });

			// Sucesso
			_SearchResult = Search.ToProperty(this, v => v.SearchResult, new Result());

			Search.OnExecuteCompleted(result => {
				this.ShowInfo = true;
				Debug.WriteLine("Encontrado " + result.Users.Count + " usuários buscando por " + result.SearchUserName);
			});

			// Act
			this.WhenAnyValue(u => u.UserName)
				.InvokeCommand(Search);
		}

		private string _userName;

		public string UserName {
			get { return _userName; }
			set { this.RaiseAndSetIfChanged(ref _userName, value); }
		}

		public ReactiveCommand<Result> Search { get; protected set; }

		ObservableAsPropertyHelper<bool> _LoadingVisibility;
		public bool LoadingVisibility  {
			get { return _LoadingVisibility.Value; }
		}

		private bool _showError;

		public bool ShowError {
			get { return _showError; }
			set { this.RaiseAndSetIfChanged(ref _showError, value); }
		}

		private bool _showInfo;

		public bool ShowInfo {
			get { return _showInfo; }
			set { this.RaiseAndSetIfChanged(ref _showInfo, value); }
		}

		public Random random { get; set; }

		public GitHubService GitHubService { get; set; }

		ObservableAsPropertyHelper<Result> _SearchResult;
		public Result SearchResult {
			get { return _SearchResult.Value; }
		}

		private async Task<Result> GetGitHubUsers(string username)
		{
			ShowError = false;
			ShowInfo = false;

			return await GitHubService.GetUserByName(UserName);
		}
	}
}