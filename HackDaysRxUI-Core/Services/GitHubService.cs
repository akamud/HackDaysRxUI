using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace HackDaysRxUICore
{
	public class GitHubService
	{
		readonly Random _random = new Random();

		static List<GitHubUserInfo> GetAll()
		{
			return JsonConvert.DeserializeObject<RootObject>(UserFakeData.UserInfo).Items;
		}

		public async Task<Result> GetUserByName(string name)
		{
			Debug.WriteLine("Buscando por: " + name);

			return await FakeData(name);

			//https://api.github.com/search/users?q=tom
		}

		private async Task<Result> FakeData(string name)
		{
			// delay na rede
			await Task.Delay(_random.Next(1000, 3000));
			// erros
			if(_random.Next(100) > 80) {
				throw new InvalidOperationException("deu ruim");
			}
			return new Result {
				SearchUserName = name,
				Users = GetAll().Where(c => c.Login.ToLower().Contains(name.ToLower())).ToList()
			};
		}
	}
}

