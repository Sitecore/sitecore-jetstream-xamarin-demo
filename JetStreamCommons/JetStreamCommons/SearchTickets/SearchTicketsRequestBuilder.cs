﻿using System;

namespace JetStreamCommons
{
  public class SearchTicketsRequestBuilder
  {
    public SearchTicketsRequestBuilder()
    {
    }

    public SearchTicketsRequestBuilder SetFromAirportItem(string airportId)
    {
      this.fromAirportId = airportId;
      return this;
    }

    public SearchTicketsRequestBuilder SetToAirportItem(string airportId)
    {
      this.toAirportId = airportId;
      return this;
    }

    public SearchTicketsRequestBuilder SetDepartDate(DateTime date)
    {
      this.departDate = date;
      return this;
    }

    public SearchTicketsRequestBuilder SetReturnDate(DateTime date)
    {
      this.returnDate = date;
      return this;
    }

    public SearchFlightsRequest Build()
    {
      this.CheckRequestDataWithExeption();

      SearchFlightsRequest request = new SearchFlightsRequest (
                                        this.fromAirportId,
                                        this.toAirportId,
                                        this.departDate,
                                        this.returnDate
                                     );
      return request;
    }

    private void CheckRequestDataWithExeption()
    {
      if (null == this.fromAirportId)
      {
        throw new ArgumentNullException("FROM_AIRPORT_IS_NULL");
      }

      if (null == this.toAirportId)
      {
        throw new ArgumentNullException("TO_AIRPORT_IS_NULL");
      }

      if (null == this.departDate)
      {
        throw new ArgumentNullException("DEPART_DATE_IS_NULL");
      }

      DateTime pastDate = DateTime.Now;
      pastDate = pastDate.AddDays(-1);

      if (pastDate.Date > this.departDate.Date)
      {
        throw new ArgumentNullException("DEPART_DATE_MUST_BE_A_FUTURE");
      }

      if (pastDate.Date > this.returnDate.Date)
      {
        throw new ArgumentNullException("RETURN_DATE_MUST_BE_A_FUTURE");
      }

      if (departDate.Date > returnDate.Date)
      {
        throw new ArgumentException("RETURN_DATE_MUST_BE_AFTER_DEPARTURE");
      }
    }

    private string fromAirportId;
    private string toAirportId;
    private DateTime departDate;
    private DateTime returnDate;
  }
}

