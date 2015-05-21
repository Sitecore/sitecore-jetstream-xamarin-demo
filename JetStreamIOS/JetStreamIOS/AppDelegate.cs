﻿using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Diagnostics;

namespace JetStreamIOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the
  // User Interface of the application, as well as listening (and optionally responding) to
  // application events from iOS.
  [Register ("AppDelegate")]
  public partial class AppDelegate : UIApplicationDelegate
  {
    // class-level declarations
    
    public override UIWindow Window
    {
      get;
      set;
    }

//    public override void FinishedLaunching(UIApplication application)
//    {
//      var timezones = TimeZoneInfo.GetSystemTimeZones();
//      foreach (TimeZoneInfo singleTimezone in timezones)
//      {
//        // singleTimezone.StandardName ==> GMT / EST
//
//        Debug.WriteLine("Timezone : {0}| Name : {1}", singleTimezone.ToString(), singleTimezone.StandardName);
//      }
//    }

    // This method is invoked when the application is about to move from active to inactive state.
    // OpenGL applications should use this method to pause.
    public override void OnResignActivation(UIApplication application)
    {
    }
    
    // This method should be used to release shared resources and it should store the application state.
    // If your application supports background exection this method is called instead of WillTerminate
    // when the user quits.
    public override void DidEnterBackground(UIApplication application)
    {
    }
    
    // This method is called as part of the transiton from background to active state.
    public override void WillEnterForeground(UIApplication application)
    {
    }
    
    // This method is called when the application is about to terminate. Save data, if needed.
    public override void WillTerminate(UIApplication application)
    {
    }
  }
}

