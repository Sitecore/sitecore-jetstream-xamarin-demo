﻿namespace JetStreamUnitTests
{
  using System;
  using NUnit.Framework;

  using JetStreamCommons;


  [TestFixture]
  public class SearchTicketsRequestBuilderTest
  {
    [Test]
    public void CorrectBuilderWillBuildRequest()
    {
      DateTime date = DateTime.Now;
      SearchFlightsRequest request = new SearchTicketsRequestBuilder ()
        .SourceAirport("SourceAirport")
        .DestinationAirport("DestinationAirport")
        .DepartureDate(date)
        .ReturnDate(date)
        .RoundTrip(true)
        .Build();

      Assert.IsNotNull(request, "request must no be null");
      Assert.AreEqual (request.FromAirportId, "SourceAirport", "wrong source airport value");
      Assert.AreEqual (request.ToAirportId, "DestinationAirport", "wrong destination airport value");
      Assert.AreEqual (request.DepartDate, date, "wrong departure date airport value");
      Assert.AreEqual (request.ReturnDate, date, "wrong returning date airport value");
      Assert.AreEqual (request.RoundTrip, true, "wrong returning date airport value");
    }

    [Test ()]
    public void EveryFieldCanBeSetTwice()
    {
      DateTime date = DateTime.Now;
      DateTime secondDate = date.AddDays(1);

      SearchFlightsRequest request = new SearchTicketsRequestBuilder ()
        .SourceAirport("SourceAirport")
        .DestinationAirport("DestinationAirport")
        .DepartureDate(date)
        .ReturnDate(date)
        .RoundTrip(false)
        .SourceAirport("SourceAirport1")
        .DestinationAirport("DestinationAirport1")
        .DepartureDate(secondDate)
        .ReturnDate(secondDate)
        .RoundTrip(true)
        .Build();

      Assert.IsNotNull(request, "request must no be null");
      Assert.AreEqual (request.FromAirportId, "SourceAirport1", "wrong source airport value");
      Assert.AreEqual (request.ToAirportId, "DestinationAirport1", "wrong destination airport value");
      Assert.AreEqual (request.DepartDate, secondDate, "wrong departure date airport value");
      Assert.AreEqual (request.ReturnDate, secondDate, "wrong returning date airport value");
      Assert.AreEqual (request.RoundTrip, true, "wrong returning date airport value");
    }

    [Test ()]
    public void ReturnDateIsNullIfRoundtripValueIsFalse()
    {
      DateTime date = DateTime.Now;
      SearchFlightsRequest request = new SearchTicketsRequestBuilder ()
        .SourceAirport("SourceAirport")
        .DestinationAirport("DestinationAirport")
        .DepartureDate(date)
        .ReturnDate(date)
        .RoundTrip(false)
        .Build();

      Assert.IsNotNull(request, "request must no be null");
      Assert.AreEqual (request.FromAirportId, "SourceAirport", "wrong source airport value");
      Assert.AreEqual (request.ToAirportId, "DestinationAirport", "wrong destination airport value");
      Assert.AreEqual (request.DepartDate, date, "wrong departure date airport value");
      Assert.IsNull   (request.ReturnDate, "wrong returning date airport value");
      Assert.AreEqual (request.RoundTrip, false, "wrong returning date airport value");
    }

    [Test ()]
    public void WillCrashWithoutFromAirport()
    {
      SearchTicketsRequestBuilder builder = new SearchTicketsRequestBuilder ()
        .DestinationAirport("bla")
        .DepartureDate(DateTime.Now)
        .ReturnDate(DateTime.Now);

      TestDelegate action = () => builder.Build();
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test ()]
    public void WillCrashWithoutDepartureDate()
    {
      SearchTicketsRequestBuilder builder = new SearchTicketsRequestBuilder ()
        .DestinationAirport("bla")
        .SourceAirport("bla")
        .RoundTrip(true);

      TestDelegate action = () => builder.Build();
      Assert.Throws<ArgumentNullException>(action);
    }

    [Test ()]
    public void WillCrashWithWrongDepartureDate()
    {
      DateTime date = DateTime.Now;
      date = date.AddDays(-1);//past

      SearchTicketsRequestBuilder builder = new SearchTicketsRequestBuilder ()
        .DestinationAirport("bla")
        .SourceAirport("bla")
        .DepartureDate(date)
        .RoundTrip(true);

      TestDelegate action = () => builder.Build();
      Assert.Throws<ArgumentNullException>(action);
    }

  }
}

