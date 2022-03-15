﻿using MediatR;

using Microsoft.AspNetCore.Identity;

using Catalog.IdentityService.Domain.Entities;

namespace Catalog.IdentityService.Application.Users.Commands;

public class UpdateUserPasswordCommand : IRequest
{
    public UpdateUserPasswordCommand(string userId, string currentPassword, string newPassword)
    {
        UserId = userId;
        CurrentPassword = currentPassword;
        NewPassword = newPassword;
    }

    public string UserId { get; }
    public string CurrentPassword { get; private set; }

    public string NewPassword { get; }

    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand>
    {
        private readonly UserManager<User> _userManager;

        public UpdateUserPasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            return Unit.Value;
        }
    }
}