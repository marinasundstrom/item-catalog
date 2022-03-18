using System;

using Catalog.Domain.Entities;

namespace Catalog.Application;

static class UserDtoExtensions
{
    public static string? GetDisplayName(this User user) => !String.IsNullOrEmpty(user.DisplayName) ? user?.DisplayName : $"{user.FirstName} {user?.LastName}";
}