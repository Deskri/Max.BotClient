using Max.BotClient.DTOs;
using Max.BotClient.Types;

namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов Request из Types в DTOs.
    /// </summary>
    internal static class RequestMappingExtensions
    {
        /// <summary>
        /// Преобразует Types NewMessageBody в DTO NewMessageBody.
        /// </summary>
        public static DTOs.NewMessageBody ToDto(this Types.NewMessageBody? type)
        {
            if (type == null) return null!;

            return new DTOs.NewMessageBody
            {
                Text = type.Text,
                Attachments = type.Attachments?.ToDtoAttachments(),
                Link = type.Link?.ToDto(),
                Notify = type.Notify,
                Format = (DTOs.TextFormat?)type.Format
            };
        }

        /// <summary>
        /// Преобразует Types NewMessageLink в DTO NewMessageLink.
        /// </summary>
        public static DTOs.NewMessageLink? ToDto(this Types.NewMessageLink? type)
        {
            if (type == null) return null;

            return new DTOs.NewMessageLink
            {
                Type = (DTOs.MessageLinkType)type.Type,
                Mid = type.Mid
            };
        }

        /// <summary>
        /// Преобразует Types AttachmentRequest[] в DTO IAttachmentRequest[] (конкретные классы в интерфейсы).
        /// </summary>
        public static IAttachmentRequest[]? ToDtoAttachments(this Types.AttachmentRequest[]? types)
        {
            if (types == null) return null;

            var result = new IAttachmentRequest[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                result[i] = types[i].ToDtoAttachment();
            }
            return result;
        }

        /// <summary>
        /// Преобразует один Types AttachmentRequest в DTO IAttachmentRequest.
        /// </summary>
        private static IAttachmentRequest ToDtoAttachment(this Types.AttachmentRequest type)
        {
            switch (type.Type)
            {
                case Types.AttachmentRequestType.Image:
                    return new PhotoAttachmentRequest
                    {
                        Payload = new PhotoAttachmentRequestPayload
                        {
                            Url = type.Payload?.Url,
                            Token = type.Payload?.Token,
                            Photos = type.Payload?.Token != null
                                ? new DTOs.PhotoToken { Token = type.Payload.Token }
                                : null
                        }
                    };

                case Types.AttachmentRequestType.Video:
                    return new VideoAttachmentRequest
                    {
                        Payload = new UploadedInfo
                        {
                            Token = type.Payload?.Token
                        }
                    };

                case Types.AttachmentRequestType.Audio:
                    return new AudioAttachmentRequest
                    {
                        Payload = new UploadedInfo
                        {
                            Token = type.Payload?.Token
                        }
                    };

                case Types.AttachmentRequestType.File:
                    return new FileAttachmentRequest
                    {
                        Payload = new UploadedInfo
                        {
                            Token = type.Payload?.Token
                        }
                    };

                case Types.AttachmentRequestType.Sticker:
                    return new StickerAttachmentRequest
                    {
                        Payload = new StickerAttachmentRequestPayload
                        {
                            Code = type.Payload?.Code!
                        }
                    };

                case Types.AttachmentRequestType.Contact:
                    return new ContactAttachmentRequest
                    {
                        Payload = new ContactAttachmentRequestPayload
                        {
                            Name = type.Payload?.Name,
                            ContactId = type.Payload?.ContactId,
                            VcfInfo = type.Payload?.VcfInfo,
                            VcfPhone = type.Payload?.VcfPhone
                        }
                    };

                case Types.AttachmentRequestType.Location:
                    return new LocationAttachmentRequest
                    {
                        Latitude = type.Payload?.Latitude ?? 0,
                        Longitude = type.Payload?.Longitude ?? 0
                    };

                case Types.AttachmentRequestType.Share:
                    return new ShareAttachmentRequest
                    {
                        Payload = new ShareAttachmentPayload
                        {
                            Url = type.Payload?.ShareUrl
                        }
                    };

                case Types.AttachmentRequestType.InlineKeyboard:
                    return new InlineKeyboardAttachmentRequest
                    {
                        Payload = new InlineKeyboardAttachmentRequestPayload
                        {
                            Buttons = type.Payload?.Buttons?.ToDtoButtons()!
                        }
                    };

                default:
                    return null!;
            }
        }

        /// <summary>
        /// Преобразует Types ButtonRequest[][] в DTO IButton[][].
        /// </summary>
        private static IButton[][]? ToDtoButtons(this Types.ButtonRequest[][]? types)
        {
            if (types == null) return null;

            var result = new IButton[types.Length][];
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] != null)
                {
                    result[i] = new IButton[types[i].Length];
                    for (int j = 0; j < types[i].Length; j++)
                    {
                        result[i][j] = types[i][j].ToDtoButton();
                    }
                }
                else
                {
                    result[i] = null!;
                }
            }
            return result;
        }

        /// <summary>
        /// Преобразует один Types ButtonRequest в DTO IButton.
        /// </summary>
        private static IButton ToDtoButton(this Types.ButtonRequest type)
        {
            switch (type.Type)
            {
                case Types.ButtonRequestType.Callback:
                    return new CallbackButton
                    {
                        Text = type.Text,
                        Payload = type.Payload!
                    };

                case Types.ButtonRequestType.Link:
                    return new LinkButton
                    {
                        Text = type.Text,
                        Url = type.Url!
                    };

                case Types.ButtonRequestType.RequestGeoLocation:
                    return new RequestGeoLocationButton
                    {
                        Text = type.Text,
                        Quick = type.Quick
                    };

                case Types.ButtonRequestType.RequestContact:
                    return new RequestContactButton
                    {
                        Text = type.Text
                    };

                case Types.ButtonRequestType.OpenApp:
                    return new OpenAppButton
                    {
                        Text = type.Text,
                        WebApp = type.WebApp,
                        ContactId = type.ChatId
                    };

                case Types.ButtonRequestType.Message:
                    return new MessageButton
                    {
                        Text = type.Text
                    };

                default:
                    return null!;
            }
        }
    }
}
