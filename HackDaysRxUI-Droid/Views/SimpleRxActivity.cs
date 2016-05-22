using Android.App;
using Android.Widget;
using Android.OS;
using HackDaysRxUICore;
using System;
using Android.Text;
using System.Reactive.Linq;
using Android.Text.Method;
using HackDaysRxUIDroid.Converters;
using System.Collections.Generic;
using System.Reactive.Disposables;
using HackDaysRxUICore.Helpers;
using System.Linq;
using System.Reactive;

namespace HackDaysRxUIDroid.Views
{
    [Activity (Label = "HackDaysRxUI-Simple-Rx", MainLauncher = true, Icon = "@mipmap/icon")]
    public class SimpleRxActivity : Activity
    {
        private CompositeDisposable compositeDisposables = new CompositeDisposable();

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            UserNameEditText = FindViewById<EditText>(Resource.Id.UserNameEditText);
            Log = FindViewById<TextView>(Resource.Id.Log);
            Log.MovementMethod = new ScrollingMovementMethod();

            Range();

            //Timer();

            //EventPattern();

            //Merge();

            //CombineLatest();

            //Zip();

            //ObservableAsync();
        }


        private void Timer()
        {
            var timerStream = Observable.Timer(TimeSpan.FromSeconds(3));

            timerStream
                .Subscribe(num =>
                    RunOnUiThread(() =>
                    {
                        Log.Text = "Timer";
                    }))
                .DisposeWith(compositeDisposables);
        }

        private void Range()
        {
            var rangeStream = Observable.Range(1, 10);

            rangeStream
                //.StartWith(0)
                .Subscribe(num =>
                    RunOnUiThread(() =>
                    {
                        Log.Text = num.ToString() + "\n" + Log.Text;
                    }), 
                    () => Log.Text = "Completed \n" + Log.Text)
                .DisposeWith(compositeDisposables);
        }

        private void EventPattern()
        {
            var textStream = Observable.FromEventPattern<EventHandler<TextChangedEventArgs>, TextChangedEventArgs>(
                                            x => UserNameEditText.TextChanged += x,
                                            x => UserNameEditText.TextChanged -= x
                                        );
                                        //.Select(args => args.EventArgs.Text);

            textStream
                //.Where(text => text.Contains('o'))
                //.Buffer(3)
                //.Throttle(TimeSpan.FromSeconds(1))
                //.Delay(TimeSpan.FromSeconds(1))
                .Subscribe(text =>
                    RunOnUiThread(() =>
                    {
                        Log.Text = text.ToString() + "\n" + Log.Text;
                    }))
                .DisposeWith(compositeDisposables);
        }

        

        private void Merge()
        {
            var textStream = CreateObservableFromEvent()
                                .Select(args => args.EventArgs.Text);

            var rangeStream = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(num => num.ToString());

            textStream
                .Merge(rangeStream)
                .Subscribe(text =>
                    RunOnUiThread(() =>
                    {
                        Log.Text = text.ToString() + "\n" + Log.Text;
                    }))
                .DisposeWith(compositeDisposables);
        }

        private void CombineLatest()
        {
            var textStream = CreateObservableFromEvent()
                                .Select(args => args.EventArgs.Text);

            var rangeStream = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(num => num.ToString());

            textStream
                .CombineLatest(rangeStream, (text, num) => text + " " + num)
                .Subscribe(text =>
                    RunOnUiThread(() =>
                    {
                        Log.Text = text.ToString() + "\n" + Log.Text;
                    }))
                .DisposeWith(compositeDisposables);
        }

        private void Zip()
        {
            var textStream = CreateObservableFromEvent()
                                .Select(args => args.EventArgs.Text);

            var rangeStream = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(num => num.ToString());

            textStream
                .Zip(rangeStream, (text, num) => text + " " + num)
                .Subscribe(text =>
                    RunOnUiThread(() =>
                    {
                        Log.Text = text.ToString() + "\n" + Log.Text;
                    }))
                .DisposeWith(compositeDisposables);
        }

        private void ObservableAsync()
        {
            var gitHubService = new GitHubService();
            var button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += (e, s) => {
                Log.TextFormatted = null;
                var asyncStream = Observable.FromAsync(_ => gitHubService.GetUserByName("om"));
                
                asyncStream
                    //.Timeout(TimeSpan.FromSeconds(2))
                    //.Retry(3)
                    //.Catch<List<GitHubUserInfo>, Exception>(ex => Observable.Return<List<GitHubUserInfo>>(null))
                    .Subscribe(usersList =>
                        RunOnUiThread(() =>
                        {
                            if (usersList == null)
                                Log.TextFormatted = Html.FromHtml("<font color='red'>Erro</font>");
                            else
                            {
                                string users = "";
                                foreach (var user in usersList)
                                {
                                    users = users + user.Login.ToString() + "<br />";
                                }
                                Log.TextFormatted = Html.FromHtml(users);
                            }
                        }))
                    .DisposeWith(compositeDisposables);
            };
        }

        private IObservable<EventPattern<TextChangedEventArgs>> CreateObservableFromEvent()
        {
            return Observable.FromEventPattern<EventHandler<TextChangedEventArgs>, TextChangedEventArgs>(
                                            x => UserNameEditText.TextChanged += x,
                                            x => UserNameEditText.TextChanged -= x
                                        );
        }

        protected override void OnDestroy()
        {
            compositeDisposables.Clear();

            base.OnDestroy();
        }

        public EditText UserNameEditText { get; private set; }

        public TextView Log { get; private set; }
    }
}


