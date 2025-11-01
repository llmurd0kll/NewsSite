using System.ComponentModel.DataAnnotations;

namespace NewsSite.Web.Models
    {
    public class NewsInputModel
        {
        public int Id { get; set; }

        [Required]
        public string TitleRu { get; set; } = string.Empty;
        public string? SubtitleRu { get; set; }
        public string? TextRu { get; set; }

        [Required]
        public string TitleEn { get; set; } = string.Empty;
        public string? SubtitleEn { get; set; }
        public string? TextEn { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; }

        public IFormFile? Image { get; set; }   // загружаемый файл
        public string? ExistingImagePath { get; set; } // для Edit
        }
    }
