﻿using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.UnitTests.Apartments
{
    internal static class ApartmentData
    {
        public static Apartment Create(Money price, Money? cleaningFee = null) => new(
            Guid.NewGuid(),
            new Name("Test apartment"),
            new Description("Test description"),
            new Address("Country", "State", "ZipCOde", "City", "Streer"),
            price,
            cleaningFee ?? Money.Zero(),
            []);            
    }
}
