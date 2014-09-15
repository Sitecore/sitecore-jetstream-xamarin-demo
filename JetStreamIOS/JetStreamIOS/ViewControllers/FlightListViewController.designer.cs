// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace JetStreamIOS
{
	[Register ("FlightListViewController")]
	partial class FlightListViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem FilterToolbarButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITableView FlightsTableView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TodayDateLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TomorrowDateLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel TomorrowPriceLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel YesterdayDateLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel YesterdayPriceLabel { get; set; }

		[Action ("OnTomorrowButtonPressed:")]
		partial void OnTomorrowButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("OnYesterdayButtonPressed:")]
		partial void OnYesterdayButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("unwindToFlightList:")]
		partial void unwindToFlightList (MonoTouch.UIKit.UIStoryboardSegue unwindSegue);
		
		void ReleaseDesignerOutlets ()
		{
			if (FilterToolbarButton != null) {
				FilterToolbarButton.Dispose ();
				FilterToolbarButton = null;
			}

			if (FlightsTableView != null) {
				FlightsTableView.Dispose ();
				FlightsTableView = null;
			}

			if (TodayDateLabel != null) {
				TodayDateLabel.Dispose ();
				TodayDateLabel = null;
			}

			if (TomorrowDateLabel != null) {
				TomorrowDateLabel.Dispose ();
				TomorrowDateLabel = null;
			}

			if (TomorrowPriceLabel != null) {
				TomorrowPriceLabel.Dispose ();
				TomorrowPriceLabel = null;
			}

			if (YesterdayDateLabel != null) {
				YesterdayDateLabel.Dispose ();
				YesterdayDateLabel = null;
			}

			if (YesterdayPriceLabel != null) {
				YesterdayPriceLabel.Dispose ();
				YesterdayPriceLabel = null;
			}
		}
	}
}
