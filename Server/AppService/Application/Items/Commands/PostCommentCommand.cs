
using Catalog.Application.Common.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Items.Commands;

public record PostCommentCommand(string ItemId, string Text) : IRequest<string>
{
    public class PostCommentCommandHandler : IRequestHandler<PostCommentCommand, string>
    {
        private readonly ICatalogContext context;

        public PostCommentCommandHandler(ICatalogContext context)
        {
            this.context = context;
        }

        public async Task<string> Handle(PostCommentCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);

            if (item is null) throw new Exception();

            var comment = item.AddComment(request.Text);

            await context.SaveChangesAsync(cancellationToken);

            return comment.Id;
        }
    }
}
