namespace Max.BotClient
{
    internal class GetChatsParams
    {
        [QueryParam] public int? Count { get; set; }
        [QueryParam] public long? Marker { get; set; }
    }

    internal class SendActionParams
    {
        [BodyParam] public Types.SenderAction? Action { get; set; }
    }

    internal class PinMessageParams
    {
        [BodyParam] public string? MessageId { get; set; }
        [BodyParam] public bool? Notify { get; set; }
    }

    internal class GetMessagesByChatParams
    {
        [QueryParam] public long? ChatId { get; set; }
        [QueryParam] public long? From { get; set; }
        [QueryParam] public long? To { get; set; }
        [QueryParam] public int? Count { get; set; }
    }

    internal class GetMessagesByIdsParams
    {
        [QueryParam] public string[]? MessageIds { get; set; }
    }

    internal class SendMessageParams
    {
        [QueryParam] public long? ChatId { get; set; }
        [QueryParam] public long? UserId { get; set; }
        [QueryParam] public bool? DisableLinkPreview { get; set; }
        [Body] public DTOs.NewMessageBody? Body { get; set; }
    }

    internal class EditMessageParams
    {
        [QueryParam] public string? MessageId { get; set; }
        [Body] public DTOs.NewMessageBody? Body { get; set; }
    }

    internal class DeleteMessageParams
    {
        [QueryParam] public string? MessageId { get; set; }
    }

    internal class AnswerCallbackParams
    {
        [QueryParam] public string? CallbackId { get; set; }
        [BodyParam] public DTOs.NewMessageBody? Message { get; set; }
        [BodyParam] public string? Notification { get; set; }
    }

    internal class GetChatMembersParams
    {
        [QueryParam] public long[]? UserIds { get; set; }
        [QueryParam] public int? Count { get; set; }
        [QueryParam] public long? Marker { get; set; }
    }

    internal class AddChatMembersParams
    {
        [BodyParam] public long[]? UserIds { get; set; }
    }

    internal class RemoveChatMemberParams
    {
        [QueryParam] public long? UserId { get; set; }
        [QueryParam] public bool? Block { get; set; }
    }

    internal class UnsubscribeParams
    {
        [QueryParam] public string? Url { get; set; }
    }

    internal class GetUpdatesParams
    {
        [QueryParam] public int? Limit { get; set; }
        [QueryParam] public int? Timeout { get; set; }
        [QueryParam] public long? Marker { get; set; }
        [QueryParam("types")] public Types.UpdateType[]? UpdateTypes { get; set; }
    }

    internal class GetUploadUrlParams
    {
        [QueryParam] public Types.UploadType? Type { get; set; }
    }
}
