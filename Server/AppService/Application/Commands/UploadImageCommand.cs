
using MediatR;
using Catalog.Infrastructure;
using Catalog.Application.Common.Interfaces;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Commands;

public class UploadImageCommand : IRequest<UploadImageResult>
{
    public string Id { get; set; }

    public Stream Stream { get; set; }

    public UploadImageCommand(string id, Stream stream)
    {
        Id = id;
        Stream = stream;
    }

    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, UploadImageResult>
    {
        private readonly ICatalogContext context;
        private readonly IUrlHelper urlHelper;
        private readonly IFileUploaderService _fileUploaderService;
        private readonly IItemsClient client;

        public UploadImageCommandHandler(ICatalogContext context, IUrlHelper urlHelper, IFileUploaderService fileUploaderService, IItemsClient client)
        {
            this.context = context;
            this.urlHelper = urlHelper;
            this._fileUploaderService = fileUploaderService;
            this.client = client;
        }

        public async Task<UploadImageResult> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var item = await context.Items.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null)
            {
                return UploadImageResult.Successful;
            }

            string imageId = $"image-{request.Id}";

            await _fileUploaderService.UploadFileAsync(imageId, request.Stream, cancellationToken);

            item.Image = imageId;
            await context.SaveChangesAsync();

            await client.ImageUploaded(item.Id, urlHelper.CreateImageUrl(item.Image)!);

            return UploadImageResult.Successful;
        }
    }
}
