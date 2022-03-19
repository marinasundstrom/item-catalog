using System;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Notifications.Application.Common.Interfaces;

namespace Notifications.Application.Subscriptions.Queries;

public record GetSubscriptionGroupQuery(string? SubscriptionGroupId, string? Tag) : IRequest<SubscriptionGroupDto?>
{
    public class GetSubscriptionGroupQueryHandler : IRequestHandler<GetSubscriptionGroupQuery, SubscriptionGroupDto?>
    {
        private readonly INotificationsContext _context;

        public GetSubscriptionGroupQueryHandler(INotificationsContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionGroupDto?> Handle(GetSubscriptionGroupQuery request, CancellationToken cancellationToken)
        {
            var query = _context.SubscriptionGroups
                .AsNoTracking()
                .AsQueryable();

            if (request.SubscriptionGroupId is not null)
            {
                query = query.Where(sg => sg.Id == request.SubscriptionGroupId);
            }

            if (request.Tag is not null)
            {
                query = query.Where(sg => sg.Name == request.Tag);
            }

            var subscriptionGroup = await query.FirstOrDefaultAsync();

            if(subscriptionGroup is null)
            {
                return null;
            }

            return new SubscriptionGroupDto(subscriptionGroup.Id, subscriptionGroup.Name);
        }
    }
}

