using NewsSite.Domain.Entities;

namespace NewsSite.Web.Extensions
    {
    // Хелпер для выбора полей новости в зависимости от текущей культуры (ru/en)
    public static class NewsExtensions
        {
        public static (string Title, string Subtitle, string Text) ForCulture(this News n, string? culture)
            {
            var c = culture?.StartsWith("en", StringComparison.OrdinalIgnoreCase) == true ? "en" : "ru";
            return c == "en"
                ? (n.TitleEn, n.SubtitleEn ?? string.Empty, n.TextEn)
                : (n.TitleRu, n.SubtitleRu ?? string.Empty, n.TextRu);
            }
        }
    }
