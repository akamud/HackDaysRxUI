using Android.App;
using Android.Widget;
using Android.OS;
using HackDaysRxUICore;
using ReactiveUI;
using System;
using Android.Text;
using System.Reactive.Linq;
using Android.Text.Method;
using HackDaysRxUIDroid.Converters;
using System.Collections.Generic;
using System.Reactive.Disposables;
using HackDaysRxUICore.Helpers;

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

            #region Range
            //var rangeStream = Observable.Range(1, 10);

            //rangeStream.Subscribe(num =>
            //    {
            //        Log.Text = num.ToString() + "\n" + Log.Text;
            //    })
            //    .DisposeWith(compositeDisposables);
            #endregion

            #region Timer
            //var timerStream = Observable.Timer(TimeSpan.FromSeconds(3));

            //timerStream.Subscribe(num =>
            //{
            //    Log.Text = "Timer";
            //})
            //    .DisposeWith(compositeDisposables);
            #endregion

            #region Event
            var textStream = Observable.FromEventPattern<EventHandler<TextChangedEventArgs>, TextChangedEventArgs>(
                x => UserNameEditText.TextChanged += x,
                x => UserNameEditText.TextChanged -= x
            );

            textStream
                .Subscribe(args =>
                {
                    Log.Text = args.EventArgs.Text + "\n" + Log.Text;
                });
            #endregion

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


