
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Items.Commands;

public record UpdateCommentCommand(string CommentId, string Text) : IRequest
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
    {
        private readonly ICatalogContext context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCommentCommandHandler(ICatalogContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await context.Comments.FirstOrDefaultAsync(i => i.Id == request.CommentId, cancellationToken);

            if (comment is null) throw new Exception();

            if (!IsAuthorizedToEdit(comment))
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            comment.Text = request.Text;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool IsAuthorizedToEdit(Domain.Entities.Comment comment) => _currentUserService.IsCurrentUser(comment.CreatedById!) || _currentUserService.IsUserInRole(Roles.Administrator);
    }
}
