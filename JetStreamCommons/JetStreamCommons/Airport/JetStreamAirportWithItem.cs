﻿namespace JetStreamCommons.Airport
{
  using System;
  using Sitecore.MobileSDK.API.Items;


  public class JetStreamAirportWithItem : IJetStreamAirport
  {
    private ISitecoreItem item;

    public JetStreamAirportWithItem(ISitecoreItem item)
    {
      this.item = item;
    }

    public string Country 
    { 
      get
      {
        return this.item["Country"].RawValue;
      }
    }

    public string City 
    { 
      get
      {
        return this.item["City"].RawValue;
      }
    }

    public string Code 
    { 
      get
      {
        return this.item["Airport Code"].RawValue;
      }
    }

    public string Name
    { 
      get
      {
        return this.item["Airport Name"].RawValue;
      }
    }

    public string Id 
    { 
      get
      {
        return this.item.Id;
      }
    }

    public string DisplayName 
    { 
      get
      {
        return this.item.DisplayName;
      }
    }

  }
}

