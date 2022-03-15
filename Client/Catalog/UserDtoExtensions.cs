using System;

using Catalog.IdentityService.Client;

namespace Catalog;

public static class UserDtoExtensions
{
    public static string? GetDisplayName(this Catalog.Client.UserDto user) => !String.IsNullOrEmpty(user.DisplayName) ? user?.DisplayName : $"{user.FirstName} {user?.LastName}";

    public static string? GetDisplayName(this UserDto user) => !String.IsNullOrEmpty(user.DisplayName) ? user?.DisplayName : $"{user.FirstName} {user?.LastName}";
}