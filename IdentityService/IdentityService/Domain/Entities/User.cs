// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Catalog.IdentityService.Domain.Common.Interfaces;

using Microsoft.AspNetCore.Identity;

namespace Catalog.IdentityService.Domain.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class User : IdentityUser, IAuditableEntity, ISoftDelete
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? DisplayName { get; set; }
    public List<Role> Roles { get; } = new List<Role>();
    public List<UserRole> UserRoles { get; } = new List<UserRole>();

    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    public DateTime? Deleted { get; set; }
    public string DeletedBy { get; set; }
}

public class Role : IdentityRole<string>
{
    public Role()
    {
        Id = Guid.NewGuid().ToString();
    }

    public List<User> Users { get; } = new List<User>();

    public List<UserRole> UserRoles { get; } = new List<UserRole>();
}

public class UserRole : IdentityUserRole<string>
{
    public User User { get; set; }

    public Role Role { get; set; }
}