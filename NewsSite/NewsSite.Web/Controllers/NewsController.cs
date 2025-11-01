 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Domain.Entities;

namespace NewsSite.Web.Controllers
    {
    public class NewsController : Controller
        {
        private readonly AppDbContext _db;
        public NewsController(AppDbContext db) => _db = db;

        // Страница списка
        public async Task<IActionResult> Index()
            {
            var items = await _db.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .Take(10)
                .ToListAsync();

            return View(items);
            }


        // Частичное представление для подгрузки новостей
        public async Task<IActionResult> List(int page = 1, int pageSize = 10)
            {
            var items = await _db.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .Skip(( page - 1 ) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (items.Count == 0)
                return Content(string.Empty);

            return PartialView("_NewsList", items);
            }

        // Детальная страница
        public async Task<IActionResult> Details(int id)
            {
            var item = await _db.News.FindAsync(id);
            if (item == null || !item.IsPublished)
                return NotFound();
            return View(item);
            }
        }
    }
