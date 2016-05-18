using System;
using ReactiveUI;
using Android.Views;

namespace HackDaysRxUIDroid
{
	public class BooleanToVisibilityTypeConverter : IBindingTypeConverter
	{
		public int GetAffinityForObjects (Type fromType, Type toType)
		{
			return 100;
		}

		public bool TryConvert (object from, Type toType, object conversionHint, out object result)
		{
			if (toType == typeof(ViewStates))
			{
				bool fromAsBool = (bool) from;
				result = fromAsBool ? ViewStates.Visible : ViewStates.Gone;
				return true;
			}
			else
			{
				ViewStates fromAsViewStates = (ViewStates) from;
				result = fromAsViewStates == ViewStates.Visible ? true : false;
				return true;
			}
		}
	}
}

