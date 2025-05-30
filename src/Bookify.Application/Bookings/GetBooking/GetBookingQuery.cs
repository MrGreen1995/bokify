using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;