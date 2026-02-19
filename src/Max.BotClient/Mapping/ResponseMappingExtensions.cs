using Max.BotClient.DTOs;
using Max.BotClient.Types;

namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов Response из DTOs в Types.
    /// </summary>
    internal static class ResponseMappingExtensions
    {
        /// <summary>
        /// Преобразует DTO GetMessagesResponse в Types GetMessagesResponse.
        /// </summary>
        public static Types.GetMessagesResponse ToResponse(this DTOs.GetMessagesResponse? dto)
        {
            if (dto == null) return null!;

            return new Types.GetMessagesResponse
            {
                Messages = dto.Messages?.ToMessages()
            };
        }

        /// <summary>
        /// Преобразует DTO SendMessageResponse в Types SendMessageResponse.
        /// </summary>
        public static Types.SendMessageResponse ToResponse(this DTOs.SendMessageResponse? dto)
        {
            if (dto == null) return null!;

            return new Types.SendMessageResponse
            {
                Message = dto.Message?.ToMessage()
            };
        }

        /// <summary>
        /// Преобразует DTO GetUpdatesResponse в Types GetUpdatesResponse.
        /// </summary>
        public static Types.GetUpdatesResponse ToResponse(this DTOs.GetUpdatesResponse? dto)
        {
            if (dto == null) return null!;

            return new Types.GetUpdatesResponse
            {
                Updates = dto.Updates?.ToUpdates() ?? System.Array.Empty<Types.Update>(),
                Marker = dto.Marker
            };
        }

        /// <summary>
        /// Преобразует DTO IUpdate[] в Types Update[].
        /// </summary>
        private static Types.Update[]? ToUpdates(this IUpdate[]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.Update[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                result[i] = dtos[i].ToUpdate();
            }
            return result;
        }

        /// <summary>
        /// Преобразует одно DTO IUpdate в Types Update.
        /// </summary>
        private static Types.Update ToUpdate(this IUpdate dto)
        {
            var result = new Types.Update
            {
                UpdateType = (Types.UpdateType)dto.UpdateType,
                Timestamp = dto.Timestamp
            };

            switch (dto)
            {
                case MessageCreatedUpdate messageCreated:
                    result.Message = messageCreated.Message?.ToMessage();
                    result.UserLocale = messageCreated.UserLocale;
                    break;

                case MessageCallbackUpdate messageCallback:
                    result.Callback = messageCallback.Callback?.ToCallback();
                    result.Message = messageCallback.Message?.ToMessage();
                    result.UserLocale = messageCallback.UserLocale;
                    break;

                case MessageEditedUpdate messageEdited:
                    result.Message = messageEdited.Message?.ToMessage();
                    break;

                case MessageRemovedUpdate messageRemoved:
                    result.MessageId = messageRemoved.MessageId;
                    result.ChatId = messageRemoved.ChatId;
                    result.UserId = messageRemoved.UserId;
                    break;

                case BotAddedUpdate botAdded:
                    result.ChatId = botAdded.ChatId;
                    result.User = botAdded.User?.ToUser();
                    result.IsChannel = botAdded.IsChannel;
                    break;

                case BotRemovedUpdate botRemoved:
                    result.ChatId = botRemoved.ChatId;
                    result.User = botRemoved.User?.ToUser();
                    result.IsChannel = botRemoved.IsChannel;
                    break;

                case DialogMutedUpdate dialogMuted:
                    result.ChatId = dialogMuted.ChatId;
                    result.User = dialogMuted.User?.ToUser();
                    result.MutedUntil = dialogMuted.MutedUntil;
                    result.UserLocale = dialogMuted.UserLocale;
                    break;

                case DialogUnmutedUpdate dialogUnmuted:
                    result.ChatId = dialogUnmuted.ChatId;
                    result.User = dialogUnmuted.User?.ToUser();
                    result.UserLocale = dialogUnmuted.UserLocale;
                    break;

                case DialogClearedUpdate dialogCleared:
                    result.ChatId = dialogCleared.ChatId;
                    result.User = dialogCleared.User?.ToUser();
                    result.UserLocale = dialogCleared.UserLocale;
                    break;

                case DialogRemovedUpdate dialogRemoved:
                    result.ChatId = dialogRemoved.ChatId;
                    result.User = dialogRemoved.User?.ToUser();
                    result.UserLocale = dialogRemoved.UserLocale;
                    break;

                case UserAddedUpdate userAdded:
                    result.ChatId = userAdded.ChatId;
                    result.User = userAdded.User?.ToUser();
                    result.InviterId = userAdded.InviterId;
                    result.IsChannel = userAdded.IsChannel;
                    break;

                case UserRemovedUpdate userRemoved:
                    result.ChatId = userRemoved.ChatId;
                    result.User = userRemoved.User?.ToUser();
                    result.AdminId = userRemoved.AdminId;
                    result.IsChannel = userRemoved.IsChannel;
                    break;

                case BotStartedUpdate botStarted:
                    result.ChatId = botStarted.ChatId;
                    result.User = botStarted.User?.ToUser();
                    result.Payload = botStarted.Payload;
                    result.UserLocale = botStarted.UserLocale;
                    break;

                case BotStoppedUpdate botStopped:
                    result.ChatId = botStopped.ChatId;
                    result.User = botStopped.User?.ToUser();
                    result.UserLocale = botStopped.UserLocale;
                    break;

                case ChatTitleChangedUpdate chatTitleChanged:
                    result.ChatId = chatTitleChanged.ChatId;
                    result.User = chatTitleChanged.User?.ToUser();
                    result.Title = chatTitleChanged.Title;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Преобразует DTO Callback в Types Callback.
        /// </summary>
        private static Types.Callback? ToCallback(this DTOs.Callback? dto)
        {
            if (dto == null) return null;

            return new Types.Callback
            {
                Timestamp = dto.Timestamp,
                CallbackId = dto.CallbackId,
                Payload = dto.Payload,
                User = dto.User?.ToUser()
            };
        }

        /// <summary>
        /// Преобразует DTO UserBotInfo в Types BotInfo.
        /// </summary>
        public static Types.BotInfo ToBotInfo(this DTOs.UserBotInfo? dto)
        {
            if (dto == null) return null!;

            return new Types.BotInfo
            {
                UserId = dto.UserId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                IsBot = dto.IsBot,
                LastActivityTime = dto.LastActivityTime,
                Description = dto.Description,
                AvatarUrl = dto.AvatarUrl,
                FullAvatarUrl = dto.FullAvatarUrl,
                Commands = dto.Commands?.ToBotCommands()
            };
        }

        /// <summary>
        /// Преобразует DTO BotCommandInfo[] в Types BotCommand[].
        /// </summary>
        private static Types.BotCommand[]? ToBotCommands(this DTOs.BotCommandInfo[]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.BotCommand[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                result[i] = new Types.BotCommand
                {
                    Name = dtos[i].Name,
                    Description = dtos[i].Description
                };
            }
            return result;
        }
    }
}
