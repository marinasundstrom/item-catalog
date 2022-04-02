﻿using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Contracts;
using Catalog.IdentityService.Domain.Entities;
using Catalog.IdentityService.Domain.Exceptions;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Catalog.IdentityService.Application.Users.Commands;

public record DeleteUserCommand(string UserId) : IRequest
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventPublisher _eventPublisher;

        public DeleteUserCommandHandler(UserManager<User> userManager, ICurrentUserService currentUserService, IEventPublisher eventPublisher)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _eventPublisher = eventPublisher;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            await _userManager.DeleteAsync(user);

            await _eventPublisher.PublishEvent(new UserDeleted(user.Id, _currentUserService.UserId));

            return Unit.Value;
        }
    }
}