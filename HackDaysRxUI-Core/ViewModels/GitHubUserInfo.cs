using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HackDaysRxUICore
{
	public class GitHubUserInfo : ReactiveObject
	{
		[DataMember]
		public string Login { get; set; }
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Avatar_url { get; set; }
		[DataMember]
		public string Gravatar_id { get; set; }
		[DataMember]
		public string Url { get; set; }
		[DataMember]
		public string Html_url { get; set; }
		[DataMember]
		public string Followers_url { get; set; }
		[DataMember]
		public string Following_url { get; set; }
		[DataMember]
		public string Gists_url { get; set; }
		[DataMember]
		public string Starred_url { get; set; }
		[DataMember]
		public string Subscriptions_url { get; set; }
		[DataMember]
		public string Organizations_url { get; set; }
		[DataMember]
		public string Repos_url { get; set; }
		[DataMember]
		public string Events_url { get; set; }
		[DataMember]
		public string Received_events_url { get; set; }
		[DataMember]
		public string Type { get; set; }
		[DataMember]
		public bool Site_admin { get; set; }
		[DataMember]
		public double Score { get; set; }
	}

	public class RootObject
	{
		public int Total_count { get; set; }
		public bool Incomplete_results { get; set; }
		public List<GitHubUserInfo> Items { get; set; }
	}
}

