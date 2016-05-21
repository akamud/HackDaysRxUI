using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using ReactiveUI;
using System.Text;
using Octokit;

namespace HackDaysRxUICore
{
	public class GitHubService
	{
		private List<GitHubUserInfo> list;

		public GitHubService()
		{
			list = new List<GitHubUserInfo>();
		}

		public async Task<List<GitHubUserInfo>> GetUserByName(string name)
		{
            List<GitHubUserInfo> list = new List<GitHubUserInfo>();

            if (!string.IsNullOrWhiteSpace(name))
            {
			    Debug.WriteLine("Searching for: " + name);

                var github = new GitHubClient(new ProductHeaderValue("RxUI"));
                var users = await github.Search.SearchUsers(new SearchUsersRequest(name)).ConfigureAwait(false);
                list = users.Items.Where(c => c.Login.ToLower().Contains(name.ToLower()))
                    .Select(u => new GitHubUserInfo() { Login = u.Login })
                    .ToList();
            }

            return list;
        }
	}
}

