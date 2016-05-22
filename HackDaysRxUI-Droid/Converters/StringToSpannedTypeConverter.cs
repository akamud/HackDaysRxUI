using System;
using ReactiveUI;
using Android.Views;
using Android.Text;

namespace HackDaysRxUIDroid.Converters
{
	public class StringToSpannedTypeConverter : IBindingTypeConverter
	{
		public int GetAffinityForObjects (Type fromType, Type toType)
		{
			return 100;
		}

		public bool TryConvert (object from, Type toType, object conversionHint, out object result)
		{
            var fromAsString = from.ToString();

            if (!string.IsNullOrWhiteSpace(fromAsString))
			{
                result = Html.FromHtml(fromAsString);
                return true;
            }

            result = Html.FromHtml("");
            return true;
		}
	}
}

