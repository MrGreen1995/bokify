using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;

namespace Bookify.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IBookingRepository _bookingRespository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingService _pricingService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReserveBookingCommandHandler(
        IUserRepository userRepository, 
        IApartmentRepository apartmentRepository, 
        PricingService pricingService, 
        IUnitOfWork unitOfWork, 
        IBookingRepository bookingRespository, 
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _apartmentRepository = apartmentRepository;
        _pricingService = pricingService;
        _unitOfWork = unitOfWork;
        _bookingRespository = bookingRespository;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }
        
        var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);

        if (apartment is null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }
        
        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await _bookingRespository.IsOverlappingAsync(apartment, duration, cancellationToken))
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        try
        {
            var booking = Booking.Reserve(
                apartment,
                user.Id,
                duration,
                _dateTimeProvider.UtcNow,
                _pricingService);
        
            _bookingRespository.Add(booking);
        
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        
            return booking.Id;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }
}