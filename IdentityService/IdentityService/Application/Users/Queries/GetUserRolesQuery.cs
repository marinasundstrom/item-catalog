﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Application.Common.Models;
using Catalog.IdentityService.Domain.Entities;

namespace Catalog.IdentityService.Application.Users.Queries;

public record GetUserRolesQuery(
    string UserId, int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, IdentityService.Application.Common.Models.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<RoleDto>>
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, ItemsResult<RoleDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetUserRolesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<RoleDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                  .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            var query = _context.Roles
                .Where(x => x.Users.Any(x => x.Id == request.UserId))
                //.OrderBy(p => p.Created)
                .Skip(request.PageSize * request.Page)
                .Take(request.PageSize)
                .AsNoTracking()
                .AsSplitQuery();

            if (request.SearchString is not null)
            {
                query = query.Where(p =>
                    p.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalItems = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection == IdentityService.Application.Common.Models.SortDirection.Desc ? IdentityService.SortDirection.Descending : IdentityService.SortDirection.Ascending);
            }

            var roles = await query
                .ToListAsync(cancellationToken);

            var dtos = roles.Select(role => new RoleDto(role.Id, role.Name));

            return new ItemsResult<RoleDto>(dtos, totalItems);
        }
    }
}