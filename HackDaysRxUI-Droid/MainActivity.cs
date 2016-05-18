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

			//Button button = FindViewById<Button> (Resource.Id.myButton);



			ViewModel = new ViewModel();
			this.Bind (ViewModel, v => v.UserName, x => x.UserNameEditText.Text);
			this.Bind (ViewModel, v => v.UserName, x => x.UserName.Text);
		}

		public EditText UserNameEditText {
			get { return this.GetControl<EditText> (); }
		}

		public TextView UserName {
			get { return this.GetControl<TextView> (); }
		}
	}
}


