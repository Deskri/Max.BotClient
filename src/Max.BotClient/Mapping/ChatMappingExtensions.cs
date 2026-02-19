using System.Linq;

namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов Chat между DTOs и Types.
    /// </summary>
    internal static class ChatMappingExtensions
    {
        /// <summary>
        /// Преобразует DTO Chat в Types Chat.
        /// </summary>
        public static Types.Chat? ToChat(this DTOs.Chat? dto)
        {
            if (dto == null) return null;

            return new Types.Chat
            {
                ChatId = dto.ChatId,
                Type = (Types.ChatType)dto.Type,
                Status = (Types.ChatStatus)dto.Status,
                Title = dto.Title,
                Icon = dto.Icon?.ToImage(),
                LastEventTime = dto.LastEventTime,
                ParticipantsCount = dto.ParticipantsCount,
                OwnerId = dto.OwnerId,
                Participants = dto.Participants,
                IsPublic = dto.IsPublic,
                Link = dto.Link,
                Description = dto.Description,
                DialogWithUser = dto.DialogWithUser?.ToUserWithPhoto(),
                ChatMessageId = dto.ChatMessageId,
                PinnedMessage = dto.PinnedMessage?.ToMessage()
            };
        }

        /// <summary>
        /// Преобразует массив DTO Chats в Types Chats.
        /// </summary>
        public static Types.Chat[]? ToChats(this DTOs.Chat[]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.Chat[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                result[i] = dtos[i].ToChat()!;
            }
            return result;
        }

        /// <summary>
        /// Преобразует DTO Image в Types Image.
        /// </summary>
        public static Types.Image? ToImage(this DTOs.Image? dto)
        {
            if (dto == null) return null;

            return new Types.Image
            {
                Url = dto.Url
            };
        }

        /// <summary>
        /// Преобразует DTO GetChatsResponse в Types GetChatsResponse.
        /// </summary>
        public static Types.GetChatsResponse? ToGetChatsResponse(this DTOs.GetChatsResponse? dto)
        {
            if (dto == null) return null;

            return new Types.GetChatsResponse
            {
                Chats = dto.Chats?.ToChats(),
                Marker = dto.Marker
            };
        }

        /// <summary>
        /// Преобразует DTO GetPinnedMessageResponse в Types GetPinnedMessageResponse.
        /// </summary>
        public static Types.GetPinnedMessageResponse? ToGetPinnedMessageResponse(this DTOs.GetPinnedMessageResponse? dto)
        {
            if (dto == null) return null;

            return new Types.GetPinnedMessageResponse
            {
                Message = dto.Message?.ToMessage()
            };
        }

        /// <summary>
        /// Преобразует DTO ChatMember в Types ChatMember.
        /// </summary>
        public static Types.ChatMember? ToChatMember(this DTOs.ChatMember? dto)
        {
            if (dto == null) return null;

            return new Types.ChatMember
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
                LastAccessTime = dto.LastAccessTime,
                IsOwner = dto.IsOwner,
                IsAdmin = dto.IsAdmin,
                JoinTime = dto.JoinTime,
                Permissions = dto.Permissions?.Select(p => (Types.ChatAdminPermission)p).ToArray(),
                Alias = dto.Alias
            };
        }

        /// <summary>
        /// Преобразует массив DTO ChatMember в Types ChatMember.
        /// </summary>
        public static Types.ChatMember[]? ToChatMembers(this DTOs.ChatMember[]? dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(d => d.ToChatMember()!).ToArray();
        }

        /// <summary>
        /// Преобразует DTO GetChatMembersResponse в Types GetChatMembersResponse.
        /// </summary>
        public static Types.GetChatMembersResponse? ToGetChatMembersResponse(this DTOs.GetChatMembersResponse? dto)
        {
            if (dto == null) return null;

            return new Types.GetChatMembersResponse
            {
                Members = dto.Members?.ToChatMembers(),
                Marker = dto.Marker
            };
        }

        /// <summary>
        /// Преобразует Types ChatAdmin в DTO ChatAdmin.
        /// </summary>
        public static DTOs.ChatAdmin ToDto(this Types.ChatAdmin admin)
        {
            return new DTOs.ChatAdmin
            {
                UserId = admin.UserId,
                Permissions = admin.Permissions?.Select(p => (DTOs.ChatAdminPermission)p).ToArray(),
                Alias = admin.Alias
            };
        }

        /// <summary>
        /// Преобразует массив Types ChatAdmin в DTO.
        /// </summary>
        public static DTOs.ChatAdmin[] ToDto(this Types.ChatAdmin[] admins)
        {
            return admins.Select(a => a.ToDto()).ToArray();
        }
    }
}
