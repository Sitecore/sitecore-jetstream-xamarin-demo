﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UIKit;
using MapKit;
using Foundation;
using CoreLocation;

using JetStreamCommons;
using JetStreamIOSFull.MapUI;
using JetStreamCommons.Destinations;
using JetStreamIOSFull.Helpers;


namespace JetStreamIOSFull
{
  public partial class DetailViewController : BaseViewController, IMKMapViewDelegate
  {
    private IEnumerable destinations;

    private MapManager mapManager;

    public DetailViewController(IntPtr handle) : base(handle)
    {
      
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      this.mapManager = new MapManager(this.Appearance);
      this.map.Delegate = mapManager;

    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      this.RefreshMap();
    }

    partial void RefreshButtonTouched(Foundation.NSObject sender)
    {
      this.RefreshMap();
    }

    private async void RefreshMap()
    {
      bool destinationsLoaded = false;
      try
      {
        this.destinations = await this.DownloadAllDestinations();
        destinationsLoaded = true;
      }
      catch
      {
        destinationsLoaded = false;
        AlertHelper.ShowLocalizedAlertWithOkOption("NETWORK_ERROR_TITLE", "CANNOT_DOWNLOAD_DESTINATIONS_ERROR");
      }
  
      if (destinationsLoaded)
      {
        this.ShowCurrentDestinationsOnMap();
      }

    }

    private void ShowCurrentDestinationsOnMap()
    {
      this.mapManager.ResetMapState(this.map);

      foreach(IDestination elem in this.destinations)
      {
        bool coordinatesAvailable = (elem.Latitude != 0.0) && (elem.Longitude != 0.0); 
        if (coordinatesAvailable)
        {
          CLLocationCoordinate2D coordinates = new CLLocationCoordinate2D(elem.Latitude, elem.Longitude);

          string instanceUrl = this.Endpoint.InstanceUrl;
          string imagePath = elem.ImagePath;
          string imageUrl = String.Concat(instanceUrl, imagePath);

          DestinationAnnotation annotation = new DestinationAnnotation(elem.DisplayName, imageUrl, coordinates, this.Appearance);
          this.mapManager.AddAnnotationForMap(this.map, annotation);

        }
      }
    }

    private async Task<IEnumerable> DownloadAllDestinations()
    {
     
        //FIXME: error here, some objects must be disposed!!!

        var session = this.Endpoint.GetSession();
        using (var loader = new DestinationsLoader(session))
        {
          return await loader.LoadOnlyDestinations();
        }
     

    }

    public override void DidReceiveMemoryWarning()
    {
      base.DidReceiveMemoryWarning();
      // Release any cached data, images, etc that aren't in use.
    }
  }
}


