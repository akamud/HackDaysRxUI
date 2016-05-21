﻿using Android.App;
using Android.Widget;
using Android.OS;
using HackDaysRxUICore;
using ReactiveUI;
using System;
using Android.Text;
using System.Reactive.Linq;
using Android.Text.Method;

namespace HackDaysRxUIDroid
{
	[Activity (Label = "HackDaysRxUI-Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : ReactiveActivity<ViewModel>
	{
		protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            this.WireUpControls();

            ViewModel = new ViewModel();

            this.Bind(ViewModel, vm => vm.UserName, v => v.UserNameEditText.Text);
            this.Bind(ViewModel, vm => vm.LoadingVisibility, v => v.LoadingView.Visibility, null, new BooleanToVisibilityTypeConverter(), new BooleanToVisibilityTypeConverter());
            this.Bind(ViewModel, vm => vm.ShowError, v => v.ErrorView.Visibility, null, new BooleanToVisibilityTypeConverter(), new BooleanToVisibilityTypeConverter());

            var adapter = new ReactiveListAdapter<GitHubUserInfo>(
                ViewModel.SearchResults,
                (viewModel, parent) => new GitHubUserInfoView(viewModel, this, parent));

            UsersList.Adapter = adapter;

            EnableConsoleLog();
        }

        private void EnableConsoleLog()
        {
            Log.MovementMethod = new ScrollingMovementMethod();

            ViewModel.WhenAnyValue(x => x.Log)
                            .Where(log => !string.IsNullOrWhiteSpace(log))
                            .Subscribe(log =>
                            {
                                Log.TextFormatted = Html.FromHtml("<b>" + log + "</b>");
                            });
        }

        public EditText UserNameEditText { get; private set; }

		public TextView UserName { get; private set; }

		public TextView Log { get; private set; }

        public LinearLayout LoadingView { get; private set; }

		public LinearLayout ErrorView { get; private set; }

		public ListView UsersList { get; private set; }

		public TextView SearchInfo { get; private set; }
	}
}


