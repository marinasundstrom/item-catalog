
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Items.Commands;

public record DeleteCommentCommand(string CommentId) : IRequest
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
    {
        private readonly ICatalogContext context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCommentCommandHandler(ICatalogContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await context.Comments
                .Include(x => x.Item)
                .FirstOrDefaultAsync(i => i.Id == request.CommentId, cancellationToken);

            if (comment is null) throw new Exception();

            if (!IsAuthorizedToDelete(comment)) 
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            context.Comments.Remove(comment);

            comment.Item.CommentCount--;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool IsAuthorizedToDelete(Domain.Entities.Comment comment) => _currentUserService.IsCurrentUser(comment.CreatedById!) || _currentUserService.IsUserInRole(Roles.Administrator);
    }
}