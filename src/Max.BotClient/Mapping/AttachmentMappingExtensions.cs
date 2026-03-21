using Max.BotClient.DTOs;
using Max.BotClient.Types;

namespace Max.BotClient.Mapping
{
    /// <summary>
    /// Методы расширения для маппинга типов Attachment между DTOs и Types.
    /// </summary>
    internal static class AttachmentMappingExtensions
    {
        /// <summary>
        /// Преобразует DTO IAttachment[] в Types Attachment[] (интерфейсы в конкретные классы).
        /// </summary>
        public static Types.Attachment[]? ToAttachments(this DTOs.IAttachment[]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.Attachment[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                result[i] = dtos[i].ToAttachment();
            }
            return result;
        }

        /// <summary>
        /// Преобразует одно DTO IAttachment в Types Attachment.
        /// </summary>
        private static Types.Attachment ToAttachment(this DTOs.IAttachment dto)
        {
            var result = new Types.Attachment
            {
                Type = (Types.AttachmentType?)dto.Type
            };

            switch (dto)
            {
                case DTOs.PhotoAttachment photo:
                    result.PhotoId = photo.Payload?.PhotoId;
                    result.Url = photo.Payload?.Url;
                    result.Token = photo.Payload?.Token;
                    break;

                case DTOs.VideoAttachment video:
                    result.Url = video.Payload?.Url;
                    result.Token = video.Payload?.Token;
                    result.Width = video.Width;
                    result.Height = video.Height;
                    result.Duration = video.Duration;
                    result.ThumbnailUrl = video.Thumbnail?.Url;
                    break;

                case DTOs.AudioAttachment audio:
                    result.Url = audio.Payload?.Url;
                    result.Token = audio.Payload?.Token;
                    result.Transcription = audio.Transcription;
                    break;

                case DTOs.FileAttachment file:
                    result.Url = file.Payload?.Url;
                    result.Token = file.Payload?.Token;
                    result.Filename = file.Filename;
                    result.Size = file.Size;
                    break;

                case DTOs.StickerAttachment sticker:
                    result.Url = sticker.Payload?.Url;
                    result.Code = sticker.Payload?.Code;
                    result.Width = sticker.Width;
                    result.Height = sticker.Height;
                    break;

                case DTOs.ContactAttachment contact:
                    result.VcfInfo = contact.Payload?.VcfInfo;
                    result.ContactUser = contact.Payload?.MaxInfo?.ToUser();
                    break;

                case DTOs.ShareAttachment share:
                    result.Url = share.Payload?.Url;
                    result.Token = share.Payload?.Token;
                    result.Title = share.Title;
                    result.Description = share.Description;
                    result.ImageUrl = share.ImageUrl;
                    break;

                case DTOs.LocationAttachment location:
                    result.Latitude = location.Latitude;
                    result.Longitude = location.Longitude;
                    break;

                case DTOs.InlineKeyboardAttachment keyboard:
                    result.Buttons = keyboard.Payload?.Buttons?.ToButtons();
                    break;
            }

            return result;
        }

        /// <summary>
        /// Преобразует DTO IMarkupElement[] в Types MarkupElement[].
        /// </summary>
        public static Types.MarkupElement[]? ToMarkupElements(this IMarkupElement[]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.MarkupElement[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                result[i] = dtos[i].ToMarkupElement();
            }
            return result;
        }

        /// <summary>
        /// Преобразует один DTO IMarkupElement в Types MarkupElement.
        /// </summary>
        private static Types.MarkupElement ToMarkupElement(this IMarkupElement dto)
        {
            var result = new Types.MarkupElement
            {
                Type = (Types.MarkupType?)dto.Type,
                From = dto.From,
                Length = dto.Length
            };

            if (dto is DTOs.LinkMarkupElement link)
            {
                result.Url = link.Url;
            }
            else if (dto is DTOs.UserMentionMarkupElement mention)
            {
                result.UserLink = mention.UserLink;
                result.UserId = mention.UserId;
            }

            return result;
        }

        /// <summary>
        /// Преобразует DTO IButton[][] в Types Button[][].
        /// </summary>
        public static Types.Button[][]? ToButtons(this IButton[][]? dtos)
        {
            if (dtos == null) return null;

            var result = new Types.Button[dtos.Length][];
            for (int i = 0; i < dtos.Length; i++)
            {
                if (dtos[i] != null)
                {
                    result[i] = new Types.Button[dtos[i].Length];
                    for (int j = 0; j < dtos[i].Length; j++)
                    {
                        result[i][j] = dtos[i][j].ToButton();
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
        /// Преобразует одну DTO IButton в Types Button.
        /// </summary>
        private static Types.Button ToButton(this IButton dto)
        {
            var result = new Types.Button
            {
                Type = (Types.ButtonType?)dto.Type,
                Text = dto.Text
            };

            switch (dto)
            {
                case DTOs.CallbackButton callback:
                    result.Payload = callback.Payload;
                    break;

                case DTOs.LinkButton link:
                    result.Url = link.Url;
                    break;

                case DTOs.RequestGeoLocationButton geo:
                    result.Quick = geo.Quick;
                    break;

                case DTOs.OpenAppButton app:
                    result.WebApp = app.WebApp;
                    result.ContactId = app.ContactId;
                    break;
            }

            return result;
        }
    }
}
