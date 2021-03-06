﻿using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using JetStreamCommons.Destinations;
using SDWebImage;
using JetStreamCommons;
using InstanceSettings;

namespace JetStreamIOSFull.DestinationDetails
{
  public class DestinationImagesSource : UITableViewSource
  {
    List<IAttraction> tableItems;
    string CellIdentifier = "DestinationImageCellId";
    private InstancesManager instancesManager;

    private IDestination baseDestination = null;

    public DestinationImagesSource(List<IAttraction> items, IDestination destination, InstancesManager instancesManager)
    {
      this.baseDestination = destination;
      this.instancesManager = instancesManager;
      this.tableItems = items;
    }

    public override nint RowsInSection (UITableView tableview, nint section)
    {
      return this.tableItems.Count + 1;
    }

    public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
    {
      DestinationImageCell cell = tableView.DequeueReusableCell(CellIdentifier) as DestinationImageCell;

      IItemWithImage item;

      if (indexPath.Row == 0)
      {
        item = this.baseDestination;
      }
      else
      {
        item = this.tableItems [indexPath.Row - 1];
      }

      cell.SetImage(null);
      cell.ActivityIndicator.StartAnimating();

      this.DownloadAndShowDestinationImage(item, tableView, cell);

      return cell;
    }

    private void DownloadAndShowDestinationImage(IItemWithImage attraction, UITableView tableView, DestinationImageCell cell)
    {
      string imagePath = null;

      try
      {
        imagePath = SitecoreWebApiSessionExt.MediaDownloadUrl(this.instancesManager.ActiveInstance.InstanceUrl, attraction.ImagePath);
        NSUrl imageUrl = new NSUrl(imagePath);

        cell.DestinationImageView.SetImage (
          url: imageUrl,
          placeholder: null,
          options: SDWebImageOptions.RetryFailed | SDWebImageOptions.LowPriority,
          completedBlock: (image, data, error, finished) =>
        {
          if (image != null)
          {
            cell.ActivityIndicator.StopAnimating();
          }
        });
      }
      catch(Exception ex)
      {
        Console.WriteLine("Can not download image: " + ex.ToString());
        Console.WriteLine("Image path: " + imagePath);
      }
    }
  }
}