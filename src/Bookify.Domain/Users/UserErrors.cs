﻿using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users;

public static class UserErrors
{
    public static Error NotFound = new("User.Found", "The user was not found");
    
    public static Error InvalidCredentials = new("User.InvalidCredentials", "Incorrect login or password");
}