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
		readonly Random _random = new Random();
		private List<GitHubUserInfo> list;

		public GitHubService()
		{
			list = new List<GitHubUserInfo>();
		}

		static List<GitHubUserInfo> GetAll()
		{
			var root = JsonConvert.DeserializeObject<RootObject>(UserFakeData.UserInfo);

			return root.Items;
		}

		public async Task<List<GitHubUserInfo>> GetUserByName(string name)
		{
            List<GitHubUserInfo> list = new List<GitHubUserInfo>();

            if (!string.IsNullOrWhiteSpace(name))
            {
			    Debug.WriteLine("Buscando por: " + name);

                await Task.Yield();

                return await FakeData(name);

                //var github = new GitHubClient(new ProductHeaderValue("RxUI"));
                //var users = await github.Search.SearchUsers(new SearchUsersRequest(name)).ConfigureAwait(false);
                //list = users.Items.Where(c => c.Login.ToLower().Contains(name.ToLower()))
                //    .Select(u => new GitHubUserInfo() { Login = u.Login })
                //    .ToList();
            }

            return list;
        }

        private async Task<List<GitHubUserInfo>> FakeData(string name)
        {
            // delay na rede
            //await Task.Delay(_random.Next(1000, 3000));
            await Task.Delay(_random.Next(1000, 3000));

            //erros
            if (_random.Next(100) > 90)
            {
                Debug.WriteLine("Erro buscando por: " + name);
                throw new InvalidOperationException("deu ruim");
            }

            var result = new Result(GetAll().Where(c => c.Login.ToLower().Contains(name.ToLower())).ToList())
            {
                SearchUserName = name
            };

            return GetAll().Where(c => c.Login.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}

