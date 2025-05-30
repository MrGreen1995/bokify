using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """"
                           SELECT 
                               id as Id,
                               apartment_id as ApartmentId,
                               user_id as UserId,
                               status as Status,
                               price_for_period_amount as PriceAmount,
                               price_for_period_currency as PriceCurrency,
                               cleaning_fee_amount as CleaningFeeAmount,
                               cleaning_fee_currency as CleaningFeeCurrency,
                               amenities_up_charge_amount as AmenitiesUpChargeAmount,
                               amenities_up_charge_currency as AmenitiesUpChargeCurrency,
                               total_price_amount as TotalPriceAmount,
                               total_price_currency as TotalPriceCurrency,
                               duration_start as DurationStart,
                               duration_end as DurationEnd,
                               created_on_utc as CreatedOnUtc
                           FROM bookings 
                           WHERE BookingId = @BookingId 
                           """";
        
        var booking = await connection.QueryFirstOrDefaultAsync<Booking>(sql, new
        {
            request.BookingId
        });
        
        throw new NotImplementedException();
    }
}