using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Catalog.Application.Common.Interfaces;
using Catalog.Application.Items;
using Catalog.Application.Users;
using Catalog.Application.Messages;

namespace Catalog.Application;

public static class Mapper
{
    public static UserDto ToDto(this Domain.Entities.User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName,  user.Email, user.Created, user.Deleted);
    }

    public static ItemDto ToDto(this Domain.Entities.Item item, IUrlHelper urlHelper)
    {
        return new ItemDto(item.Id, item.Name, item.Description, urlHelper.CreateImageUrl(item.Image), item.CommentCount, item.Created, item.CreatedBy!.ToDto()!, item.LastModified, item.LastModifiedBy?.ToDto());
    }

    public static CommentDto ToDto(this Domain.Entities.Comment comment)
    {
        return new CommentDto(comment.Id, comment.Text, comment.Created, comment.CreatedBy!.ToDto(), comment.LastModified, comment.LastModifiedBy?.ToDto());
    }

    public static Messages.MessageDto ToDto(this Domain.Entities.Message message)
    {
        return new Messages.MessageDto(message.Id, message.Text, message.ReplyTo?.ToDto(), message.Receipts?.Select(r => r.ToDto()), message.Replies?.Select(r => r.ToDto()), message.Created, message.CreatedBy!.ToDto(), message.LastModified, message.LastModifiedBy?.ToDto(), message.Deleted, message.DeletedBy?.ToDto());
    }

    public static Messages.ReceiptDto ToDto(this Domain.Entities.MessageReceipt receipt)
    {
        return new Messages.ReceiptDto(receipt.Id, receipt.Message.Id, receipt.CreatedBy!.ToDto(), receipt.Created);
    }
}