using System;
using System.ComponentModel.DataAnnotations;

namespace NewsSite.Domain.Entities
    {
    public class News
        {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно")]
        [StringLength(200, ErrorMessage = "Максимум 200 символов")]
        public string TitleRu { get; set; } = string.Empty;

        [StringLength(400)]
        public string? SubtitleRu { get; set; }

        [Required]
        public string TextRu { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string TitleEn { get; set; } = string.Empty;

        [StringLength(400)]
        public string? SubtitleEn { get; set; }

        [Required]
        public string TextEn { get; set; } = string.Empty;

        [StringLength(300)]
        public string? ImagePath { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = true;
        }
    }
