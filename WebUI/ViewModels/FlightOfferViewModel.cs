using FluentValidation;
using Infrastructure.Interfaces;
using Models;
using Models.Amadeus;

namespace WebUI.ViewModels;

public class FlightOfferViewModel
{
    readonly IAmadeusService _amadeusService;
    public FlightOfferViewModel(IAmadeusService amadeusService)
    {
        _amadeusService = amadeusService;
    }

    public async Task GetFlightOfferResponses()
    {
        Response = await _amadeusService.GetFlightOffers(Origin, Destination, DepartureDate, NumberOfPassengers, ReturnDate);
    }

    public IataModel Origin { get; set; } = null!;
    public IataModel Destination { get; set; } = null!;
    public DateTime DepartureDate { get; set; } = DateTime.Now.AddDays(1);
    public DateTime? ReturnDate { get; set; } = null!;
    public uint NumberOfPassengers { get; set; } = 1;


    public FlightOfferResponse? Response { get; set; }
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

    //private bool IsDepartureDateValid(FlightOfferViewModel viewModel, DateTime departureDate)
    //{
    //    if (viewModel.ReturnDate != null && viewModel.ReturnDate < departureDate)
    //    {
    //        return false;
    //    }

    //    return true;
    //}
}

