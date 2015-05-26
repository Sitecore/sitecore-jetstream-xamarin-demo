﻿namespace Jetstream
{
  using Android.App;
  using Android.Content;
  using Android.Graphics;
  using Android.OS;
  using Android.Support.V7.App;
  using Android.Views;
  using Android.Widget;
  using Com.Lilarcor.Cheeseknife;
  using Com.Mikepenz.Google_material_typeface_library;
  using Com.Mikepenz.Iconics;
  using Com.Mikepenz.Materialdrawer;
  using Com.Mikepenz.Materialdrawer.Accountswitcher;
  using Com.Mikepenz.Materialdrawer.Model;
  using Com.Mikepenz.Materialdrawer.Model.Interfaces;
  using Com.Rengwuxian.Materialedittext;
  using Jetstream.UI.Fragments;
  using Jetstream.Utils;
  using Sitecore.MobileSDK;
  using Sitecore.MobileSDK.API;
  using Sitecore.MobileSDK.API.Session;
  using Sitecore.MobileSDK.PasswordProvider.Android;

  [Activity(MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : AppCompatActivity, Drawer.IOnDrawerItemClickListener
  {
    [InjectView(Resource.Id.toolbar)]
    private Android.Support.V7.Widget.Toolbar toolbar;

    private AccountHeader header = null;
    private Drawer drawer = null;
    private Prefs prefs;

    private DestinationsOnMapFragment mapFragment;

    protected override void OnCreate(Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);
      this.SetContentView(Resource.Layout.Main);

      this.prefs = Prefs.From(this);

      Cheeseknife.Inject(this);

      this.InitDrawer(savedInstanceState);
    }

    private void InitDrawer(Bundle savedInstanceState)
    {
      this.SetSupportActionBar(this.toolbar);

      this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
      this.SupportActionBar.SetHomeButtonEnabled(false);

      this.PrepareHeader(savedInstanceState);

      var destinations = new PrimaryDrawerItem();
      destinations.WithName(Resource.String.text_destinations_item);
      destinations.WithIcon(GoogleMaterial.Icon.GmdFlight);
      destinations.WithIdentifier(1);
      destinations.WithCheckable(false);

      var settings = new PrimaryDrawerItem();
      settings.WithName(Resource.String.text_settings_item);
      settings.WithIcon(GoogleMaterial.Icon.GmdSettings);
      settings.WithIdentifier(2);
      settings.WithCheckable(false);

      this.drawer = new DrawerBuilder()
                .WithActivity(this)
                .WithRootView(Resource.Id.drawer_container)
                .WithToolbar(this.toolbar)
                .WithAccountHeader(this.header)
                .AddDrawerItems(destinations, settings)
                .WithOnDrawerItemClickListener(this)
                .WithSavedInstance(savedInstanceState)
                .WithActionBarDrawerToggleAnimated(true)
                .Build();

      this.mapFragment = new DestinationsOnMapFragment();
      this.FragmentManager.BeginTransaction().Replace(Resource.Id.map_fragment_container, this.mapFragment).Commit();
    }

    private void PrepareHeader(Bundle savedInstanceState)
    {
      var profile = new ProfileDrawerItem()
        .WithName(this.GetString(Resource.String.text_default_user))
        .WithIcon(
              new IconicsDrawable(this, GoogleMaterial.Icon.GmdVerifiedUser)
        .ActionBarSize()
        .PaddingDp(5)
        .Color(Color.Black));

      this.header = new AccountHeaderBuilder()
        .WithActivity(this)
        .WithCompactStyle(true)
        .WithSelectionListEnabled(false)
        .WithHeaderBackground(Resource.Drawable.header)
        .AddProfiles(profile)
        .WithSavedInstance(savedInstanceState)
        .Build();

      this.header.View.Clickable = false;

      var email = this.header.View.FindViewById<TextView>(Resource.Id.account_header_drawer_email);
      email.Visibility = ViewStates.Gone;
    }

    public override void OnBackPressed()
    {
      if (this.drawer != null && this.drawer.IsDrawerOpen)
      {
        this.drawer.CloseDrawer();
      }
      else
      {
        base.OnBackPressed();
      }
    }

    protected override void OnSaveInstanceState(Bundle outState)
    {
      outState = this.drawer.SaveInstanceState(outState);
      outState = this.header.SaveInstanceState(outState);
      base.OnSaveInstanceState(outState);
    }

    public bool OnItemClick(AdapterView parent, Android.Views.View view, int position, long id, IDrawerItem drawerItem)
    {
      if (drawerItem == null)
      {
        return true;
      }

      this.drawer.SetSelectionByIdentifier(drawerItem.Identifier, false);

      switch (drawerItem.Identifier)
      {
        case 1:
          break;
        case 2:
          this.ShowSettingsDialog();
          break;
      }

      return false;
    }

    private void ShowSettingsDialog()
    {
      var rootView = LayoutInflater.From(this).Inflate(Resource.Layout.fragment_settings, null, false);

      var sitecoreUrlField = rootView.FindViewById<MaterialEditText>(Resource.Id.field_sitecore_url);
      sitecoreUrlField.Text = this.prefs.InstanceUrl;

      sitecoreUrlField.AddValidator(new SitecoreUrlValidator(this.GetString(Resource.String.error_wrong_url)));

      var builder = new Android.Support.V7.App.AlertDialog.Builder(this);
      builder.SetTitle(this.GetString(Resource.String.text_settings_item));

      builder.SetPositiveButton(this.GetString(Resource.String.text_button_apply), handler: null);
      builder.SetNegativeButton(this.GetString(Resource.String.text_button_cancel), handler: null);

      builder.SetView(rootView);
      var dialog = builder.Show();

      dialog.GetButton((int)DialogButtonType.Positive).SetOnClickListener(new ApplyButtonClickListener(sitecoreUrlField, this.prefs, dialog));

      new Handler().PostDelayed(() => this.drawer.SetSelectionByIdentifier(1, false), 500);
    }

    private class ApplyButtonClickListener : Java.Lang.Object, Android.Views.View.IOnClickListener
    {
      private MaterialEditText urlField;
      private Prefs prefs;
      private readonly Dialog dialog;

      protected internal ApplyButtonClickListener(MaterialEditText urlField, Prefs prefs, Dialog dialog)
      {
        this.urlField = urlField;
        this.prefs = prefs;
        this.dialog = dialog;
      }

      public void OnClick(Android.Views.View v)
      {
        if (this.urlField.Validate())
        {
          this.prefs.InstanceUrl = this.urlField.Text;
          this.dialog.Dismiss();
        }
      }

      protected override void Dispose(bool disposing)
      {
        base.Dispose(disposing);

        this.urlField = null;
        this.prefs = null;
      }
    }

    //TODO:
    public ScApiSession Session
    {
      get
      {
        ISitecoreWebApiSession session = null;
        //        if (isAuthentiated)
        //        {
        using (var credentials = new SecureStringPasswordProvider("sitecore\\admin", "b"))
        {
          session = SitecoreWebApiSessionBuilder.AuthenticatedSessionWithHost(this.prefs.InstanceUrl)
                        .Credentials(credentials)
                          .DefaultDatabase("web")
                        .BuildSession();
        }
        //        }
        //        else
        //        {
        //        session = SitecoreWebApiSessionBuilder.AnonymousSessionWithHost("jetstream800394rev150402.test24dk1.dk.sitecore.net")
        //          .DefaultDatabase("web")
        //            .BuildSession();
        //        }

        return (ScApiSession)session;
      }
    }
  }
}