﻿namespace JetStreamCommons.FlightFilter
{
  using System;

  public interface IFlightFilterUserInput
  {
    decimal MaxPrice { get; }
    DateTime EarliestDepartureTime { get; }
    DateTime LatestDepartureTime { get; }
    TimeSpan MaxDuration {get;}

    bool IsRedEyeFlight {get;}
    bool IsInFlightWifiIncluded {get;}
    bool IsPersonalEntertainmentIncluded {get;}
    bool IsFoodServiceIncluded {get;}
  }
}

