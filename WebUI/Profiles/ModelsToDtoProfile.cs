using AutoMapper;
using Infrastructure.Interfaces;
using Models.Amadeus;
using Models.CurrencyConverter;
using WebUI.DTOs;

namespace WebUI.Profiles;

public class ModelsToDtoProfile : Profile
{
    public ModelsToDtoProfile()
    {
        CreateMap<FlightOfferResponse, FlightOfferResponseDto>();
        CreateMap<Dictionaries, DictionariesDto>();


        CreateMap<FlightOfferData, FlightOfferDataDto>();

        CreateMap<Itinerary, ItineraryDto>()
            .ForMember(dst => dst.Duration, opt => opt.MapFrom<DurationResolver>());


        CreateMap<Segment, SegmentDto>()
            .ForMember(dst => dst.NumberOfStops, opt => opt.MapFrom(src => src.NumberOfStops))
            .ForMember(dst => dst.Duration, opt => opt.MapFrom<DurationResolver>())
            .ForMember(dst => dst.Arrival, opt => opt.MapFrom<IataModelResolver, FlightEndPoint>(x => x.Arrival))
            .ForMember(dst => dst.Departure, opt => opt.MapFrom<IataModelResolver, FlightEndPoint>(x => x.Departure));

        CreateMap<FlightOfferPrice, FlightOfferPriceDto>()
            .ForMember(dst => dst.TotalHrk, opt => opt.MapFrom<EurToHrkResolver>())
            .ForMember(dst => dst.TotalUsd, opt => opt.MapFrom<EurToUsdResolver>())
            .ForMember(dst => dst.TotalEur, opt => opt.MapFrom(src => src.Total));
    }
}

public class IataModelResolver : IMemberValueResolver<Segment, SegmentDto, FlightEndPoint, IataModelDto>
{
    private IIataService _iataService;

    public IataModelResolver(IIataService iataService)
    {
        _iataService = iataService;
    }

    public IataModelDto Resolve(Segment source, SegmentDto destination, FlightEndPoint sourceMember, IataModelDto destMember, ResolutionContext context)
    {
        var iataModel = _iataService.GetIata(sourceMember.IataCode)!;

        return new() { City = iataModel.City, Country = iataModel.Country, Iata = iataModel.Iata, Name = iataModel.Name, State = iataModel.State };
    }
}

public class EurToHrkResolver : IValueResolver<FlightOfferPrice, FlightOfferPriceDto, decimal>
{
    private CurrencyRates _someService;

    public EurToHrkResolver(CurrencyRates someService)
    {
        _someService = someService;
    }

    public decimal Resolve(FlightOfferPrice source, FlightOfferPriceDto destination, decimal destMember, ResolutionContext context) =>
        _someService.EurToHrkRate * source.Total;
}

public class EurToUsdResolver : IValueResolver<FlightOfferPrice, FlightOfferPriceDto, decimal>
{
    private CurrencyRates _someService;

    public EurToUsdResolver(CurrencyRates someService)
    {
        _someService = someService;
    }

    public decimal Resolve(FlightOfferPrice source, FlightOfferPriceDto destination, decimal destMember, ResolutionContext context) =>
        _someService.EurToUsdRate * source.Total;
}



public class DurationResolver : IValueResolver<Itinerary, ItineraryDto, TimeSpan>, IValueResolver<Segment, SegmentDto, TimeSpan>
{
    const string HOURS_GROUP_NAME = "hours";
    const string MINUTES_GROUP_NAME = "minutes";
    public TimeSpan Resolve(Itinerary source, ItineraryDto destination, TimeSpan destMember, ResolutionContext context) =>
        ConvertTime(source.Duration);


    public TimeSpan Resolve(Segment source, SegmentDto destination, TimeSpan destMember, ResolutionContext context) =>
        ConvertTime(source.Duration);

    private TimeSpan ConvertTime(string timeSpan)
    {
        //Matches hours and minutes from string into groups, expects string in format: PT12H00M
        System.Text.RegularExpressions.Regex exp = new(@"^PT(?'hours'\d+H)?(?'minutes'\d+M)?$");
        System.Text.RegularExpressions.Match match = exp.Match(timeSpan);

        string hours = match.Groups[HOURS_GROUP_NAME].Success ? match.Groups[HOURS_GROUP_NAME].Value[..^1] : "0";
        string minutes = match.Groups[MINUTES_GROUP_NAME].Success ? match.Groups[MINUTES_GROUP_NAME].Value[..^1] : "0";


        return TimeSpan.FromMinutes((int.Parse(hours) * 60 + int.Parse(minutes)));

    }
}

