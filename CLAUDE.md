# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build the library
dotnet build src/Max.BotClient/Max.BotClient.csproj

# Build in Release mode
dotnet build src/Max.BotClient/Max.BotClient.csproj -c Release
```

## Project Overview

Max.BotClient is a .NET Standard 2.0 class library SDK for MAX Bot API (for broad compatibility across .NET Framework 4.6.1+ and .NET Core 2.0+).

**Solution structure:**
- `Max.BotClient.slnx` - Solution file (XML format)
- `src/Max.BotClient/` - Main library project

**Core files:**
- `Max.BotClient.cs` - Main `BotClient` class (`partial class`), implements `IBotClient`, contains `SendRequest<T>` with retry/exponential backoff
- `Max.BotClient.Options.cs` - `BotClientOptions` (token, API URL, retry settings)
- `Max.BotClient.JsonOptions.cs` - `BotClientJsonOptions` with snake_case naming policy
- `Max.BotClient.ApiException.cs` - `MaxBotClientApiException` with `IsRetryable` (429/5xx)
- `Max.BotClient.ApiExtensions.cs` - Internal `ProcessApi<TDto, TResult>` helper methods
- `Max.BotClient.Extensions.cs` - Internal `ToSnakeCase()` utility

**API method files (partial class `BotClientApiMethods`):**
- `Max.BotClient.ApiMethods.Messages.cs` - GetMessages, GetMessage, SendMessage, EditMessage, DeleteMessage, AnswerCallback
- `Max.BotClient.ApiMethods.Chats.cs` - GetChats, GetChat, UpdateChat, DeleteChat, SendAction, PinMessage, UnpinMessage, GetPinnedMessage
- `Max.BotClient.ApiMethods.Members.cs` - GetMyMembership, LeaveChat, GetChatMembers, AddChatMembers, RemoveChatMember, GetChatAdmins, AddChatAdmins, RemoveChatAdmin
- `Max.BotClient.ApiMethods.Subscriptions.cs` - GetSubscriptions, Subscribe, Unsubscribe, GetUpdates
- `Max.BotClient.ApiMethods.Misc.cs` - GetMe, GetUploadUrl, GetVideo
- `Max.BotClient.Polling.cs` - StartReceiving, ReceiveAsync (long polling)

**Types/ folder** - Public API types:
- `Message.cs` - Unified Message class (builder + read + edit), Attachment, MarkupElement, Button, enums
- `Attachments.cs` - IAttachment, PhotoAttachment, VideoAttachment, AudioAttachment, FileAttachment, StickerAttachment, ContactAttachment, ShareAttachment, LocationAttachment, InlineKeyboardAttachment
- `User.cs` - User, UserWithPhoto
- `Update.cs` - Update, GetUpdatesResponse
- `Chat.cs` - Chat, ChatMember, ChatAdmin, GetChatsResponse, GetChatMembersResponse, enums
- `Callback.cs` - Callback
- `BotInfo.cs` - BotInfo, BotCommand
- `Video.cs` - VideoInfo, VideoUrls
- `Upload.cs` - UploadType, UploadResult
- `Subscription.cs` - Subscription, SubscribeResult, UpdateType enum
- `SendMessage.cs` - SendMessageResponse, NewMessageLink, TextFormat, MessageLinkRequestType
- `NewMessageBody.cs` - NewMessageBody
- `AttachmentRequest.cs` - AttachmentRequest, AttachmentPayload, ButtonRequest, enums
- `UpdateChat.cs` - UpdateChatRequest, ChatIcon, PhotoToken
- `Builders/InlineKeyboardBuilder.cs` - Fluent builder for inline keyboards

**DTOs/ folder** - Internal data transfer objects for API JSON serialization:
- `Message.cs`, `User.cs`, `Attachments.cs`, `Buttons.cs`, `Markup.cs`, `Requests.cs`, `Updates.cs`, `Chat.cs`, `Video.cs`

**Mapping/ folder** - Converters between Types and DTOs:
- `UniversalMappingExtensions.cs` - Central `ToResult<TDto, TResult>()` dispatcher used by `ProcessApi`
- `MessageMappingExtensions.cs` - DTOs.Message -> Types.Message
- `ResponseMappingExtensions.cs` - Response DTOs -> Types (GetMessagesResponse, SendMessageResponse, GetUpdatesResponse, BotInfo, Update mappings)
- `AttachmentMappingExtensions.cs` - DTOs.IAttachment -> Types.Attachment, markup, buttons
- `RequestMappingExtensions.cs` - Types -> DTOs for sending (NewMessageBody, AttachmentRequests, Buttons)
- `UserMappingExtensions.cs` - DTOs.User -> Types.User
- `ChatMappingExtensions.cs` - DTOs.Chat -> Types.Chat, ChatMember, ChatAdmin
- `VideoMappingExtensions.cs` - DTOs.VideoInfo -> Types.VideoInfo
- `UpdateChatMappingExtensions.cs` - Types.UpdateChatRequest -> DTO

## Architecture Principles

### Unified Message Architecture

**IMPORTANT:** We use a **single unified `Message` class** for all message operations:
- **Creating** new messages (builder pattern)
- **Receiving** messages from API (read-only properties)
- **Editing** existing messages (builder pattern)

DO NOT CREATE separate classes like `NewMessage`, `EditMessageRequest`, or `MessageBuilder`.
USE the single `Message` class for all scenarios.

### Message Class Design

The `Message` class serves three purposes:

1. **Builder Mode** (Creating/Editing):
   - Internal state: `_builderText`, `_builderAttachments`, `_isBuilderMode`
   - Methods: `WithText()`, `WithPhoto()`, `AddPhoto()`, `ReplacePhotos()`, `ClearPhotos()`, etc.
   - Fluent API for building messages

2. **Read Mode** (Receiving):
   - Public properties: `Text`, `Mid`, `Sender`, `Timestamp`, etc.
   - Properties are `internal set` - only SDK can set them
   - Methods: `GetPhotos()`, `GetVideos()`, `GetFiles()`, etc.
   - Returns typed attachment collections

3. **Conversion** (Internal):
   - `ToMessageBody()` - converts to DTOs for API
   - `ConvertToAttachmentRequest()` - converts typed attachments to DTOs

### DTOs vs Types

- **DTOs** (`DTOs/` namespace) - must be `internal`. Used only for JSON serialization with the API. Polymorphic via interfaces (`IAttachment`, `IButton`, `IMarkupElement`, `IUpdate`).
- **Types** (`Types/` namespace) - `public`. Flat/unified classes. Properties from API responses should use `internal set`. User-created request objects (e.g. `UpdateChatRequest`, `ChatAdmin`) can use regular `set`.

### Mapping Layer

- `ProcessApi<TDto, TResult>()` deserializes JSON into DTO, then calls `UniversalMappingExtensions.ToResult<TDto, TResult>()` to convert.
- **IMPORTANT:** When adding a new `ProcessApi<TDto, TResult>` call, you MUST add the corresponding mapping case in `UniversalMappingExtensions.ToResult()`, otherwise it throws `NotSupportedException` at runtime.
- Individual mapping methods live in dedicated files (MessageMappingExtensions, ChatMappingExtensions, etc.).

### Attachment System

**Typed Attachment Classes** (in `Types/Attachments.cs`):
- `IAttachment` - base interface
- `PhotoAttachment`, `VideoAttachment`, `AudioAttachment`, `FileAttachment`
- `StickerAttachment`, `ContactAttachment`, `ShareAttachment`, `LocationAttachment`
- `InlineKeyboardAttachment`

**Key Features:**
- Properties are `internal set` - immutable from user code
- Static factory methods: `FromUrl()`, `FromToken()`, `FromCode()`, `Create()`
- Used both for receiving (parsed from API) and sending (builder creates them)

### API Methods Pattern

All API methods are extension methods on `BotClient` in `static partial class BotClientApiMethods`:

```csharp
public static async Task<ReturnType> MethodName(
    this BotClient botClient,
    // ... parameters ...
    CancellationToken cancellationToken = default
)
{
    var queryParams = new List<string>();
    // ... build query ...

    var response = await botClient.ProcessApi<DtoType, TypesType>(
        HttpMethod, path, requestBody, cancellationToken
    );

    return response.Property;
}
```

**Key principles:**
- Extension methods on `BotClient`
- Accept `Message` class directly (not separate request types)
- Use `cancellationToken` for async operations
- Map between DTOs (internal) and Types (public API)

### Long Polling

`Max.BotClient.Polling.cs` provides:
- `ReceiverOptions` - Limit, Timeout, AllowedUpdates, DropPendingUpdates
- `StartReceiving()` - non-blocking, launches polling in background Task
- `ReceiveAsync()` - blocking (awaitable), main polling loop with marker-based pagination

Uses existing `GetUpdates()` method. MAX API uses marker (not offset) for pagination.

## Usage Examples

### Creating and Sending Message

```csharp
var message = new Message("Hello World!")
    .WithPhoto("https://example.com/photo.jpg")
    .AddPhoto("https://example.com/photo2.jpg")
    .WithVideo("video_token")
    .WithKeyboard(kb => kb
        .AddRow()
        .AddCallbackButton("Button 1", "payload1"))
    .ToChat();

await botClient.SendMessage(chatId, message);
```

### Long Polling

```csharp
var cts = new CancellationTokenSource();

botClient.StartReceiving(
    updateHandler: async (bot, update, ct) => {
        Console.WriteLine(update.UpdateType);
    },
    errorHandler: async (bot, ex, ct) => {
        Console.WriteLine(ex.Message);
    },
    options: new ReceiverOptions { Timeout = 30 },
    cancellationToken: cts.Token
);

// Or blocking:
await botClient.ReceiveAsync(updateHandler, errorHandler, options, cts.Token);
```

## Implementation Guidelines

### When Adding New API Methods

1. Add method to the appropriate `BotClientApiMethods` partial class file
2. If using `ProcessApi<TDto, TResult>`, add mapping case in `UniversalMappingExtensions.ToResult()`
3. Use existing types (especially `Message` for message operations)
4. Follow async pattern with `CancellationToken`
5. Add XML documentation with `<see href="..."/>` to API docs

### When Adding New Types

1. Add to `Types/` folder - these are public API
2. Add corresponding DTOs to `DTOs/` folder - **must be internal**
3. Add mapping in `Mapping/` folder
4. Register mapping in `UniversalMappingExtensions.ToResult()` if used with `ProcessApi<TDto, TResult>`
5. Use `internal set` for properties that come from API responses
6. Follow builder pattern for mutable operations

### When Working with Messages

- Use `Message` class for all message operations
- Use `With*()` methods to replace attachments
- Use `Add*()` methods to append attachments
- Use `Get*()` methods to retrieve typed attachments
- Do NOT create separate builder classes
- Do NOT expose internal `Attachment` class (use typed classes)
- Do NOT make attachment properties publicly settable
