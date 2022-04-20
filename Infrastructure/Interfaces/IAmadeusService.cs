using Models;
using Models.Amadeus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces;

public interface IAmadeusService
{
    public Task<FlightOfferResponse?> GetFlightOffers(FlightOfferRequest request);
}

