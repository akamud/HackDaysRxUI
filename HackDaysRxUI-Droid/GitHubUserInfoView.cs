﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;
using HackDaysRxUICore;

namespace HackDaysRxUIDroid
{
	public class GitHubUserInfoView : ReactiveViewHost<GitHubUserInfo>
	{
		public GitHubUserInfoView(GitHubUserInfo viewModel, Context ctx, ViewGroup parent) : base(ctx, Resource.Layout.GitHubUserInfo, parent)
		{
			ViewModel = viewModel;
			this.OneWayBind(ViewModel, vm => vm.Login, v => v.Login.Text);
		}

		public TextView Login { get; private set; }
	}
}
