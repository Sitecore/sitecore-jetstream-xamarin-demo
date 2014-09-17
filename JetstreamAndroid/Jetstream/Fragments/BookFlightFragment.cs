﻿namespace JetstreamAndroid.Fragments
{
  using System;
  using Android.App;
  using Android.OS;
  using Android.Text;
  using Android.Views;
  using Android.Widget;
  using JetstreamAndroid.Adapters;
  using JetStreamCommons.Airport;

  public class BookFlightFragment : Fragment
  {
    private const int DialogDepart = 1;
    private const int DialogReturn = 2;

    private DateTime departDate = DateTime.Today;
    private DateTime returnDate = DateTime.Today.AddDays(1);

    private IJetStreamAirport fromAirport;
    private IJetStreamAirport toAirport;

    private Button departDateButton;
    private Button returnDateButton;

    private TextView returnDateTextView;

    private CheckBox roundTripCheckBox;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup viewGroup, Bundle bundle)
    {
      var root = inflater.Inflate(Resource.Layout.fragment_search, viewGroup, false);

      this.InitializeButtons(root);

      this.returnDateTextView = root.FindViewById<TextView>(Resource.Id.textview_return_date);

      this.roundTripCheckBox = root.FindViewById<CheckBox>(Resource.Id.checkBox_roundtrip);
      this.roundTripCheckBox.Click += (o, e) =>
      {
        if (this.roundTripCheckBox.Checked)
        {
          this.ShowReturnDate();
        }
        else
        {
          this.HideReturnDate();
        }
      };

      this.UpdateDepartDate(DateTime.Today);
      this.InitFields(root);
      return root;
    }

    private void InitFields(View root)
    {
      var autoCompleteAdapter = new AutoCompleteAdapter(Activity, Android.Resource.Layout.SimpleDropDownItem1Line, new string[] { });

      var fromField = root.FindViewById<AutoCompleteTextView>(Resource.Id.field_from_location);
      var toField = root.FindViewById<AutoCompleteTextView>(Resource.Id.field_to_location);

      fromField.TextChanged += (sender, args) =>
      {
        this.fromAirport = null;
      };

      fromField.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
      {
        this.fromAirport = autoCompleteAdapter.SearchedAirports[args.Position];
      };

      toField.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
      {
        this.toAirport = autoCompleteAdapter.SearchedAirports[args.Position];
      };

      toField.TextChanged += (sender, args) =>
      {
        this.toAirport = null;
      };

      fromField.Adapter = autoCompleteAdapter;
      toField.Adapter = autoCompleteAdapter;
    }

    private void InitializeButtons(View root)
    {
      this.departDateButton = root.FindViewById<Button>(Resource.Id.button_depart_date);
      this.departDateButton.Click += (sender, args) => this.ShowDialogForDate(departDate, DialogDepart);

      this.returnDateButton = root.FindViewById<Button>(Resource.Id.button_return_date);
      this.returnDateButton.Click += (sender, args) => this.ShowDialogForDate(returnDate, DialogReturn);

      var searchButton = root.FindViewById<Button>(Resource.Id.button_search_tickets);
      searchButton.Click += delegate(object sender, EventArgs args)
      {

      };
    }

    private void ShowDialogForDate(DateTime time, int dialogId)
    {
      EventHandler<DatePickerDialog.DateSetEventArgs> action = null;

      switch (dialogId)
      {
        case DialogDepart:
          action = (sender, e) => this.UpdateDepartDate(e.Date);
          break;
        case DialogReturn:
          action = (sender, e) => this.UpdateReturnDate(e.Date);
          break;
      }

      new DatePickerDialog(Activity, action, time.Year, time.Month - 1, time.Day).Show();
    }

    private void UpdateReturnDate(DateTime date)
    {
      this.returnDate = date.Date;
      this.returnDateButton.Text = date.Date.ToShortDateString();
    }

    private void UpdateDepartDate(DateTime date)
    {
      this.departDate = date.Date;
      this.departDateButton.Text = date.Date.ToShortDateString();
    }

    private void HideReturnDate()
    {
      this.returnDateButton.Visibility = ViewStates.Gone;
      this.returnDateTextView.Visibility = ViewStates.Gone;
    }

    private void ShowReturnDate()
    {
      this.returnDateButton.Visibility = ViewStates.Visible;
      this.returnDateTextView.Visibility = ViewStates.Visible;
    }
  }
}