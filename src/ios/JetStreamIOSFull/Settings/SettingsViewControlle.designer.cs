// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JetStreamIOSFull.Settings
{
	[Register ("SettingsViewControlle")]
	partial class SettingsViewControlle
	{
		[Outlet]
		UIKit.UIImageView BackgroundImageView { get; set; }

		[Outlet]
		UIKit.UITableView HistoryTableView { get; set; }

		[Outlet]
		UIKit.UITextField UrlTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BackgroundImageView != null) {
				BackgroundImageView.Dispose ();
				BackgroundImageView = null;
			}

			if (UrlTextField != null) {
				UrlTextField.Dispose ();
				UrlTextField = null;
			}

			if (HistoryTableView != null) {
				HistoryTableView.Dispose ();
				HistoryTableView = null;
			}
		}
	}
}
