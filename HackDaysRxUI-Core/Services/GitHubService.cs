using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using ReactiveUI;

namespace HackDaysRxUICore
{
	public class GitHubService
	{
		readonly Random _random = new Random();
		private ReactiveList<GitHubUserInfo> list;

		public GitHubService()
		{
			list = new ReactiveList<GitHubUserInfo>();
		}

		static List<GitHubUserInfo> GetAll()
		{
			var root = JsonConvert.DeserializeObject<RootObject>(UserFakeData.UserInfo);

			return root.Items;
		}

		public async Task<ReactiveList<GitHubUserInfo>> GetUserByName(string name)
		{
			Debug.WriteLine("Buscando por: " + name);

			return await FakeData(name);

			//https://api.github.com/search/users?q=tom
		}

		private async Task<ReactiveList<GitHubUserInfo>> FakeData(string name)
		{
			// delay na rede
			await Task.Delay(_random.Next(1000, 3000));
			// erros
//			if(_random.Next(100) > 80) {
//				throw new InvalidOperationException("deu ruim");
//			}

			var result = new Result(GetAll().Where(c => c.Login.ToLower().Contains(name.ToLower())).ToList()) {
				SearchUserName = name
			};

			return new ReactiveList<GitHubUserInfo>(GetAll().Where(c => c.Login.ToLower().Contains(name.ToLower())));
		}
	}
}

