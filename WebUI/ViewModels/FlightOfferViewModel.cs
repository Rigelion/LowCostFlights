using AutoMapper;
using FluentValidation;
using Infrastructure.Interfaces;
using Models;
using Models.Amadeus;
using Models.CurrencyConverter;
using WebUI.DTOs;

namespace WebUI.ViewModels;

public class FlightOfferViewModel
{
    readonly IAmadeusService _amadeusService;
    readonly ICurrencyConverterService _currencyConverterService;
    readonly IMapper _mapper;
    public FlightOfferViewModel(IAmadeusService amadeusService, IMapper mapper, ICurrencyConverterService currencyConverter)
    {
        _amadeusService = amadeusService;
        _mapper = mapper;
        _currencyConverterService = currencyConverter;
    }

    public IataModel Origin { get; set; } = null!;
    public IataModel Destination { get; set; } = null!;
    public DateTime DepartureDate { get; set; } = DateTime.Now.AddDays(1);
    public DateTime? ReturnDate { get; set; } = null!;
    public uint NumberOfPassengers { get; set; } = 1;

    public CurrencyTypeViewModel CurrencyType { get; set; } = GetCurrencyTypeViewModels().First();


    public FlightOfferResponseDto? Response { get; set; }

    public async Task GetFlightOfferResponses()
    {
        var resp = await _amadeusService.GetFlightOffers(new(Origin.Iata, Destination.Iata, DepartureDate, NumberOfPassengers, ReturnDate));
        var currencyRates = await _currencyConverterService.GetCurrencyRates();
        Response = _mapper.Map<FlightOfferResponseDto>(resp, opts => opts.Items[typeof(CurrencyRates).ToString()] = currencyRates);
    }

    public static IEnumerable<CurrencyTypeViewModel> GetCurrencyTypeViewModels() => Enum.GetValues<CurrencyType>().ToList().Select<CurrencyType, CurrencyTypeViewModel>(x => new(x));

}

public class FlightRequestViewModelValidator : AbstractValidator<FlightOfferViewModel>
{
    public FlightRequestViewModelValidator()
    {
        RuleFor(x => x.Origin).Cascade(CascadeMode.Stop).NotNull().WithMessage("Origin Airport cannot be empty!")
            .Must((x, origin) => x.Destination != origin).WithMessage("Origin Airport cannot be same as Destination Airport!");

        RuleFor(x => x.Destination).Cascade(CascadeMode.Stop).NotNull().WithMessage("Destination Airport cannot be empty!")
            .Must((x, destination) => x.Origin != destination).WithMessage("Destination Airport cannot be same as Origin Airport!");

        RuleFor(x => x.DepartureDate).Cascade(CascadeMode.Stop).NotNull().WithMessage("Departure Date cannot be empty")
            .GreaterThan(DateTime.Now).WithMessage("Departure Date cannot be today!");

        RuleFor(x => x.ReturnDate).Cascade(CascadeMode.Stop).GreaterThan(x => x.DepartureDate).WithMessage("Return Date cannot be before Departure Date!");

        RuleFor(x => x.NumberOfPassengers).GreaterThan((uint)0);
    }
}

public class CurrencyTypeViewModel
{
    public CurrencyTypeViewModel(CurrencyType currency)
    {
        CurrencyValue = currency;
        CurrencyName = currency.ToString();
    }

    public CurrencyType CurrencyValue { get; set; }
    public string CurrencyName { get; set; } = null!;
}

public enum CurrencyType
{
    EUR,
    HRK,
    USD
}

