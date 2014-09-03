// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using JetStreamCommons;
using System.Collections;
using Sitecore.MobileSDK.API.Items;
using System.Threading.Tasks;

namespace JetStreamIOS
{
	public partial class SearchViewController : UIViewController
	{
    private SearchTicketsRequestBuilder SearchRequestBuilder;

		public SearchViewController (IntPtr handle) : base (handle)
		{
		}
      
    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      this.LocalizeUI();

      SearchRequestBuilder = new  SearchTicketsRequestBuilder();
    }

    private void LocalizeUI()
    {
      string searchButtonTitle = NSBundle.MainBundle.LocalizedString("SEARCH_FLIGHTS_BUTTON_TITLE", null);
      this.SearchButton.SetTitle(searchButtonTitle, UIControlState.Normal);

      string businessClassButtonTitle = NSBundle.MainBundle.LocalizedString("BUSINESS_CLASS_BUTTON_TITLE", null);
      string economyClassButtonTitle = NSBundle.MainBundle.LocalizedString("ECONOMY_CLASS_BUTTON_TITLE", null);
      string firstClassButtonTitle = NSBundle.MainBundle.LocalizedString("FIRST_CLASS_BUTTON_TITLE", null);
      this.ClassSegmentedControl.SetTitle(businessClassButtonTitle, 0);
      this.ClassSegmentedControl.SetTitle(economyClassButtonTitle, 1);
      this.ClassSegmentedControl.SetTitle(firstClassButtonTitle, 2);

      this.DepartTitleLabel.Text = NSBundle.MainBundle.LocalizedString("DEPART_TITLE", null);
      this.ReturnTitleLabel.Text = NSBundle.MainBundle.LocalizedString("RETURN_TITLE", null);
      this.ClassTitleLabel.Text = NSBundle.MainBundle.LocalizedString("CLASS_TITLE", null);
      this.CountTitleLabel.Text = NSBundle.MainBundle.LocalizedString("COUNT_TITLE", null);
      this.RoundtripTitleLabel.Text = NSBundle.MainBundle.LocalizedString("ROUNDTRIP_TITLE", null);

      this.FromLocationTextField.Placeholder = NSBundle.MainBundle.LocalizedString("FROM_LOCATION_PLACEHOLDER", null); 
      this.ToLocationTextField.Placeholder = NSBundle.MainBundle.LocalizedString("TO_LOCATION_PLACEHOLDER", null);
    }

    partial void OnSearchButtonTouched (MonoTouch.UIKit.UIButton sender)
    {
//      DateTime departDate = DateTime.Parse(DepartDateButton.TitleLabel.Text);
//      DateTime returnDate = DateTime.Parse(ReturnDateButton.TitleLabel.Text);
//      ScItemsResponse departFlights = this.SearchTicketsWithFlightDate(departDate);
//      ScItemsResponse returnFlights = this.SearchTicketsWithFlightDate(returnDate);
    }

    private async void SearchTicketsWithFlightDate(DateTime date)
    {
      DateTime departDate = DateTime.Parse(DepartDateButton.TitleLabel.Text);
      DateTime returnDate = DateTime.Parse(ReturnDateButton.TitleLabel.Text);

      SearchFlightsRequest request = SearchRequestBuilder
        .SetDepartDate (departDate)
        .SetReturnDate (returnDate)
        .Build ();

      if (null == request.FromAirportId)
      {
        //please select departure airport
      }

      if (null == request.ToAirportId)
      {
        //please select arrival airport
      }

      //TODO: show flights list VC here
      //TODO: move this to flights list VC and make this method sync
      RestManager restManager = new RestManager();
      ScItemsResponse result = await restManager.SearchDepartTicketsWithRequest(request);
    }

    partial void CountValueChanged (MonoTouch.UIKit.UIStepper sender)
    {
      ResultCountLabel.Text = sender.Value.ToString();
    }

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      base.PrepareForSegue(segue, sender);

      if ("ToAirportQuickSearch" == segue.Identifier)
      {
        var searchAirportsViewController = segue.DestinationViewController as SearchAirportTableViewController;
        searchAirportsViewController.NameToSearch = this.ToLocationTextField.Text;
      }
      else
        if ("FromAirportQuickSearch" == segue.Identifier)
        {
          var searchAirportsViewController = segue.DestinationViewController as SearchAirportTableViewController;
          searchAirportsViewController.NameToSearch = this.FromLocationTextField.Text;
        }
    }
	}
}
