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
        public ReactiveCommand<List<GitHubUserInfo>> Search { get; protected set; }

        private readonly GitHubService GitHubService = new GitHubService();

        public ViewModel()
        {
            Search = ReactiveCommand.CreateAsyncTask(parameter => GetGitHubUsers((string)parameter));
            #region config
            // Executing
            //_IsLoading = Search.IsExecuting
            //    .ToProperty(this, s => s.IsLoading, true);

            //// Erros
            //Search.ThrownExceptions.Subscribe(ex =>
            //{
            //    ShowError = true;
            //    AppendLog("Erro buscando por: " + this.UserName);
            //});

            //Search.OnExecuteCompleted(result =>
            //{
            //    AppendLog("Encontrado " + result.Count + " usuários");
            //});
            #endregion

            #region command
            //this.WhenAnyValue(u => u.UserName)
            //    .SelectMany(u => this.Search.ExecuteAsync(u))
            //    .Subscribe(results =>
            //    {
            //        SearchResults.Clear();
            //        if (results != null)
            //            SearchResults.AddRange(results);
            //    });
            #endregion

            this.WhenAnyValue(u => u.UserName)
                .SelectMany(s => Search.ExecuteAsync(s))
                .Subscribe(results =>
                {
                    SearchResults.Clear();
                    if (results != null)
                        SearchResults.AddRange(results);
                });
        }


        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { this.RaiseAndSetIfChanged(ref _userName, value); }
        }

        #region prop
        // Valor depende de outra property, transformada em um observable
        ObservableAsPropertyHelper<bool> _IsLoading;
        public bool IsLoading  {
            get { return _IsLoading.Value; }
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
        #endregion

        private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
        {
            ShowError = false;

            AppendLog("Buscando por: " + username);

            return await GitHubService.GetUserByName(username);
        }

        #region log
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
        #endregion
    }
}