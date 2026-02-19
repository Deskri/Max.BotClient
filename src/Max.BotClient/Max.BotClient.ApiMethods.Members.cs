using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Max.BotClient.Mapping;

namespace Max.BotClient
{
    public static partial class BotClientApiMethods
    {
        /// <summary>
        /// Получить информацию о членстве текущего бота в групповом чате.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/chats/-chatId-/members/me"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Информация о членстве бота в чате.</returns>
        public static async Task<Types.ChatMember> GetMyMembership(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.ChatMember, Types.ChatMember>(
            HttpMethod.Get,
            $"/chats/{chatId}/members/me",
            cancellationToken: cancellationToken
        );

        /// <summary>
        /// Удалить бота из участников группового чата.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/chats/-chatId-/members/me"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        public static async Task<bool> LeaveChat(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<DTOs.ApiResponse>(
            HttpMethod.Delete,
            $"/chats/{chatId}/members/me",
            cancellationToken: cancellationToken
        )).Success;

        /// <summary>
        /// Получение участников группового чата.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/chats/-chatId-/members"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="userIds">Список ID пользователей, чье членство нужно получить. При передаче параметры count и marker игнорируются.</param>
        /// <param name="count">Количество участников (1-100, по умолчанию 20).</param>
        /// <param name="marker">Указатель на следующую страницу данных.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список участников чата и маркер для следующей страницы.</returns>
        public static async Task<Types.GetChatMembersResponse> GetChatMembers(
            this BotClient botClient,
            long chatId,
            long[]? userIds = null,
            int? count = null,
            long? marker = null,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<GetChatMembersParams, DTOs.GetChatMembersResponse, Types.GetChatMembersResponse>(
            HttpMethod.Get,
            $"/chats/{chatId}/members",
            () => new GetChatMembersParams { UserIds = userIds, Count = count, Marker = marker },
            cancellationToken
        );

        /// <summary>
        /// Добавить участников в групповой чат.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/chats/-chatId-/members"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="userIds">Массив ID пользователей для добавления в чат.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        /// <remarks>Для добавления участников могут потребоваться дополнительные права.</remarks>
        public static async Task<bool> AddChatMembers(
            this BotClient botClient,
            long chatId,
            long[] userIds,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<AddChatMembersParams, DTOs.ApiResponse>(
            HttpMethod.Post,
            $"/chats/{chatId}/members",
            () => new AddChatMembersParams { UserIds = userIds },
            cancellationToken
        )).Success;

        /// <summary>
        /// Удалить участника из группового чата.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/chats/-chatId-/members"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="userId">ID пользователя, которого нужно удалить из чата.</param>
        /// <param name="block">Если true, пользователь будет заблокирован в чате. Применяется только для чатов с публичной или приватной ссылкой.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        /// <remarks>Для удаления участников могут потребоваться дополнительные права.</remarks>
        public static async Task<bool> RemoveChatMember(
            this BotClient botClient,
            long chatId,
            long userId,
            bool? block = null,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<RemoveChatMemberParams, DTOs.ApiResponse>(
            HttpMethod.Delete,
            $"/chats/{chatId}/members",
            () => new RemoveChatMemberParams { UserId = userId, Block = block },
            cancellationToken
        )).Success;

        /// <summary>
        /// Получить список администраторов группового чата.
        /// <see href="https://dev.max.ru/docs-api/methods/GET/chats/-chatId-/members/admins"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список администраторов чата с информацией о членстве.</returns>
        /// <remarks>Бот должен быть администратором в запрашиваемом чате.</remarks>
        public static async Task<Types.GetChatMembersResponse> GetChatAdmins(
            this BotClient botClient,
            long chatId,
            CancellationToken cancellationToken = default
        ) => await botClient.ProcessApi<DTOs.GetChatMembersResponse, Types.GetChatMembersResponse>(
            HttpMethod.Get,
            $"/chats/{chatId}/members/admins",
            cancellationToken: cancellationToken
        );

        /// <summary>
        /// Назначить администраторов группового чата.
        /// <see href="https://dev.max.ru/docs-api/methods/POST/chats/-chatId-/members/admins"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="admins">Список пользователей для назначения администраторами (максимум 50).</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если все администраторы успешно добавлены.</returns>
        public static async Task<bool> AddChatAdmins(
            this BotClient botClient,
            long chatId,
            Types.ChatAdmin[] admins,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<DTOs.ApiResponse>(
            HttpMethod.Post,
            $"/chats/{chatId}/members/admins",
            new DTOs.AddChatAdminsRequest { Admins = admins.ToDto() },
            cancellationToken
        )).Success;

        /// <summary>
        /// Отменить права администратора у пользователя в групповом чате.
        /// <see href="https://dev.max.ru/docs-api/methods/DELETE/chats/-chatId-/members/admins/-userId-"/>
        /// </summary>
        /// <param name="botClient">Клиент бота.</param>
        /// <param name="chatId">ID чата.</param>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>true, если запрос был успешным.</returns>
        public static async Task<bool> RemoveChatAdmin(
            this BotClient botClient,
            long chatId,
            long userId,
            CancellationToken cancellationToken = default
        ) => (await botClient.ProcessApi<DTOs.ApiResponse>(
            HttpMethod.Delete,
            $"/chats/{chatId}/members/admins/{userId}",
            cancellationToken: cancellationToken
        )).Success;
    }
}
