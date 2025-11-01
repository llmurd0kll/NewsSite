 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsSite.Data;
using NewsSite.Domain.Entities;

namespace NewsSite.Web.Controllers
    {
    public class NewsController : Controller
        {
        private readonly AppDbContext _context;
        private const int PageSize = 4;
        public NewsController(AppDbContext context)
            {
            _context = context;
            }

        // Страница списка
        public IActionResult Index()
            {
            var news = _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .Take(PageSize)
                .ToList();

            var total = _context.News.Count(n => n.IsPublished);
            ViewBag.TotalCount = total;
            ViewBag.PageSize = PageSize;

            return View(news);
            }


        // Частичное представление для подгрузки новостей
        public IActionResult List(int skip, int take = PageSize)
            {
            var news = _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedAt)
                .Skip(skip)
                .Take(take)
                .ToList();

            return PartialView("_NewsCardList", news);
            }

        // Детальная страница
        public async Task<IActionResult> Details(int id)
            {
            var item = await _context.News.FindAsync(id);
            if (item == null || !item.IsPublished)
                return NotFound();
            return View(item);
            }
        }
    }
