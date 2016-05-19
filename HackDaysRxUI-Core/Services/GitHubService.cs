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

		public async Task<List<GitHubUserInfo>> GetUserByName(string name)
		{
			Debug.WriteLine("Buscando por: " + name);

			// delay na rede
			await Task.Delay(_random.Next(1000, 3000));

			// erros
			if (_random.Next(100) > 80)
			{
				throw new InvalidOperationException("deu ruim");
			}

			return GetAll()
				.Where(c => string.Compare(c.Login, name, StringComparison.CurrentCultureIgnoreCase) >= 0)
				.ToList();

			//https://api.github.com/search/users?q=tom
		}
	}
}

