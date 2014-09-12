using JetStreamIOS.ViewControllers.FlightsTable;

namespace JetStreamIOS
{
  using System;
  using System.Collections.Generic;

  using MonoTouch.Foundation;
  using MonoTouch.UIKit;

  using JetStreamCommons;
  using JetStreamCommons.Flight;
  using JetStreamCommons.FlightSearch;
  using Sitecore.MobileSDK.API.Session;


	public partial class FlightListViewController : UIViewController
	{
    #region Injected Variables
    private bool IsFlyingBack { get; set; }
    public IFlightSearchUserInput SearchOptionsFromUser { get; set; }
    #endregion Injected Variables

    #region UIViewController
		public FlightListViewController(IntPtr handle) : base (handle)
		{
		}

    public override void ViewWillAppear(bool animated)
    {
      DateTime today = this.SearchOptionsFromUser.ForwardFlightDepartureDate;
      DateTime yesterday = today.AddDays(-1);
      DateTime tomorrow  = today.AddDays(+1);

      this.TodayDateLabel.Text = DateConverter.StringFromDateForUI(today);
      this.YesterdayDateLabel.Text = DateConverter.StringFromDateForUI(yesterday);
      this.TomorrowDateLabel.Text = DateConverter.StringFromDateForUI(tomorrow);

      this.YesterdayPriceLabel.Text = NSBundle.MainBundle.LocalizedString("PRICE_UNAVAILABLE", null);
      this.TomorrowPriceLabel.Text = NSBundle.MainBundle.LocalizedString("PRICE_UNAVAILABLE", null);

      this.ReloadData();
    }
    #endregion UIViewController

    #region IBAction
    partial void OnTomorrowButtonPressed (MonoTouch.Foundation.NSObject sender)
    {
      AlertHelper.ShowLocalizedAlertWithOkOption("tomorrow", "tomorrow");
    }

    partial void OnYesterdayButtonPressed (MonoTouch.Foundation.NSObject sender)
    {
      AlertHelper.ShowLocalizedAlertWithOkOption("yesterday", "yesterday");
    }
    #endregion IBAction


    private async void ReloadData()
    {
      // @adk : on top of NSUserDefaults singleton
      InstanceSettings endpoint = new InstanceSettings();
      ISitecoreWebApiSession webApiSession = endpoint.GetSession();
      using (RestManager jetStreamSession = new RestManager(webApiSession))
      {
        var loader = new FlightSearchLoader(
          jetStreamSession,
          this.SearchOptionsFromUser.SourceAirport,
          this.SearchOptionsFromUser.DestinationAirport,
          this.SearchOptionsFromUser.ForwardFlightDepartureDate);

        IEnumerable<IJetStreamFlight> flights = await loader.GetFlightsForTheGivenDateAsync();
        var tableSource = new FlightsTableViewEnumerableDataSource(flights);
        this.FlightsTableView.DataSource = tableSource;
        this.FlightsTableView.ReloadData();

        DaySummary yesterday = await loader.GetPreviousDayAsync();
        DaySummary tomorrow = await loader.GetNextDayAsync();

        this.YesterdayDateLabel.Text = DateConverter.StringFromDateForUI(yesterday.DepartureDate);


        this.YesterdayPriceLabel.Text = yesterday.LowestPrice.HasValue ? 
            yesterday.LowestPrice.Value.ToString("C") : 
            NSBundle.MainBundle.LocalizedString("PRICE_UNAVAILABLE", null);

        this.TomorrowDateLabel.Text = DateConverter.StringFromDateForUI(tomorrow.DepartureDate);
        this.TomorrowPriceLabel.Text = tomorrow.LowestPrice.HasValue ? 
          tomorrow.LowestPrice.Value.ToString("C") :
          NSBundle.MainBundle.LocalizedString("PRICE_UNAVAILABLE", null);
      }
    }


	}
}