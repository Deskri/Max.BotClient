using System;

namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Универсальный маппер для преобразования DTO типов в Types.
    /// </summary>
    internal static class UniversalMappingExtensions
    {
        /// <summary>
        /// Универсальный метод маппинга, преобразующий DTO типы в Types с использованием pattern matching.
        /// Используется в ApiExtensions для маппинга ответов API.
        /// </summary>
        public static TResult ToResult<TDto, TResult>(this TDto dto)
        {
            return (dto, typeof(TResult)) switch
            {
                // Messages
                (DTOs.GetMessagesResponse msgs, _) when typeof(TResult) == typeof(Types.GetMessagesResponse)
                    => (TResult)(object)msgs.ToResponse(),

                (DTOs.SendMessageResponse msg, _) when typeof(TResult) == typeof(Types.SendMessageResponse)
                    => (TResult)(object)msg.ToResponse(),

                (DTOs.Message message, _) when typeof(TResult) == typeof(Types.Message)
                    => (TResult)(object)message.ToMessage()!,

                // Updates
                (DTOs.GetUpdatesResponse upd, _) when typeof(TResult) == typeof(Types.GetUpdatesResponse)
                    => (TResult)(object)upd.ToResponse(),

                // Bot info
                (DTOs.UserBotInfo bot, _) when typeof(TResult) == typeof(Types.BotInfo)
                    => (TResult)(object)bot.ToBotInfo(),

                // Chats
                (DTOs.Chat chat, _) when typeof(TResult) == typeof(Types.Chat)
                    => (TResult)(object)chat.ToChat()!,

                (DTOs.GetChatsResponse chats, _) when typeof(TResult) == typeof(Types.GetChatsResponse)
                    => (TResult)(object)chats.ToGetChatsResponse()!,

                (DTOs.GetPinnedMessageResponse pin, _) when typeof(TResult) == typeof(Types.GetPinnedMessageResponse)
                    => (TResult)(object)pin.ToGetPinnedMessageResponse()!,

                // Members
                (DTOs.ChatMember member, _) when typeof(TResult) == typeof(Types.ChatMember)
                    => (TResult)(object)member.ToChatMember()!,

                (DTOs.GetChatMembersResponse members, _) when typeof(TResult) == typeof(Types.GetChatMembersResponse)
                    => (TResult)(object)members.ToGetChatMembersResponse()!,

                // Video
                (DTOs.VideoInfo video, _) when typeof(TResult) == typeof(Types.VideoInfo)
                    => (TResult)(object)video.ToVideoInfo()!,

                _ => throw new NotSupportedException(
                    $"Маппинг из {typeof(TDto).Name} в {typeof(TResult).Name} не поддерживается. " +
                    "Добавьте новый случай в UniversalMappingExtensions.ToResult() если этот маппинг необходим.")
            };
        }
    }
}
