﻿using MapKit;
using System.Collections.Generic;
using UIKit;
using System;

namespace JetStreamIOSFull.MapUI
{
  public class MapDelegate : MKMapViewDelegate 
  {
    private const double iphoneScaleFactorLatitude = 1024.0 / 80;
    private const double iphoneScaleFactorLongitude = 768.0 / 80;

    private double prevZoomLevel = 0;

    protected string annotationIdentifier = "BasicAnnotation";

    private List<DestinationAnnotation> annotations = new List<DestinationAnnotation>();

    public void SetAnnotationsForMap(MKMapView mapView, List<DestinationAnnotation> annotations)
    {
      this.annotations = annotations;
      this.FilterAnnotations(mapView);
    }

    public void  AddAnnotationForMap(MKMapView mapView,  DestinationAnnotation annotation)
    {
      annotations.Add(annotation);
      this.FilterAnnotations(mapView);
    }

    public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation ann)
    {
      DestinationAnnotation annotation = ann as DestinationAnnotation;
      AnnotationViewWithRoundedImage annotationView = mapView.DequeueReusableAnnotation(annotationIdentifier) as AnnotationViewWithRoundedImage;   

      if (annotationView == null)
        annotationView = new AnnotationViewWithRoundedImage(annotation, annotationIdentifier);
      else 
        annotationView.Annotation = annotation;

      annotationView.CanShowCallout = true;

      if (annotation is DestinationAnnotation)
      {
        DestinationAnnotation castedAnnotation = annotation as DestinationAnnotation;
        UIImage image = castedAnnotation.Image;

        if ( image != null) 
        {
            annotationView.Image = image;
        }
      }

      annotationView.HiddenCountChanged(annotation.HiddenCount);

      return annotationView;
    }     

    private void ClearAnnotations(MKMapView mapView)
    {
      InvokeOnMainThread(() =>
      {
        for (int i = 0; i < this.annotations.Count; ++i)
        {
          DestinationAnnotation checkingLocation = this.annotations [i];
          checkingLocation.HiddenCount = 1;
          mapView.RemoveAnnotation(checkingLocation);
        }
      });
    }

    private void FilterAnnotations(MKMapView mapView)
    {
      this.ClearAnnotations(mapView);

      InvokeOnMainThread(() =>
      {
        double latDelta = (mapView.Region.Span.LatitudeDelta/iphoneScaleFactorLatitude);
        double longDelta = (mapView.Region.Span.LongitudeDelta/iphoneScaleFactorLongitude);

        List<DestinationAnnotation> shopsToShow = new List<DestinationAnnotation>();

        for (int i = 0; i < this.annotations.Count; ++i) 
        {
          DestinationAnnotation checkingLocation = this.annotations[i];
          double latitude = checkingLocation.Coordinate.Latitude;
          double longitude = checkingLocation.Coordinate.Longitude;

          bool shouldBeHiden = false;

          foreach (DestinationAnnotation tempPlacemark in shopsToShow) 
          {
            shouldBeHiden = Math.Abs(tempPlacemark.Coordinate.Latitude - latitude) < latDelta
                         && Math.Abs(tempPlacemark.Coordinate.Longitude - longitude) < longDelta;

            if (shouldBeHiden)
            {
              InvokeOnMainThread(() =>
              {
                mapView.RemoveAnnotation(checkingLocation);
                tempPlacemark.HiddenCount = tempPlacemark.HiddenCount + 1;
              });
              break;
            }
          }

          if (!shouldBeHiden)
          {
            shopsToShow.Add(checkingLocation);
          }

        }

        foreach (IMKAnnotation annotation in shopsToShow) 
        {
          mapView.AddAnnotation(annotation);
        }
      });

    }

    public override void RegionChanged (MKMapView mapView, bool animated)
    {
      double newZoomLevel = MapHelper.GetZoomLevel(mapView);
      bool shouldRefilter = Math.Abs(newZoomLevel - prevZoomLevel) > 0.3;

      if (shouldRefilter)
      {
        this.FilterAnnotations(mapView); 
        prevZoomLevel = newZoomLevel;
      }
    }

//    public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
//    {
//      
//    }
//
//    public override void DidDeselectAnnotationView(MKMapView mapView, MKAnnotationView view)
//    {
//
//    }

  }
}

