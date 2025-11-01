using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Domain.Entities;
using NewsSite.Web.Models;

namespace NewsSite.Web.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
        {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public NewsController(AppDbContext db, IWebHostEnvironment env)
            {
            _db = db;
            _env = env;
            }

        // список
        public async Task<IActionResult> Index()
            {
            var items = await _db.News
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();

            return View(items);
            }

        // GET Create
        public IActionResult Create()
            {
            return View(new NewsInputModel());
            }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsInputModel model)
            {
            if (!ModelState.IsValid)
                return View(model);

            var entity = new News
                {
                TitleRu = model.TitleRu,
                SubtitleRu = model.SubtitleRu,
                TextRu = model.TextRu,
                TitleEn = model.TitleEn,
                SubtitleEn = model.SubtitleEn,
                TextEn = model.TextEn,
                PublishedAt = DateTime.SpecifyKind(model.PublishedAt, DateTimeKind.Utc),
                IsPublished = model.IsPublished,
                ImagePath = await SaveImage(model.Image)
                };

            _db.News.Add(entity);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

        // GET Edit
        public async Task<IActionResult> Edit(int id)
            {
            var item = await _db.News.FindAsync(id);
            if (item == null)
                return NotFound();

            return View(new NewsInputModel
                {
                Id = item.Id,
                TitleRu = item.TitleRu,
                SubtitleRu = item.SubtitleRu,
                TextRu = item.TextRu,
                TitleEn = item.TitleEn,
                SubtitleEn = item.SubtitleEn,
                TextEn = item.TextEn,
                PublishedAt = item.PublishedAt,
                IsPublished = item.IsPublished,
                ExistingImagePath = item.ImagePath
                });
            }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsInputModel model)
            {
            var entity = await _db.News.FindAsync(model.Id);
            if (entity == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            entity.TitleRu = model.TitleRu;
            entity.SubtitleRu = model.SubtitleRu;
            entity.TextRu = model.TextRu;
            entity.TitleEn = model.TitleEn;
            entity.SubtitleEn = model.SubtitleEn;
            entity.TextEn = model.TextEn;
            entity.PublishedAt = DateTime.SpecifyKind(model.PublishedAt, DateTimeKind.Utc);
            entity.IsPublished = model.IsPublished;

            if (model.Image != null)
                entity.ImagePath = await SaveImage(model.Image);

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

        // Удаление
        public async Task<IActionResult> Delete(int id)
            {
            var item = await _db.News.FindAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
            }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            var item = await _db.News.FindAsync(id);
            if (item == null)
                return NotFound();

            _db.News.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }

        private async Task<string?> SaveImage(IFormFile? file)
            {
            if (file == null || file.Length == 0)
                return null;

            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                {
                await file.CopyToAsync(stream);
                }

            return $"/uploads/{fileName}";
            }
        }
    }
