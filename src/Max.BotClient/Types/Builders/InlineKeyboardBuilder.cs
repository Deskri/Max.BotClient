using System;
using System.Collections.Generic;
using System.Linq;

namespace Max.BotClient.Types.Builders
{
    /// <summary>
    /// Builder для создания inline клавиатуры с кнопками.
    /// </summary>
    public class InlineKeyboardBuilder
    {
        private readonly List<List<ButtonRequest>> _rows = new List<List<ButtonRequest>>();
        private List<ButtonRequest>? _currentRow;

        /// <summary>
        /// Начинает новый ряд кнопок.
        /// </summary>
        public InlineKeyboardBuilder AddRow()
        {
            _currentRow = new List<ButtonRequest>();
            _rows.Add(_currentRow);
            return this;
        }

        /// <summary>
        /// Добавляет кнопку с callback в текущий ряд.
        /// </summary>
        /// <param name="text">Текст на кнопке</param>
        /// <param name="payload">Данные callback (до 4000 символов)</param>
        public InlineKeyboardBuilder AddCallbackButton(string text, string payload)
        {
            EnsureCurrentRow();
            _currentRow!.Add(new ButtonRequest
            {
                Type = ButtonRequestType.Callback,
                Text = text,
                Payload = payload
            });
            return this;
        }

        /// <summary>
        /// Добавляет кнопку со ссылкой в текущий ряд.
        /// </summary>
        /// <param name="text">Текст на кнопке</param>
        /// <param name="url">URL ссылки</param>
        public InlineKeyboardBuilder AddLinkButton(string text, string url)
        {
            EnsureCurrentRow();
            _currentRow!.Add(new ButtonRequest
            {
                Type = ButtonRequestType.Link,
                Text = text,
                Url = url
            });
            return this;
        }

        /// <summary>
        /// Добавляет кнопку запроса геолокации в текущий ряд.
        /// </summary>
        /// <param name="text">Текст на кнопке</param>
        /// <param name="quick">Быстрый запрос (без подтверждения)</param>
        public InlineKeyboardBuilder AddRequestGeoLocationButton(string text, bool quick = false)
        {
            EnsureCurrentRow();
            _currentRow!.Add(new ButtonRequest
            {
                Type = ButtonRequestType.RequestGeoLocation,
                Text = text,
                Quick = quick
            });
            return this;
        }

        /// <summary>
        /// Добавляет кнопку запроса контакта в текущий ряд.
        /// </summary>
        /// <param name="text">Текст на кнопке</param>
        public InlineKeyboardBuilder AddRequestContactButton(string text)
        {
            EnsureCurrentRow();
            _currentRow!.Add(new ButtonRequest
            {
                Type = ButtonRequestType.RequestContact,
                Text = text
            });
            return this;
        }

        /// <summary>
        /// Добавляет кнопку открытия веб-приложения в текущий ряд.
        /// </summary>
        /// <param name="text">Текст на кнопке</param>
        /// <param name="webApp">URL веб-приложения</param>
        /// <param name="chatId">ID чата (опционально)</param>
        public InlineKeyboardBuilder AddOpenAppButton(string text, string webApp, long? chatId = null)
        {
            EnsureCurrentRow();
            _currentRow!.Add(new ButtonRequest
            {
                Type = ButtonRequestType.OpenApp,
                Text = text,
                WebApp = webApp,
                ChatId = chatId
            });
            return this;
        }

        /// <summary>
        /// Добавляет кнопку отправки текстового сообщения в текущий ряд.
        /// </summary>
        /// <param name="text">Текст на кнопке (будет отправлен как сообщение)</param>
        public InlineKeyboardBuilder AddMessageButton(string text)
        {
            EnsureCurrentRow();
            _currentRow!.Add(new ButtonRequest
            {
                Type = ButtonRequestType.Message,
                Text = text
            });
            return this;
        }

        /// <summary>
        /// Конвертирует builder в InlineKeyboardAttachment для чтения.
        /// </summary>
        internal InlineKeyboardAttachment ToKeyboardAttachment()
        {
            return new InlineKeyboardAttachment
            {
                Buttons = _rows.Select(row => row.Select(br => new Button
                {
                    Type = (ButtonType)(int)br.Type,
                    Text = br.Text,
                    Payload = br.Payload,
                    Url = br.Url,
                    Quick = br.Quick,
                    WebApp = br.WebApp,
                    ContactId = br.ChatId
                }).ToArray()).ToArray()
            };
        }

        /// <summary>
        /// Возвращает AttachmentRequest для использования в отправке сообщения.
        /// </summary>
        internal AttachmentRequest ToAttachmentRequest()
        {
            if (_rows.Count == 0)
            {
                throw new InvalidOperationException("Клавиатура не содержит ни одного ряда кнопок. Используйте AddRow() для добавления ряда.");
            }

            return new AttachmentRequest
            {
                Type = AttachmentRequestType.InlineKeyboard,
                Payload = new AttachmentPayload
                {
                    Buttons = _rows.ConvertAll(row => row.ToArray()).ToArray()
                }
            };
        }

        private void EnsureCurrentRow()
        {
            if (_currentRow == null)
            {
                AddRow();
            }
        }
    }
}
