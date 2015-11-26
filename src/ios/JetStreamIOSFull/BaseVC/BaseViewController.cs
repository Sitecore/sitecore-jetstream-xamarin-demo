﻿using System;
using UIKit;
using JetStreamIOSFull.Helpers;
using CoreGraphics;
using Foundation;

namespace JetStreamIOSFull.BaseVC
{
  public partial class BaseViewController  : UIViewController
  {
    private AppearanceHelper appearanceHelper;
    private InstanceSettings.InstanceSettings endpoint;

    public UINavigationItem RealNavigationItem;
    public UIViewController BaseVC;

    private LoadingOverlay loadingOverlay;

    public BaseViewController(IntPtr handle) : base(handle)
    {

    }

    public void ShowLoader()
    {
      this.loadingOverlay = new LoadingOverlay (this.View.Bounds, NSBundle.MainBundle.LocalizedString ("Loading...", null));
      View.Add (loadingOverlay);
    }

    public void HideLoader()
    {
      if (this.loadingOverlay != null)
      {
        this.loadingOverlay.Hide ();
      }
    }

    public InstanceSettings.InstanceSettings Endpoint
    {
      get
      {
        if (this.endpoint == null)
        {
          throw new NullReferenceException("Endpoint value must be initialized");
        }

        return this.endpoint;
      }
      set
      { 
        this.endpoint = value;
      }
    }

    public AppearanceHelper Appearance
    {
      get
      {
        if (this.appearanceHelper == null)
        {
          throw new NullReferenceException("Appearance value must be initialized");

        }

        return this.appearanceHelper;
      }
      set
      { 
        this.appearanceHelper = value;
        this.InitializeNavBarLogo();
      }
    }

    private void InitializeNavBarLogo()
    {
      if (this.NavigationItem != null)
      {
        UIImage image = this.appearanceHelper.Common.NavigationBarLogo;
        UIImageView imageView = new UIImageView(new CGRect(0, 0, image.Size.Width, image.Size.Height));
        imageView.Image = image;
        this.NavigationItem.TitleView = imageView;
      }
    }
  }
}

