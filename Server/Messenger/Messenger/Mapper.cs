using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Messenger.Application.Common.Interfaces;
using Messenger.Application.Users;
using Messenger.Application.Messages;
using Messenger.Contracts;

namespace Messenger.Application;

public static class Mapper
{
    public static UserDto ToDto(this Domain.Entities.User user)
    {
        return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName,  user.Email, user.Created, user.Deleted);
    }

    public static Contracts.MessageDto ToDto(this Domain.Entities.Message message)
    {
        return new Contracts.MessageDto(message.Id, message.Text, message.ReplyTo?.ToDto(), message.Receipts?.Select(r => r.ToDto()), message.Replies?.Select(r => r.ToDto()), message.Created, message.CreatedBy!.ToDto(), message.LastModified, message.LastModifiedBy?.ToDto(), message.Deleted, message.DeletedBy?.ToDto());
    }

    public static Contracts.ReceiptDto ToDto(this Domain.Entities.MessageReceipt receipt)
    {
        return new Contracts.ReceiptDto(receipt.Id, receipt.Message.Id, receipt.CreatedBy!.ToDto(), receipt.Created);
    }
}