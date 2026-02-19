using Max.BotClient.DTOs;
using Max.BotClient.Types;

namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов Message между DTOs и Types.
    /// </summary>
    internal static class MessageMappingExtensions
    {
        /// <summary>
        /// Преобразует DTO Message в Types Message (выравнивание вложенной структуры).
        /// </summary>
        public static Types.Message? ToMessage(this DTOs.Message? dto)
        {
            if (dto == null) return null;

            return new Types.Message
            {
                // Sender (преобразование в Types.User)
                Sender = dto.Sender?.ToUser(),

                // Recipient (выравнивание)
                RecipientChatId = dto.Recipient?.ChatId,
                RecipientChatType = (Types.ChatType?)dto.Recipient?.ChatType,
                RecipientUserId = dto.Recipient?.UserId,

                // Свойства уровня сообщения
                Timestamp = dto.Timestamp,
                Url = dto.Url,

                // Link (выравнивание LinkedMessage)
                LinkType = (Types.MessageLinkType?)dto.Link?.Type,
                LinkSender = dto.Link?.Sender?.ToUser(),
                LinkChatId = dto.Link?.ChatId,
                LinkMid = dto.Link?.Message?.Mid,
                LinkSeq = dto.Link?.Message?.Seq,
                LinkText = dto.Link?.Message?.Text,
                LinkAttachments = dto.Link?.Message?.Attachments?.ToAttachments(),
                LinkMarkup = dto.Link?.Message?.Markup?.ToMarkupElements(),

                // Body (выравнивание MessageBody)
                Mid = dto.Body?.Mid,
                Seq = dto.Body?.Seq,
                Text = dto.Body?.Text,
                Attachments = dto.Body?.Attachments?.ToAttachments(),
                Markup = dto.Body?.Markup?.ToMarkupElements(),

                // Stat (выравнивание MessageStat)
                Views = dto.Stat?.Views
            };
        }

        /// <summary>
        /// Преобразует массив DTO Messages в Types Messages.
        /// </summary>
        public static Types.Message[]? ToMessages(this DTOs.Message[]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.Message[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                result[i] = dtos[i].ToMessage()!;
            }
            return result;
        }
    }
}
