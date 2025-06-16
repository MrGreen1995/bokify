using Bookify.Domain.Users;

namespace Bookify.Application.UnitTests.Users
{
    internal static class UserData
    {
        public static User Create() => User.Create(FirstName, LastName, Email);

        public static readonly FirstName FirstName= new("FirstName");
        public static readonly LastName LastName= new("LastName");
        public static readonly Email Email= new("Email");
    }
}
