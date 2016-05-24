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
            Search = ReactiveCommand.CreateAsyncTask(parameter => GetGitHubUsers((string)parameter));

            // Executing
            _LoadingVisibility = Search.IsExecuting
                .ToProperty(this, s => s.LoadingVisibility, true);

            this.WhenAnyValue(u => u.UserName)
                .Select(u => this.Search.ExecuteAsync(u))
                .Switch()
                .Subscribe(results =>
                {
                    SearchResults.Clear();
                    if (results != null)
                        SearchResults.AddRange(results);
                }, ex => { ShowError = true; });
        }

        ObservableAsPropertyHelper<bool> _LoadingVisibility;
        public bool LoadingVisibility
        {
            get { return _LoadingVisibility.Value; }
        }

        public ReactiveCommand<List<GitHubUserInfo>> Search { get; protected set; }

        private readonly GitHubService GitHubService = new GitHubService();

        private string _userName;

        public string UserName {
            get { return _userName; }
            set { this.RaiseAndSetIfChanged(ref _userName, value); }
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

        private async Task<List<GitHubUserInfo>> GetGitHubUsers(string username)
        {
            Debug.WriteLine("Searching: " + username);
            ShowError = false;

            return await GitHubService.GetUserByName(username);
        }
    }
}