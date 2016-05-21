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
        public ViewModel ()
        {
            Search = ReactiveCommand.CreateAsyncTask(parameter => GetGitHubUsers(this.UserName));

            // Executing
            _LoadingVisibility = Search.IsExecuting
                .ToProperty(this, s => s.LoadingVisibility, true);

            // Erros
            Search.ThrownExceptions.Subscribe(ex => 
            {
                ShowError = true;
                Debug.WriteLine("Erro buscando por: " + this.UserName);
            });

            Search.OnExecuteCompleted(result => {
                Debug.WriteLine("Encontrado " + result.Count + " usuários buscando por " + this.UserName);
            });

            this.Search.Subscribe(
                results =>
                {
                    SearchResults.Clear();
                    if (results != null)
                        SearchResults.AddRange(results);
                });

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

        private readonly GitHubService GitHubService = new GitHubService();

        private ReactiveList<GitHubUserInfo> _searchResults = new ReactiveList<GitHubUserInfo>();

        public ReactiveList<GitHubUserInfo> SearchResults
        {
            get { return _searchResults; }
            set { this.RaiseAndSetIfChanged(ref _searchResults, value); }
        }

        private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
        {
            ShowError = false;

            //await Task.Yield();

            return await GitHubService.GetUserByName(username).ConfigureAwait(false);
        }
    }
}