using System;

using Foundation;
using UIKit;
using JetStreamIOSFull.BaseVC;

namespace JetStreamIOSFull.NOTIMPLEMENTED
{
  public partial class CheckInViewController : BaseViewController
	{
		public CheckInViewController (IntPtr handle) : base (handle)
		{
		}

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      this.TopImageView.Image = this.Appearance.CheckIn.TopImage;
      string name = NSBundle.MainBundle.LocalizedString("CHECK_IN_PLACEHOLDER_TITLE", null);
      this.TitleLabel.Text = name;
    }
	}
}
