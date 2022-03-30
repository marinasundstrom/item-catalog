using System.Security.Claims;

namespace Messenger.Contracts;

public record PostMessage(string UserId, string ConversationId, string Text, string? ReplyToId = null);

public record UpdateMessage(string UserId, string ConversationId, string MessageId, string Text);

public record DeleteMessage(string UserId, string ConversationId, string MessageId);

public record MarkMessageAsRead(string UserId, string ConversationId, string MessageId);


public record StartTyping(string UserId, string ConversationId, string MessageId);

public record EndTyping(string UserId, string ConversationId, string MessageId);


public record MessagePosted(MessageDto Message);

public record MessageUpdated(string ConversationId, string MessageId, string Text, DateTime Edited);

public record MessageDeleted(string ConversationId, string MessageId);

public record MessageRead(ReceiptDto Receipt /* string UserId, string ConversationId, string MessageId */);


public record StartedTyping(string UserId, string ConversationId, string MessageId);

public record EndedTyping(string UserId, string ConversationId, string MessageId);