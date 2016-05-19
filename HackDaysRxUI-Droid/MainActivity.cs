using Android.App;
using Android.Widget;
using Android.OS;
using HackDaysRxUICore;
using ReactiveUI;

namespace HackDaysRxUIDroid
{
	[Activity (Label = "HackDaysRxUI-Droid", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : ReactiveActivity<ViewModel>
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);

			this.WireUpControls();

			ViewModel = new ViewModel();

			this.Bind(ViewModel, vm => vm.UserName, v => v.UserNameEditText.Text);
			this.Bind(ViewModel, vm => vm.LoadingVisibility, v => v.LoadingView.Visibility, null, new BooleanToVisibilityTypeConverter() ,new BooleanToVisibilityTypeConverter());
			this.Bind(ViewModel, vm => vm.ShowError, v => v.ErrorView.Visibility, null, new BooleanToVisibilityTypeConverter() ,new BooleanToVisibilityTypeConverter());
			//this.Bind(ViewModel, vm => vm.SearchResult.SearchUserName, v => v.SearchInfo.Text);

			var adapter = new ReactiveListAdapter<GitHubUserInfo>(
				ViewModel.SearchResult,
				(viewModel, parent) => new GitHubUserInfoView(viewModel, this, parent));

			UsersList.Adapter = adapter;
		}

		public EditText UserNameEditText { get; private set; }

		public TextView UserName { get; private set; }

		public LinearLayout LoadingView { get; private set; }

		public LinearLayout ErrorView { get; private set; }

		public ListView UsersList { get; private set; }

		public TextView SearchInfo { get; private set; }
	}
}


