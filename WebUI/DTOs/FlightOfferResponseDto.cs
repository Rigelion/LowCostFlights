namespace WebUI.DTOs;

public class FlightOfferResponseDto
{
    public IEnumerable<FlightOfferDataDto> ReturnedFlights { get; set; } = null!;
    public DictionariesDto Dictionaries { get; set; } = null!;
}

public class DictionariesDto
{
    public IDictionary<string, string> Carriers { get; set; } = null!;
    public IDictionary<string, string> Aircrafts { get; set; } = null!;
}

public class FlightOfferDataDto
{
    public int NumberOfBookableSeats { get; set; }
    public IEnumerable<ItineraryDto> Itineraries { get; set; } = null!;
    public TimeSpan TotalDuration => Itineraries.Aggregate(TimeSpan.Zero, (subtotal, t) => subtotal.Add(t.Duration));
    public FlightOfferPriceDto Price { get; set; } = null!;
}

public class FlightOfferPriceDto
{
    public decimal TotalEur { get; set; }
    public decimal TotalHrk { get; set; }
    public decimal TotalUsd { get; set; }

}

public class ItineraryDto
{
    public IEnumerable<SegmentDto> Segments { get; set; } = null!;
    public TimeSpan Duration { get; set; }
}

public class SegmentDto
{
    public int NumberOfStops { get; set; }
    public TimeSpan Duration { get; set; }
    public IataModelDto Departure { get; set; } = null!;
    public IataModelDto Arrival { get; set; } = null!;

    public string Carrier { get; set; } = null!;
}