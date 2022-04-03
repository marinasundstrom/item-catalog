
using Catalog.ApiKeys.Application.Common.Interfaces;
using Catalog.ApiKeys.Application.Common.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.ApiKeys.Application.Commands;

public record CheckApiKeyCommand(string ApiKey, string[] RequestedResources) : IRequest<ApiKeyResult>
{
    public class CheckApiKeyCommandHandler : IRequestHandler<CheckApiKeyCommand, ApiKeyResult>
    {
        private readonly IApiKeysContext context;

        public CheckApiKeyCommandHandler(IApiKeysContext context)
        {
            this.context = context;
        }

        public async Task<ApiKeyResult> Handle(CheckApiKeyCommand request, CancellationToken cancellationToken)
        {
            var apiKey = await context.ApiKeys.FirstOrDefaultAsync(apiKey => apiKey.Key == request.ApiKey);

            return apiKey is null ? new ApiKeyResult(ApiKeyStatus.Unauthorized) : new ApiKeyResult(ApiKeyStatus.Authorized);
        }
    }
}

public record ApiKeyResult(ApiKeyStatus Status);

public enum ApiKeyStatus
{
    Unauthorized,
    Authorized
}