using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Domain.Entities;

namespace NewsSite.Web.Areas.Admin.Controllers
    {
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // только админ
    public class NewsController : Controller
        {
        private readonly AppDbContext _db;

        public NewsController(AppDbContext db)
            {
            _db = db;
            }

        // Страница списка
        public async Task<IActionResult> Index()
            {
            var items = await _db.News
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();

            return View(items);
            }

        public IActionResult Create()
            {
            return View(new News());
            }

        //создание
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News model)
            {
            if (!ModelState.IsValid)
                return View(model);

            _db.News.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }


        // Редактирование
        public async Task<IActionResult> Edit(int id)
            {
            var item = await _db.News.FindAsync(id);
            if (item == null)
                return NotFound();
            return View(item);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News model)
            {
            var existing = await _db.News.FindAsync(id);
            if (existing == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            existing.TitleRu = model.TitleRu;
            existing.SubtitleRu = model.SubtitleRu;
            existing.TextRu = model.TextRu;
            existing.TitleEn = model.TitleEn;
            existing.SubtitleEn = model.SubtitleEn;
            existing.TextEn = model.TextEn;
            existing.IsPublished = model.IsPublished;
            existing.PublishedAt = model.PublishedAt;

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
        }
    }