namespace JetStreamIOS
{
  using System;

  using MonoTouch.Foundation;
  using MonoTouch.UIKit;

  using JetStreamIOS.Helpers;
  using JetStreamCommons.Flight;


  public partial class FlightCell : UITableViewCell
	{

    #region Events
    public delegate void OnFlightSelectedDelegate(IJetStreamFlight flight);

    public OnFlightSelectedDelegate OnFlightSelected { get; set; }

    partial void OrderButtonPressed (MonoTouch.Foundation.NSObject sender)
    {
      this.OrderThisFlight();
    }

    public void OrderThisFlight()
    {
      this.OnFlightSelected(this.Flight);
    }

    #endregion Events


    #region Model
    public IJetStreamFlight Flight
    {
      get;
      private set;
    }

    public static NSString StaticReuseIdentifier()
    {
      return new NSString("FlightCell");
    }


    public void SetModel(IJetStreamFlight flight)
    {
      this.Flight = flight;

      this.PriceLabel.Text = flight.Price.ToString("C");

      string strDepartureTime = DateConverter.StringFromTimeForUI(flight.DepartureTime.ToLocalTime());
      string strArrivalTime = DateConverter.StringFromTimeForUI(flight.ArrivalTime.ToLocalTime());

      string departureFormat = NSBundle.MainBundle.LocalizedString("DEPARTURE_DATE_FORMAT", null);
      string arrivalFormat = NSBundle.MainBundle.LocalizedString("ARRIVAL_DATE_FORMAT", null);

      this.DepartureTimeLabel.Text = string.Format(departureFormat, strDepartureTime);
      this.ArrivalTimeLabel.Text = string.Format(arrivalFormat, strArrivalTime);
    }
    #endregion Model

    #region UITableViewCell
		public FlightCell (IntPtr handle) : base (handle)
		{
		}

    public FlightCell(UITableViewCellStyle style, NSString reuseIdentifier) : base(style, reuseIdentifier)
    {
    }
    #endregion UITableViewCell
	}
}
