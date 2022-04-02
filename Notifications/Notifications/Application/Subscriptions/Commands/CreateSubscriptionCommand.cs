
using Catalog.Notifications.Application.Common.Interfaces;
using Catalog.Notifications.Domain.Entities;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Notifications.Application.Subscriptions.Commands;

public record CreateSubscriptionCommand(string UserId, string? SubscriptionGroupId, string? Tag) : IRequest<string>
{
    class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, string>
    {
        private readonly INotificationsContext _context;

        public CreateSubscriptionCommandHandler(INotificationsContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            Subscription? subscription =  await _context.Subscriptions
                                .AsNoTracking()
                                .FirstOrDefaultAsync(sg => sg.UserId == request.UserId && sg.SubscriptionGroupId == request.SubscriptionGroupId , cancellationToken);

            if(subscription is not null) 
            {
                return subscription.Id;
            }

            User? user = null;

            if (request.UserId is not null)
            {
                user = await _context.Users
                                .FirstOrDefaultAsync(sg => sg.Id == request.UserId, cancellationToken);

                if (user is null)
                {
                    throw new Exception("User not found");
                }
            }

            SubscriptionGroup? subscriptionGroup = null;

            if(request.SubscriptionGroupId is not null) 
            {
                subscriptionGroup = await _context.SubscriptionGroups
                    .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionGroupId, cancellationToken);

                if (subscriptionGroup is null)
                {
                    throw new Exception("SubscriptionGroup not found");
                }
            }

            subscription = new Subscription
            {
                Id = Guid.NewGuid().ToString(),
                User = user,
                Tag = request.Tag
            };

            if(subscriptionGroup is not null) 
            {
                subscriptionGroup.Subscriptions.Add(subscription);
            }
            else 
            {
                _context.Subscriptions.Add(subscription);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return subscription.Id;
        }
    }
}

public record AddSubscriptionToGroupCommand(string SubscriptionId, string SubscriptionGroupId) : IRequest
{
    class AddSubscriptionToGroupCommandHandler : IRequestHandler<AddSubscriptionToGroupCommand>
    {
        private readonly INotificationsContext _context;

        public AddSubscriptionToGroupCommandHandler(INotificationsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddSubscriptionToGroupCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _context.Subscriptions
                            .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionId, cancellationToken);

            if (subscription is null)
            {
                throw new Exception("Subscription not found");
            }

            var subscriptionGroup = await _context.SubscriptionGroups
                .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionGroupId, cancellationToken);

            if (subscriptionGroup is null)
            {
                throw new Exception("SubscriptionGroup not found");
            }

            subscriptionGroup.Subscriptions.Add(subscription);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

public record RemoveSubscriptionFromGroupCommand(string SubscriptionId, string SubscriptionGroupId) : IRequest
{
    class RemoveSubscriptionFromGroupCommandHandler : IRequestHandler<RemoveSubscriptionFromGroupCommand>
    {
        private readonly INotificationsContext _context;

        public RemoveSubscriptionFromGroupCommandHandler(INotificationsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveSubscriptionFromGroupCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _context.Subscriptions
                .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionId, cancellationToken);

            if (subscription is null)
            {
                throw new Exception("SubscriptionGroup not found");
            }

            var subscriptionGroup = await _context.SubscriptionGroups
                .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionGroupId, cancellationToken);

            if (subscriptionGroup is null)
            {
                throw new Exception("Subscription not found");
            }

            subscriptionGroup.Subscriptions.Remove(subscription);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

public record CreateSubscriptionGroupCommand(string? Name) : IRequest<string>
{
    class CreateSubscriptionGroupCommandHandler : IRequestHandler<CreateSubscriptionGroupCommand, string>
    {
        private readonly INotificationsContext _context;

        public CreateSubscriptionGroupCommandHandler(INotificationsContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateSubscriptionGroupCommand request, CancellationToken cancellationToken)
        {
            var subscriptionGroup = new SubscriptionGroup
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name
            };

            _context.SubscriptionGroups.Add(subscriptionGroup);

            await _context.SaveChangesAsync(cancellationToken);

            return subscriptionGroup.Id;
        }
    }
}


public record DeleteSubscriptionGroupCommand(string SubscriptionGroupId) : IRequest
{
    class DeleteSubscriptionGroupCommandHandler : IRequestHandler<DeleteSubscriptionGroupCommand>
    {
        private readonly INotificationsContext _context;

        public DeleteSubscriptionGroupCommandHandler(INotificationsContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSubscriptionGroupCommand request, CancellationToken cancellationToken)
        {
            var subscriptionGroup = await _context.SubscriptionGroups
                .FirstOrDefaultAsync(sg => sg.Id == request.SubscriptionGroupId, cancellationToken);

            if (subscriptionGroup is null)
            {
                throw new Exception("SubscriptionGroup not found");
            }

            _context.SubscriptionGroups.Remove(subscriptionGroup);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
