using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Octokit;

namespace HackDaysRxUICore
{
    public class ViewModel : ReactiveObject
    {
        public ViewModel()
        {
            Search = ReactiveCommand.CreateAsyncTask(parameter => GetGitHubUsers(this.UserName));

            // Executing
            _LoadingVisibility = Search.IsExecuting
                .ToProperty(this, s => s.LoadingVisibility, true);

            // Erros
            Search.ThrownExceptions.Subscribe(ex =>
            {
                ShowError = true;
                AppendLog("Erro buscando por: " + this.UserName);
            });

            Search.OnExecuteCompleted(result => {
                AppendLog("Encontrado " + result.Count + " usuários buscando por " + this.UserName);
            });

            this.WhenAnyValue(u => u.UserName)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.MainThreadScheduler)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(x => x.Trim())
                .DistinctUntilChanged()
                .Select(u => this.Search.ExecuteAsync())
                .Switch()
                .Retry()
                .Subscribe(results =>
                {
                    SearchResults.Clear();
                    if (results != null)
                        SearchResults.AddRange(results);
                });
        }

        public ReactiveCommand<List<GitHubUserInfo>> Search { get; protected set; }

        private readonly GitHubService GitHubService = new GitHubService();

        private string _userName;

        public string UserName {
            get { return _userName; }
            set { this.RaiseAndSetIfChanged(ref _userName, value); }
        }

        ObservableAsPropertyHelper<bool> _LoadingVisibility;
        public bool LoadingVisibility  {
            get { return _LoadingVisibility.Value; }
        }

        private bool _showError;

        public bool ShowError {
            get { return _showError; }
            set { this.RaiseAndSetIfChanged(ref _showError, value); }
        }

        private ReactiveList<GitHubUserInfo> _searchResults = new ReactiveList<GitHubUserInfo>();

        public ReactiveList<GitHubUserInfo> SearchResults
        {
            get { return _searchResults; }
            set { this.RaiseAndSetIfChanged(ref _searchResults, value); }
        }

        private string _log;

        public string Log
        {
            get { return _log; }
            set { this.RaiseAndSetIfChanged(ref _log, value); }
        }

        private void AppendLog(string log)
        {
            Log = string.IsNullOrWhiteSpace(Log) ? log : log + "<br /><br />" + Log;
        }

        private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
        {
            ShowError = false;

            AppendLog("Buscando por: " + username);

            return await GitHubService.GetUserByName(username);
        }
    }
}