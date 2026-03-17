using System;
using System.Collections.Generic;
using System.Linq;
using Max.BotClient.Types.Builders;

namespace Max.BotClient.Types
{
    /// <summary>
    /// Ответ метода GET /messages.
    /// </summary>
    public class GetMessagesResponse
    {
        /// <summary>
        /// Массив сообщений.
        /// </summary>
        public Message[]? Messages { get; set; }
    }

    /// <summary>
    /// Тип связи сообщения.
    /// </summary>
    public enum MessageLinkType
    {
        Forward,
        Reply
    }

    /// <summary>
    /// Тип чата.
    /// </summary>
    public enum ChatType
    {
        Chat,
        Dialog,
        Channel
    }

    /// <summary>
    /// Тип вложения.
    /// </summary>
    public enum AttachmentType
    {
        Image,
        Video,
        Audio,
        File,
        Sticker,
        Contact,
        Share,
        Location,
        InlineKeyboard
    }

    /// <summary>
    /// Тип элемента разметки.
    /// </summary>
    public enum MarkupType
    {
        Strong,
        Emphasized,
        Monospaced,
        Link,
        Strikethrough,
        Underline,
        UserMention
    }

    /// <summary>
    /// Тип получателя сообщения.
    /// </summary>
    public enum RecipientType
    {
        /// <summary>
        /// Отправка пользователю (по умолчанию).
        /// </summary>
        User,

        /// <summary>
        /// Отправка в чат.
        /// </summary>
        Chat
    }

    /// <summary>
    /// Сообщение в чате. Используется для получения, создания и редактирования сообщений.
    /// <see href="https://dev.max.ru/docs-api/objects/Message"/>
    /// </summary>
    public class Message
    {
        // === Builder state (для создания и редактирования) ===
        private string? _builderText;
        private readonly Dictionary<AttachmentType, List<IAttachment>> _builderAttachments = new Dictionary<AttachmentType, List<IAttachment>>();
        private InlineKeyboardBuilder? _builderKeyboard;
        private NewMessageLink? _builderLink;
        private bool? _builderNotify = true;
        private TextFormat? _builderFormat = TextFormat.Markdown;
        private RecipientType _builderRecipientType = RecipientType.User;
        private bool _isBuilderMode;

        // === Sender ===

        /// <summary>
        /// Пользователь, отправивший сообщение.
        /// </summary>
        public User? Sender { get; internal set; }

        // === Recipient ===

        /// <summary>
        /// ID чата получателя.
        /// </summary>
        public long? RecipientChatId { get; internal set; }

        /// <summary>
        /// Тип чата получателя.
        /// </summary>
        public ChatType? RecipientChatType { get; internal set; }

        /// <summary>
        /// ID пользователя получателя.
        /// </summary>
        public long? RecipientUserId { get; internal set; }

        // === Message ===

        /// <summary>
        /// Время создания сообщения в формате Unix-time.
        /// </summary>
        public long? Timestamp { get; internal set; }

        /// <summary>
        /// Публичная ссылка на пост в канале.
        /// </summary>
        public string? Url { get; internal set; }

        // === Link (LinkedMessage) ===

        /// <summary>
        /// Тип связанного сообщения.
        /// </summary>
        public MessageLinkType? LinkType { get; internal set; }

        /// <summary>
        /// Отправитель связанного сообщения.
        /// </summary>
        public User? LinkSender { get; internal set; }

        /// <summary>
        /// ID чата связанного сообщения.
        /// </summary>
        public long? LinkChatId { get; internal set; }

        /// <summary>
        /// ID связанного сообщения.
        /// </summary>
        public string? LinkMid { get; internal set; }

        /// <summary>
        /// Seq связанного сообщения.
        /// </summary>
        public long? LinkSeq { get; internal set; }

        /// <summary>
        /// Текст связанного сообщения.
        /// </summary>
        public string? LinkText { get; internal set; }

        /// <summary>
        /// Вложения связанного сообщения.
        /// </summary>
        public Attachment[]? LinkAttachments { get; internal set; }

        /// <summary>
        /// Разметка связанного сообщения.
        /// </summary>
        public MarkupElement[]? LinkMarkup { get; internal set; }

        // === Body (MessageBody) ===

        /// <summary>
        /// Уникальный ID сообщения.
        /// </summary>
        public string? Mid { get; internal set; }

        /// <summary>
        /// ID последовательности сообщения в чате.
        /// </summary>
        public long? Seq { get; internal set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string? Text { get; internal set; }

        /// <summary>
        /// Вложения сообщения (сырые данные из API).
        /// </summary>
        internal Attachment[]? Attachments { get; set; }

        /// <summary>
        /// Разметка сообщения.
        /// </summary>
        public MarkupElement[]? Markup { get; internal set; }

        // === Stat (MessageStat) ===

        /// <summary>
        /// Количество просмотров.
        /// </summary>
        public int? Views { get; internal set; }

        // === Конструкторы ===

        /// <summary>
        /// Создает новое пустое сообщение для отправки.
        /// </summary>
        public Message()
        {
            _isBuilderMode = true;
        }

        /// <summary>
        /// Создает новое сообщение с текстом.
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        public Message(string text) : this()
        {
            _builderText = text;
        }

        // === Методы получения вложений (Get*) ===

        /// <summary>
        /// Получает все вложения типа Photo.
        /// </summary>
        public IReadOnlyList<PhotoAttachment> GetPhotos()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Image, out var builderPhotos))
            {
                return builderPhotos.Cast<PhotoAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Image)
                .Select(a => new PhotoAttachment
                {
                    PhotoId = a.PhotoId,
                    Url = a.Url,
                    Token = a.Token
                })
                .ToList() ?? new List<PhotoAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа Video.
        /// </summary>
        public IReadOnlyList<VideoAttachment> GetVideos()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Video, out var builderVideos))
            {
                return builderVideos.Cast<VideoAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Video)
                .Select(a => new VideoAttachment
                {
                    Url = a.Url,
                    Token = a.Token,
                    Width = a.Width,
                    Height = a.Height,
                    Duration = a.Duration,
                    ThumbnailUrl = a.ThumbnailUrl
                })
                .ToList() ?? new List<VideoAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа Audio.
        /// </summary>
        public IReadOnlyList<AudioAttachment> GetAudio()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Audio, out var builderAudio))
            {
                return builderAudio.Cast<AudioAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Audio)
                .Select(a => new AudioAttachment
                {
                    Url = a.Url,
                    Token = a.Token,
                    Transcription = a.Transcription
                })
                .ToList() ?? new List<AudioAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа File.
        /// </summary>
        public IReadOnlyList<FileAttachment> GetFiles()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.File, out var builderFiles))
            {
                return builderFiles.Cast<FileAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.File)
                .Select(a => new FileAttachment
                {
                    Url = a.Url,
                    Token = a.Token,
                    Filename = a.Filename,
                    Size = a.Size
                })
                .ToList() ?? new List<FileAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа Sticker.
        /// </summary>
        public IReadOnlyList<StickerAttachment> GetStickers()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Sticker, out var builderStickers))
            {
                return builderStickers.Cast<StickerAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Sticker)
                .Select(a => new StickerAttachment
                {
                    Url = a.Url,
                    Code = a.Code,
                    Width = a.Width,
                    Height = a.Height
                })
                .ToList() ?? new List<StickerAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа Contact.
        /// </summary>
        public IReadOnlyList<ContactAttachment> GetContacts()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Contact, out var builderContacts))
            {
                return builderContacts.Cast<ContactAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Contact)
                .Select(a => new ContactAttachment
                {
                    VcfInfo = a.VcfInfo,
                    ContactUser = a.ContactUser
                })
                .ToList() ?? new List<ContactAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа Share.
        /// </summary>
        public IReadOnlyList<ShareAttachment> GetShares()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Share, out var builderShares))
            {
                return builderShares.Cast<ShareAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Share)
                .Select(a => new ShareAttachment
                {
                    Url = a.Url,
                    Token = a.Token,
                    Title = a.Title,
                    Description = a.Description,
                    ImageUrl = a.ImageUrl
                })
                .ToList() ?? new List<ShareAttachment>();
        }

        /// <summary>
        /// Получает все вложения типа Location.
        /// </summary>
        public IReadOnlyList<LocationAttachment> GetLocations()
        {
            if (_isBuilderMode && _builderAttachments.TryGetValue(AttachmentType.Location, out var builderLocations))
            {
                return builderLocations.Cast<LocationAttachment>().ToList();
            }

            return Attachments?
                .Where(a => a.Type == AttachmentType.Location)
                .Select(a => new LocationAttachment
                {
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                })
                .ToList() ?? new List<LocationAttachment>();
        }

        /// <summary>
        /// Получает inline клавиатуру.
        /// </summary>
        public InlineKeyboardAttachment? GetKeyboard()
        {
            if (_isBuilderMode && _builderKeyboard != null)
            {
                return _builderKeyboard.ToKeyboardAttachment();
            }

            var keyboardAttachment = Attachments?.FirstOrDefault(a => a.Type == AttachmentType.InlineKeyboard);
            if (keyboardAttachment == null) return null;

            return new InlineKeyboardAttachment
            {
                Buttons = keyboardAttachment.Buttons
            };
        }

        // === Методы построения (With*) ===

        /// <summary>
        /// Устанавливает текст сообщения.
        /// </summary>
        public Message WithText(string text)
        {
            _isBuilderMode = true;
            _builderText = text;
            return this;
        }

        /// <summary>
        /// Устанавливает формат текста (Markdown или HTML).
        /// </summary>
        public Message WithFormat(TextFormat format)
        {
            _isBuilderMode = true;
            _builderFormat = format;
            return this;
        }

        /// <summary>
        /// Добавляет или заменяет изображения. При первом вызове заменяет все существующие изображения.
        /// </summary>
        public Message WithPhoto(string url)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Image))
                _builderAttachments[AttachmentType.Image] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Image].Clear();

            _builderAttachments[AttachmentType.Image].Add(PhotoAttachment.FromUrl(url));
            return this;
        }

        /// <summary>
        /// Добавляет изображение к уже существующим.
        /// </summary>
        public Message AddPhoto(string url)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Image))
                _builderAttachments[AttachmentType.Image] = new List<IAttachment>();

            _builderAttachments[AttachmentType.Image].Add(PhotoAttachment.FromUrl(url));
            return this;
        }

        /// <summary>
        /// Добавляет изображение по токену.
        /// </summary>
        public Message WithPhotoToken(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Image))
                _builderAttachments[AttachmentType.Image] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Image].Clear();

            _builderAttachments[AttachmentType.Image].Add(PhotoAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Заменяет все изображения на указанные.
        /// </summary>
        public Message ReplacePhotos(params PhotoAttachment[] photos)
        {
            _isBuilderMode = true;
            _builderAttachments[AttachmentType.Image] = new List<IAttachment>(photos);
            return this;
        }

        /// <summary>
        /// Удаляет все изображения.
        /// </summary>
        public Message ClearPhotos()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Image);
            return this;
        }

        /// <summary>
        /// Добавляет или заменяет видео.
        /// </summary>
        public Message WithVideo(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Video))
                _builderAttachments[AttachmentType.Video] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Video].Clear();

            _builderAttachments[AttachmentType.Video].Add(VideoAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Добавляет видео к уже существующим.
        /// </summary>
        public Message AddVideo(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Video))
                _builderAttachments[AttachmentType.Video] = new List<IAttachment>();

            _builderAttachments[AttachmentType.Video].Add(VideoAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Заменяет все видео на указанные.
        /// </summary>
        public Message ReplaceVideos(params VideoAttachment[] videos)
        {
            _isBuilderMode = true;
            _builderAttachments[AttachmentType.Video] = new List<IAttachment>(videos);
            return this;
        }

        /// <summary>
        /// Удаляет все видео.
        /// </summary>
        public Message ClearVideos()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Video);
            return this;
        }

        /// <summary>
        /// Добавляет или заменяет аудио.
        /// </summary>
        public Message WithAudio(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Audio))
                _builderAttachments[AttachmentType.Audio] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Audio].Clear();

            _builderAttachments[AttachmentType.Audio].Add(AudioAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Добавляет аудио к уже существующим.
        /// </summary>
        public Message AddAudio(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Audio))
                _builderAttachments[AttachmentType.Audio] = new List<IAttachment>();

            _builderAttachments[AttachmentType.Audio].Add(AudioAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Заменяет все аудио на указанные.
        /// </summary>
        public Message ReplaceAudio(params AudioAttachment[] audio)
        {
            _isBuilderMode = true;
            _builderAttachments[AttachmentType.Audio] = new List<IAttachment>(audio);
            return this;
        }

        /// <summary>
        /// Удаляет все аудио.
        /// </summary>
        public Message ClearAudio()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Audio);
            return this;
        }

        /// <summary>
        /// Добавляет или заменяет файлы.
        /// </summary>
        public Message WithFile(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.File))
                _builderAttachments[AttachmentType.File] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.File].Clear();

            _builderAttachments[AttachmentType.File].Add(FileAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Добавляет файл к уже существующим.
        /// </summary>
        public Message AddFile(string token)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.File))
                _builderAttachments[AttachmentType.File] = new List<IAttachment>();

            _builderAttachments[AttachmentType.File].Add(FileAttachment.FromToken(token));
            return this;
        }

        /// <summary>
        /// Заменяет все файлы на указанные.
        /// </summary>
        public Message ReplaceFiles(params FileAttachment[] files)
        {
            _isBuilderMode = true;
            _builderAttachments[AttachmentType.File] = new List<IAttachment>(files);
            return this;
        }

        /// <summary>
        /// Удаляет все файлы.
        /// </summary>
        public Message ClearFiles()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.File);
            return this;
        }

        /// <summary>
        /// Добавляет стикер.
        /// </summary>
        public Message WithSticker(string code)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Sticker))
                _builderAttachments[AttachmentType.Sticker] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Sticker].Clear();

            _builderAttachments[AttachmentType.Sticker].Add(StickerAttachment.FromCode(code));
            return this;
        }

        /// <summary>
        /// Удаляет все стикеры.
        /// </summary>
        public Message ClearStickers()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Sticker);
            return this;
        }

        /// <summary>
        /// Добавляет контакт.
        /// </summary>
        public Message WithContact(string name, long? contactId = null, string? vcfInfo = null, string? vcfPhone = null)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Contact))
                _builderAttachments[AttachmentType.Contact] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Contact].Clear();

            _builderAttachments[AttachmentType.Contact].Add(ContactAttachment.Create(name, contactId, vcfInfo, vcfPhone));
            return this;
        }

        /// <summary>
        /// Удаляет все контакты.
        /// </summary>
        public Message ClearContacts()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Contact);
            return this;
        }

        /// <summary>
        /// Добавляет геолокацию.
        /// </summary>
        public Message WithLocation(double latitude, double longitude)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Location))
                _builderAttachments[AttachmentType.Location] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Location].Clear();

            _builderAttachments[AttachmentType.Location].Add(LocationAttachment.Create(latitude, longitude));
            return this;
        }

        /// <summary>
        /// Удаляет все геолокации.
        /// </summary>
        public Message ClearLocations()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Location);
            return this;
        }

        /// <summary>
        /// Добавляет ссылку для предпросмотра.
        /// </summary>
        public Message WithShare(string url)
        {
            _isBuilderMode = true;
            if (!_builderAttachments.ContainsKey(AttachmentType.Share))
                _builderAttachments[AttachmentType.Share] = new List<IAttachment>();
            else
                _builderAttachments[AttachmentType.Share].Clear();

            _builderAttachments[AttachmentType.Share].Add(ShareAttachment.FromUrl(url));
            return this;
        }

        /// <summary>
        /// Удаляет все ссылки.
        /// </summary>
        public Message ClearShares()
        {
            _isBuilderMode = true;
            _builderAttachments.Remove(AttachmentType.Share);
            return this;
        }

        /// <summary>
        /// Добавляет inline клавиатуру к сообщению.
        /// </summary>
        public Message WithKeyboard(InlineKeyboardBuilder keyboard)
        {
            _isBuilderMode = true;
            _builderKeyboard = keyboard;
            return this;
        }

        /// <summary>
        /// Добавляет inline клавиатуру к сообщению через функцию конфигурации.
        /// </summary>
        public Message WithKeyboard(Action<InlineKeyboardBuilder> configure)
        {
            _isBuilderMode = true;
            _builderKeyboard = new InlineKeyboardBuilder();
            configure(_builderKeyboard);
            return this;
        }

        /// <summary>
        /// Удаляет клавиатуру.
        /// </summary>
        public Message ClearKeyboard()
        {
            _isBuilderMode = true;
            _builderKeyboard = null;
            return this;
        }

        /// <summary>
        /// Добавляет ссылку на другое сообщение (Reply/Forward).
        /// </summary>
        public Message WithLink(MessageLinkRequestType type, string messageId)
        {
            _isBuilderMode = true;
            _builderLink = new NewMessageLink
            {
                Type = type,
                Mid = messageId
            };
            return this;
        }

        /// <summary>
        /// Отключает уведомление для получателя.
        /// </summary>
        public Message WithoutNotification()
        {
            _isBuilderMode = true;
            _builderNotify = false;
            return this;
        }

        /// <summary>
        /// Включает уведомление для получателя (по умолчанию включено).
        /// </summary>
        public Message WithNotification()
        {
            _isBuilderMode = true;
            _builderNotify = true;
            return this;
        }

        /// <summary>
        /// Указывает, что сообщение должно быть отправлено в чат.
        /// </summary>
        public Message ToChat()
        {
            _isBuilderMode = true;
            _builderRecipientType = RecipientType.Chat;
            return this;
        }

        /// <summary>
        /// Указывает, что сообщение должно быть отправлено пользователю (по умолчанию).
        /// </summary>
        public Message ToUser()
        {
            _isBuilderMode = true;
            _builderRecipientType = RecipientType.User;
            return this;
        }

        // === Внутренние методы для API ===

        /// <summary>
        /// Возвращает тип получателя сообщения.
        /// </summary>
        internal RecipientType GetRecipientType() => _builderRecipientType;

        /// <summary>
        /// Преобразует сообщение в NewMessageBody для отправки через API.
        /// </summary>
        internal NewMessageBody ToMessageBody()
        {
            var attachments = new List<AttachmentRequest>();

            // Собираем все вложения из builder'а
            foreach (var kvp in _builderAttachments)
            {
                foreach (var attachment in kvp.Value)
                {
                    attachments.Add(ConvertToAttachmentRequest(attachment));
                }
            }

            // Добавляем клавиатуру как attachment, если она есть
            if (_builderKeyboard != null)
            {
                attachments.Add(_builderKeyboard.ToAttachmentRequest());
            }

            return new NewMessageBody
            {
                Text = _builderText ?? Text,
                Attachments = attachments.Count > 0 ? attachments.ToArray() : null,
                Link = _builderLink,
                Notify = _builderNotify,
                Format = _builderFormat
            };
        }

        private AttachmentRequest ConvertToAttachmentRequest(IAttachment attachment)
        {
            switch (attachment)
            {
                case PhotoAttachment photo:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Image,
                        Payload = new AttachmentPayload
                        {
                            Url = photo.Url,
                            Token = photo.Token
                        }
                    };

                case VideoAttachment video:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Video,
                        Payload = new AttachmentPayload
                        {
                            Token = video.Token
                        }
                    };

                case AudioAttachment audio:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Audio,
                        Payload = new AttachmentPayload
                        {
                            Token = audio.Token
                        }
                    };

                case FileAttachment file:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.File,
                        Payload = new AttachmentPayload
                        {
                            Token = file.Token
                        }
                    };

                case StickerAttachment sticker:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Sticker,
                        Payload = new AttachmentPayload
                        {
                            Code = sticker.Code
                        }
                    };

                case ContactAttachment contact:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Contact,
                        Payload = new AttachmentPayload
                        {
                            Name = contact.Name,
                            ContactId = contact.ContactId,
                            VcfInfo = contact.VcfInfo,
                            VcfPhone = contact.VcfPhone
                        }
                    };

                case LocationAttachment location:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Location,
                        Payload = new AttachmentPayload
                        {
                            Latitude = location.Latitude,
                            Longitude = location.Longitude
                        }
                    };

                case ShareAttachment share:
                    return new AttachmentRequest
                    {
                        Type = AttachmentRequestType.Share,
                        Payload = new AttachmentPayload
                        {
                            ShareUrl = share.Url
                        }
                    };

                default:
                    throw new NotSupportedException($"Attachment type {attachment.GetType().Name} is not supported");
            }
        }
    }

    /// <summary>
    /// Вложение сообщения (сырые данные из API).
    /// </summary>
    public class Attachment
    {
        public AttachmentType? Type { get; set; }
        public long? PhotoId { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Duration { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Transcription { get; set; }
        public string? Filename { get; set; }
        public long? Size { get; set; }
        public string? Code { get; set; }
        public string? VcfInfo { get; set; }
        public User? ContactUser { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Button[][]? Buttons { get; set; }
        public string? Url { get; set; }
        public string? Token { get; set; }
    }

    /// <summary>
    /// Элемент разметки сообщения.
    /// </summary>
    public class MarkupElement
    {
        /// <summary>
        /// Тип элемента разметки.
        /// </summary>
        public MarkupType? Type { get; set; }

        /// <summary>
        /// Индекс начала элемента разметки в тексте.
        /// </summary>
        public int? From { get; set; }

        /// <summary>
        /// Длина элемента разметки.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// URL ссылки (для Link).
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// @username упомянутого пользователя (для UserMention).
        /// </summary>
        public string? UserLink { get; set; }

        /// <summary>
        /// ID упомянутого пользователя (для UserMention).
        /// </summary>
        public long? UserId { get; set; }
    }

    /// <summary>
    /// Тип кнопки.
    /// </summary>
    public enum ButtonType
    {
        Callback,
        Link,
        RequestGeoLocation,
        RequestContact,
        OpenApp,
        Message
    }

    /// <summary>
    /// Кнопка клавиатуры.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Тип кнопки.
        /// </summary>
        public ButtonType? Type { get; set; }

        /// <summary>
        /// Видимый текст кнопки.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Токен кнопки (для Callback).
        /// </summary>
        public string? Payload { get; set; }

        /// <summary>
        /// URL ссылки (для Link).
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Отправить местоположение без подтверждения (для RequestGeoLocation).
        /// </summary>
        public bool? Quick { get; set; }

        /// <summary>
        /// Username бота для мини-приложения (для OpenApp).
        /// </summary>
        public string? WebApp { get; set; }

        /// <summary>
        /// ID бота для мини-приложения (для OpenApp).
        /// </summary>
        public long? ContactId { get; set; }
    }
}
