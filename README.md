# Max.BotClient

<div align="center">

[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.0-blue?style=flat-square)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![License: MIT](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE)

**Неофициальный .NET SDK для [MAX Bot API](https://dev.max.ru/docs-api)**
&nbsp;·&nbsp;
**Unofficial .NET SDK for [MAX Bot API](https://dev.max.ru/docs-api)**

</div>

---

> **[Русский](#русский) · [English](#english)**

---

# Русский

## Содержание

- [О проекте](#о-проекте)
- [Быстрый старт](#быстрый-старт)
- [Конфигурация](#конфигурация)
- [Получение обновлений](#получение-обновлений)
- [Справочник API методов](#справочник-api-методов)
  - [Сообщения](#сообщения)
  - [Чаты](#чаты)
  - [Участники](#участники)
  - [Подписки](#подписки)
  - [Прочее](#прочее)
- [Построитель сообщений](#построитель-сообщений)
- [Обработка ошибок](#обработка-ошибок)

---

## О проекте

`Max.BotClient` — библиотека для разработки ботов на платформе [MAX](https://max.ru).
Написана на **.NET Standard 2.0**, совместима с .NET Framework 4.6.1+ и .NET Core 2.0+.

**Ключевые возможности:**

- Все методы MAX Bot API с полной типизацией
- Единый класс `Message` для создания, чтения и редактирования сообщений (builder pattern)
- Long Polling из коробки с автоматической пагинацией через маркеры
- Поддержка Webhook-подписок
- Автоматические повторные запросы при ошибках 429/5xx (exponential backoff)
- Поддержка вложений: фото, видео, аудио, файлы, стикеры, контакты, геолокация
- Inline-клавиатуры с fluent-builder

---

## Быстрый старт

```csharp
using Max.BotClient;
using Max.BotClient.Types;

// Создание клиента
var bot = new BotClient("ВАШ_ТОКЕН");

// Получение информации о боте
var me = await bot.GetMe();
Console.WriteLine($"Бот: @{me.Username}");

// Отправка текстового сообщения пользователю
await bot.SendMessage(userId, new Message("Привет!"));

// Отправка сообщения в чат
await bot.SendMessage(chatId, new Message("Привет, чат!").ToChat());
```

---

## Конфигурация

```csharp
var options = new BotClientOptions("ВАШ_ТОКЕН")
{
    RetryCount = 3,          // Максимум повторных попыток при 429/5xx (по умолчанию: 3)
    RetryDelaySeconds = 1    // Начальная задержка в секундах для exponential backoff (по умолчанию: 1)
                             // Фактические задержки: 1s, 2s, 4s...
};

var bot = new BotClient(options);

// Можно передать собственный HttpClient
var httpClient = new HttpClient();
var bot2 = new BotClient(options, httpClient);
```

---

## Получение обновлений

### Long Polling (рекомендуется для большинства случаев)

Метод `StartReceiving` запускает опрос в **фоновом потоке** и не блокирует выполнение:

```csharp
var cts = new CancellationTokenSource();

bot.StartReceiving(
    updateHandler: async (bot, update, ct) =>
    {
        Console.WriteLine($"Тип обновления: {update.UpdateType}");

        if (update.Message != null)
        {
            var msg = update.Message;
            Console.WriteLine($"Сообщение от {msg.Sender?.Name}: {msg.Text}");

            // Ответить в тот же чат/диалог
            await bot.SendMessage(
                msg.RecipientId ?? 0,
                new Message("Получил твоё сообщение!").ToChat()
            );
        }
    },
    errorHandler: async (bot, ex, ct) =>
    {
        Console.WriteLine($"Ошибка: {ex.Message}");
    },
    options: new ReceiverOptions
    {
        Timeout = 30,              // Таймаут long polling в секундах (0–90)
        Limit = 100,               // Обновлений за запрос (1–1000)
        DropPendingUpdates = true, // Пропустить накопившиеся обновления при старте
        AllowedUpdates = new[]     // Фильтр типов обновлений (null = все)
        {
            UpdateType.MessageCreated,
            UpdateType.MessageCallback
        }
    },
    cancellationToken: cts.Token
);

Console.WriteLine("Бот запущен. Нажмите Ctrl+C для остановки.");
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
await Task.Delay(Timeout.Infinite, cts.Token).ContinueWith(_ => { });
```

Метод `ReceiveAsync` **блокирует** текущий поток до отмены:

```csharp
await bot.ReceiveAsync(updateHandler, errorHandler, options, cts.Token);
```

### Webhook

```csharp
// Подписаться на вебхук
var result = await bot.Subscribe(
    url: "https://your-domain.com/webhook",
    updateTypes: new[] { UpdateType.MessageCreated, UpdateType.MessageCallback },
    secret: "your-secret-key"  // Для заголовка X-Max-Bot-Api-Secret
);

// Получить список активных подписок
var subscriptions = await bot.GetSubscriptions();

// Отписаться
await bot.Unsubscribe("https://your-domain.com/webhook");
```

---

## Справочник API методов

### Сообщения

| Метод | HTTP | Описание |
|-------|------|----------|
| `GetMessages(chatId, from?, to?, count?)` | GET | Получить сообщения из чата (по времени) |
| `GetMessages(messageIds[])` | GET | Получить сообщения по их ID |
| `GetMessage(messageId)` | GET | Получить одно сообщение по ID |
| `SendMessage(id, message, disableLinkPreview?)` | POST | Отправить сообщение |
| `EditMessage(messageId, message)` | PUT | Редактировать сообщение (до 24 часов) |
| `DeleteMessage(messageId)` | DELETE | Удалить сообщение (до 24 часов) |
| `AnswerCallback(callbackId, message?, notification?)` | POST | Ответить на нажатие кнопки |

```csharp
// Получить последние 10 сообщений из чата
var messages = await bot.GetMessages(chatId, count: 10);

// Отправить сообщение с отключённым превью ссылок
await bot.SendMessage(userId, new Message("https://example.com"), disableLinkPreview: true);

// Редактировать сообщение
await bot.EditMessage(message.Mid, new Message("Исправленный текст"));

// Удалить сообщение
await bot.DeleteMessage(message.Mid);

// Ответить на нажатие кнопки уведомлением
await bot.AnswerCallback(callbackId, notification: "Кнопка нажата!");

// Ответить на нажатие кнопки с обновлением сообщения
await bot.AnswerCallback(callbackId, message: new Message("Новый текст сообщения"));
```

---

### Чаты

| Метод | HTTP | Описание |
|-------|------|----------|
| `GetChats(count?, marker?)` | GET | Список чатов бота (с пагинацией) |
| `GetChat(chatId)` | GET | Информация о чате |
| `UpdateChat(chatId, request)` | PATCH | Изменить название/иконку чата |
| `DeleteChat(chatId)` | DELETE | Удалить чат для всех участников |
| `SendAction(chatId, action)` | POST | Отправить действие (typing, sending_photo...) |
| `GetPinnedMessage(chatId)` | GET | Получить закреплённое сообщение |
| `PinMessage(chatId, messageId, notify?)` | PUT | Закрепить сообщение |
| `UnpinMessage(chatId)` | DELETE | Открепить сообщение |

```csharp
// Получить все чаты (с пагинацией)
long? marker = null;
do
{
    var response = await bot.GetChats(count: 50, marker: marker);
    foreach (var chat in response.Chats)
        Console.WriteLine($"{chat.Title} ({chat.ChatId})");
    marker = response.Marker;
} while (marker != null);

// Показать "бот печатает..."
await bot.SendAction(chatId, SenderAction.TypingOn);

// Закрепить сообщение без уведомления
await bot.PinMessage(chatId, messageId, notify: false);

// Изменить название чата
await bot.UpdateChat(chatId, new UpdateChatRequest { Title = "Новое название" });
```

---

### Участники

| Метод | HTTP | Описание |
|-------|------|----------|
| `GetMyMembership(chatId)` | GET | Членство бота в чате |
| `LeaveChat(chatId)` | DELETE | Покинуть чат |
| `GetChatMembers(chatId, userIds?, count?, marker?)` | GET | Список участников |
| `AddChatMembers(chatId, userIds[])` | POST | Добавить участников |
| `RemoveChatMember(chatId, userId, block?)` | DELETE | Удалить участника |
| `GetChatAdmins(chatId)` | GET | Список администраторов |
| `AddChatAdmins(chatId, admins[])` | POST | Назначить администраторов |
| `RemoveChatAdmin(chatId, userId)` | DELETE | Снять права администратора |

```csharp
// Получить участников чата
var members = await bot.GetChatMembers(chatId, count: 100);
foreach (var member in members.Members)
    Console.WriteLine($"{member.User?.Name} — {member.Role}");

// Добавить пользователей в чат
await bot.AddChatMembers(chatId, new[] { userId1, userId2 });

// Удалить и заблокировать участника
await bot.RemoveChatMember(chatId, userId, block: true);

// Назначить администратора с правами
await bot.AddChatAdmins(chatId, new[]
{
    new ChatAdmin { UserId = userId, Permissions = AdminPermissions.ReadMessages | AdminPermissions.AddMembers }
});
```

---

### Подписки

| Метод | HTTP | Описание |
|-------|------|----------|
| `GetSubscriptions()` | GET | Список вебхук-подписок |
| `Subscribe(url, updateTypes?, secret?)` | POST | Подписаться на вебхук |
| `Unsubscribe(url)` | DELETE | Отписаться от вебхука |
| `GetUpdates(limit?, timeout?, marker?, types?)` | GET | Получить обновления (low-level) |

---

### Прочее

| Метод | HTTP | Описание |
|-------|------|----------|
| `GetMe()` | GET | Информация о боте |
| `GetUploadUrl(type)` | POST | Получить URL для загрузки файла |
| `GetVideo(videoToken)` | GET | Информация о видео |

```csharp
// Получить URL для загрузки фото
var upload = await bot.GetUploadUrl(UploadType.Photo);
// Загрузите файл по upload.Url, затем используйте токен в сообщении

// Получить информацию о видео
var video = await bot.GetVideo(videoAttachment.Token);
Console.WriteLine($"Длительность: {video.Duration} сек, {video.Width}x{video.Height}");
```

---

## Построитель сообщений

Класс `Message` используется одновременно для создания, получения и редактирования сообщений:

```csharp
// Текст с форматированием
var msg = new Message("**Жирный** и _курсив_");

// Фото по URL
var msg = new Message("Подпись")
    .WithPhoto("https://example.com/photo.jpg");

// Несколько фото
var msg = new Message()
    .WithPhoto("https://example.com/photo1.jpg")
    .AddPhoto("https://example.com/photo2.jpg");

// Фото по токену (уже загруженное)
var msg = new Message().WithPhoto(PhotoAttachment.FromToken("token123"));

// Видео
var msg = new Message("Видео").WithVideo("video_token");

// Видео + фото в одном сообщении
var msg = new Message("Медиа")
    .WithVideo("video_token")
    .WithPhoto("https://example.com/photo.jpg");

// Inline-клавиатура
var msg = new Message("Выберите действие:")
    .WithKeyboard(kb => kb
        .AddRow()
            .AddCallbackButton("Кнопка 1", "payload_1")
            .AddCallbackButton("Кнопка 2", "payload_2")
        .AddRow()
            .AddLinkButton("Сайт", "https://example.com")
    );

// Отправить в чат (по умолчанию — пользователю)
var msg = new Message("Привет!").ToChat();

// Ссылка на другое сообщение (reply/forward)
var msg = new Message("Ответ")
    .WithLink(MessageLinkRequestType.Reply, messageId, chatId);
```

### Чтение полученных сообщений

```csharp
bot.StartReceiving(async (bot, update, ct) =>
{
    var msg = update.Message;
    if (msg == null) return;

    Console.WriteLine($"Текст: {msg.Text}");
    Console.WriteLine($"От: {msg.Sender?.Name} ({msg.Sender?.UserId})");
    Console.WriteLine($"ID: {msg.Mid}");
    Console.WriteLine($"Время: {DateTimeOffset.FromUnixTimeMilliseconds(msg.Timestamp)}");

    // Получить вложения
    var photos = msg.GetPhotos();       // PhotoAttachment[]
    var videos = msg.GetVideos();       // VideoAttachment[]
    var files  = msg.GetFiles();        // FileAttachment[]
    var audio  = msg.GetAudios();       // AudioAttachment[]

    // Проверить наличие вложений
    if (msg.HasAttachments())
        Console.WriteLine($"Вложений: {msg.GetAllAttachments().Length}");

    // Inline-клавиатура
    if (update.Callback != null)
    {
        Console.WriteLine($"Payload кнопки: {update.Callback.Payload}");
        await bot.AnswerCallback(update.Callback.CallbackId, notification: "OK");
    }
});
```

---

## Обработка ошибок

```csharp
try
{
    await bot.SendMessage(userId, new Message("Текст"));
}
catch (MaxBotClientApiException ex)
{
    Console.WriteLine($"Код ошибки: {(int)ex.StatusCode}");
    Console.WriteLine($"Тело ответа: {ex.ResponseBody}");
    Console.WriteLine($"Повторяема: {ex.IsRetryable}"); // true для 429/5xx
}
catch (OperationCanceledException)
{
    Console.WriteLine("Запрос отменён");
}
```

---

---

# English

## Table of Contents

- [About](#about)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Receiving Updates](#receiving-updates)
- [API Methods Reference](#api-methods-reference)
  - [Messages](#messages)
  - [Chats](#chats)
  - [Members](#members)
  - [Subscriptions](#subscriptions)
  - [Miscellaneous](#miscellaneous)
- [Message Builder](#message-builder)
- [Error Handling](#error-handling)

---

## About

`Max.BotClient` is a library for building bots on the [MAX](https://max.ru) platform.
Targets **.NET Standard 2.0** — compatible with .NET Framework 4.6.1+ and .NET Core 2.0+.

**Key features:**

- Full coverage of MAX Bot API with strong typing
- Single unified `Message` class for creating, reading, and editing messages (builder pattern)
- Built-in Long Polling with automatic marker-based pagination
- Webhook subscription support
- Automatic retries on 429/5xx errors (exponential backoff)
- Attachment support: photos, videos, audio, files, stickers, contacts, locations
- Inline keyboards with fluent builder

---

## Quick Start

```csharp
using Max.BotClient;
using Max.BotClient.Types;

// Create the client
var bot = new BotClient("YOUR_TOKEN");

// Get bot info
var me = await bot.GetMe();
Console.WriteLine($"Bot: @{me.Username}");

// Send a text message to a user
await bot.SendMessage(userId, new Message("Hello!"));

// Send a message to a chat
await bot.SendMessage(chatId, new Message("Hello, chat!").ToChat());
```

---

## Configuration

```csharp
var options = new BotClientOptions("YOUR_TOKEN")
{
    RetryCount = 3,          // Max retries on 429/5xx (default: 3)
    RetryDelaySeconds = 1    // Initial delay in seconds for exponential backoff (default: 1)
                             // Actual delays: 1s, 2s, 4s...
};

var bot = new BotClient(options);

// You can also inject a custom HttpClient
var httpClient = new HttpClient();
var bot2 = new BotClient(options, httpClient);
```

---

## Receiving Updates

### Long Polling (recommended for most use cases)

`StartReceiving` starts polling in a **background task** and does not block:

```csharp
var cts = new CancellationTokenSource();

bot.StartReceiving(
    updateHandler: async (bot, update, ct) =>
    {
        Console.WriteLine($"Update type: {update.UpdateType}");

        if (update.Message != null)
        {
            var msg = update.Message;
            Console.WriteLine($"Message from {msg.Sender?.Name}: {msg.Text}");

            // Reply to the same chat/dialog
            await bot.SendMessage(
                msg.RecipientId ?? 0,
                new Message("Got your message!").ToChat()
            );
        }
    },
    errorHandler: async (bot, ex, ct) =>
    {
        Console.WriteLine($"Error: {ex.Message}");
    },
    options: new ReceiverOptions
    {
        Timeout = 30,              // Long polling timeout in seconds (0–90)
        Limit = 100,               // Updates per request (1–1000)
        DropPendingUpdates = true, // Skip accumulated updates on startup
        AllowedUpdates = new[]     // Filter update types (null = all)
        {
            UpdateType.MessageCreated,
            UpdateType.MessageCallback
        }
    },
    cancellationToken: cts.Token
);

Console.WriteLine("Bot started. Press Ctrl+C to stop.");
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };
await Task.Delay(Timeout.Infinite, cts.Token).ContinueWith(_ => { });
```

`ReceiveAsync` **blocks** the current thread until cancellation:

```csharp
await bot.ReceiveAsync(updateHandler, errorHandler, options, cts.Token);
```

### Webhook

```csharp
// Subscribe to webhook
var result = await bot.Subscribe(
    url: "https://your-domain.com/webhook",
    updateTypes: new[] { UpdateType.MessageCreated, UpdateType.MessageCallback },
    secret: "your-secret-key"  // Used for X-Max-Bot-Api-Secret header
);

// List active subscriptions
var subscriptions = await bot.GetSubscriptions();

// Unsubscribe
await bot.Unsubscribe("https://your-domain.com/webhook");
```

---

## API Methods Reference

### Messages

| Method | HTTP | Description |
|--------|------|-------------|
| `GetMessages(chatId, from?, to?, count?)` | GET | Get messages from a chat by time range |
| `GetMessages(messageIds[])` | GET | Get messages by their IDs |
| `GetMessage(messageId)` | GET | Get a single message by ID |
| `SendMessage(id, message, disableLinkPreview?)` | POST | Send a message |
| `EditMessage(messageId, message)` | PUT | Edit a message (within 24 hours) |
| `DeleteMessage(messageId)` | DELETE | Delete a message (within 24 hours) |
| `AnswerCallback(callbackId, message?, notification?)` | POST | Answer a button callback |

```csharp
// Get last 10 messages from a chat
var messages = await bot.GetMessages(chatId, count: 10);

// Send a message with link preview disabled
await bot.SendMessage(userId, new Message("https://example.com"), disableLinkPreview: true);

// Edit a message
await bot.EditMessage(message.Mid, new Message("Updated text"));

// Delete a message
await bot.DeleteMessage(message.Mid);

// Answer a button callback with a notification
await bot.AnswerCallback(callbackId, notification: "Button clicked!");

// Answer a button callback by updating the message
await bot.AnswerCallback(callbackId, message: new Message("New message text"));
```

---

### Chats

| Method | HTTP | Description |
|--------|------|-------------|
| `GetChats(count?, marker?)` | GET | List bot's chats (paginated) |
| `GetChat(chatId)` | GET | Get chat info |
| `UpdateChat(chatId, request)` | PATCH | Update chat title/icon |
| `DeleteChat(chatId)` | DELETE | Delete a chat for all members |
| `SendAction(chatId, action)` | POST | Send a chat action (typing, sending_photo...) |
| `GetPinnedMessage(chatId)` | GET | Get the pinned message |
| `PinMessage(chatId, messageId, notify?)` | PUT | Pin a message |
| `UnpinMessage(chatId)` | DELETE | Unpin the current message |

```csharp
// Iterate all chats with pagination
long? marker = null;
do
{
    var response = await bot.GetChats(count: 50, marker: marker);
    foreach (var chat in response.Chats)
        Console.WriteLine($"{chat.Title} ({chat.ChatId})");
    marker = response.Marker;
} while (marker != null);

// Show "bot is typing..."
await bot.SendAction(chatId, SenderAction.TypingOn);

// Pin a message silently
await bot.PinMessage(chatId, messageId, notify: false);

// Rename a chat
await bot.UpdateChat(chatId, new UpdateChatRequest { Title = "New Chat Name" });
```

---

### Members

| Method | HTTP | Description |
|--------|------|-------------|
| `GetMyMembership(chatId)` | GET | Bot's membership in a chat |
| `LeaveChat(chatId)` | DELETE | Leave a chat |
| `GetChatMembers(chatId, userIds?, count?, marker?)` | GET | List chat members |
| `AddChatMembers(chatId, userIds[])` | POST | Add members to a chat |
| `RemoveChatMember(chatId, userId, block?)` | DELETE | Remove a member |
| `GetChatAdmins(chatId)` | GET | List chat admins |
| `AddChatAdmins(chatId, admins[])` | POST | Promote admins |
| `RemoveChatAdmin(chatId, userId)` | DELETE | Demote an admin |

```csharp
// List chat members
var members = await bot.GetChatMembers(chatId, count: 100);
foreach (var member in members.Members)
    Console.WriteLine($"{member.User?.Name} — {member.Role}");

// Add users to a chat
await bot.AddChatMembers(chatId, new[] { userId1, userId2 });

// Remove and block a member
await bot.RemoveChatMember(chatId, userId, block: true);

// Promote an admin with specific permissions
await bot.AddChatAdmins(chatId, new[]
{
    new ChatAdmin { UserId = userId, Permissions = AdminPermissions.ReadMessages | AdminPermissions.AddMembers }
});
```

---

### Subscriptions

| Method | HTTP | Description |
|--------|------|-------------|
| `GetSubscriptions()` | GET | List webhook subscriptions |
| `Subscribe(url, updateTypes?, secret?)` | POST | Subscribe to webhook |
| `Unsubscribe(url)` | DELETE | Unsubscribe from webhook |
| `GetUpdates(limit?, timeout?, marker?, types?)` | GET | Get updates (low-level) |

---

### Miscellaneous

| Method | HTTP | Description |
|--------|------|-------------|
| `GetMe()` | GET | Get bot info |
| `GetUploadUrl(type)` | POST | Get a URL to upload a file |
| `GetVideo(videoToken)` | GET | Get video details |

```csharp
// Get an upload URL for a photo
var upload = await bot.GetUploadUrl(UploadType.Photo);
// Upload your file to upload.Url, then use the token in a message

// Get video details
var video = await bot.GetVideo(videoAttachment.Token);
Console.WriteLine($"Duration: {video.Duration}s, {video.Width}x{video.Height}");
```

---

## Message Builder

The `Message` class is used for creating, receiving, and editing messages all in one:

```csharp
// Plain text
var msg = new Message("Hello World!");

// Text with markdown formatting
var msg = new Message("**Bold** and _italic_");

// Photo from URL
var msg = new Message("Caption").WithPhoto("https://example.com/photo.jpg");

// Multiple photos
var msg = new Message()
    .WithPhoto("https://example.com/photo1.jpg")
    .AddPhoto("https://example.com/photo2.jpg");

// Photo from token (already uploaded)
var msg = new Message().WithPhoto(PhotoAttachment.FromToken("token123"));

// Video
var msg = new Message("Video").WithVideo("video_token");

// Combined video + photo
var msg = new Message("Media")
    .WithVideo("video_token")
    .WithPhoto("https://example.com/photo.jpg");

// Inline keyboard
var msg = new Message("Choose an action:")
    .WithKeyboard(kb => kb
        .AddRow()
            .AddCallbackButton("Button 1", "payload_1")
            .AddCallbackButton("Button 2", "payload_2")
        .AddRow()
            .AddLinkButton("Website", "https://example.com")
    );

// Send to a chat (default recipient is user)
var msg = new Message("Hello!").ToChat();

// Reply / forward
var msg = new Message("Reply text")
    .WithLink(MessageLinkRequestType.Reply, messageId, chatId);
```

### Reading incoming messages

```csharp
bot.StartReceiving(async (bot, update, ct) =>
{
    var msg = update.Message;
    if (msg == null) return;

    Console.WriteLine($"Text: {msg.Text}");
    Console.WriteLine($"From: {msg.Sender?.Name} ({msg.Sender?.UserId})");
    Console.WriteLine($"ID: {msg.Mid}");
    Console.WriteLine($"Time: {DateTimeOffset.FromUnixTimeMilliseconds(msg.Timestamp)}");

    // Access attachments
    var photos = msg.GetPhotos();       // PhotoAttachment[]
    var videos = msg.GetVideos();       // VideoAttachment[]
    var files  = msg.GetFiles();        // FileAttachment[]
    var audio  = msg.GetAudios();       // AudioAttachment[]

    // Check for attachments
    if (msg.HasAttachments())
        Console.WriteLine($"Attachments: {msg.GetAllAttachments().Length}");

    // Handle button callbacks
    if (update.Callback != null)
    {
        Console.WriteLine($"Button payload: {update.Callback.Payload}");
        await bot.AnswerCallback(update.Callback.CallbackId, notification: "OK");
    }
});
```

---

## Error Handling

```csharp
try
{
    await bot.SendMessage(userId, new Message("Text"));
}
catch (MaxBotClientApiException ex)
{
    Console.WriteLine($"Status code: {(int)ex.StatusCode}");
    Console.WriteLine($"Response body: {ex.ResponseBody}");
    Console.WriteLine($"Is retryable: {ex.IsRetryable}"); // true for 429/5xx
}
catch (OperationCanceledException)
{
    Console.WriteLine("Request was cancelled");
}
```
