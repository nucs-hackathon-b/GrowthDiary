using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GrowthDiary.Data;
using GrowthDiary.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using GrowthDiary.ViewModels;
using System.Drawing;
using GrowthDiary.Common;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace GrowthDiary.Controllers
{
    public class PostsController : Controller
    {
        private readonly GrowthDiaryContext _context;
        private readonly ILogger<PostsController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostsController(GrowthDiaryContext context, ILogger<PostsController> logger, IWebHostEnvironment environment,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Posts
        public async Task<IActionResult> Index(String search, bool ascending)
        {
            return View(await SearchPosts(search).ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var query = from p in _context.Post
                                    .Include(p => p.Replies)
                                    .Include(p => p.InReplyTo)
                                    .ThenInclude(p => p.Replies)
                                    .Include(p => p.InReplyTo)
                                    .Include(p => p.Images)
                        where p.Id == id
                        select p;
            var post = query.FirstOrDefault();
            if (post is null)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["InputModel"] = new PostInputModel()
            {
                InReplyToId = id
            };
            return View(post);

            //var post = await _context.Post
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (post == null)
            //{
            //    return NotFound();
            //}

            //return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View(new PostInputModel());
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] PostInputModel inputModel)
        {
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"); // For Windows
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");  // For Linux (Docker)
            if (ModelState.IsValid)
            {
                var post = new Post()
                {
                    Content = inputModel.Content,
                    InReplyToId = inputModel.InReplyToId,
                    CreationTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo),
                    LastModifiedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo)
                };
                _context.Post.Add(post);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{inputModel.Files}");
                post = _context.Post.Where(p => p.Id == post.Id).Include(p => p.Images).Single();
                // Upload image files
                if (inputModel.Files != null)
                {
                    foreach (var file in inputModel.Files)
                    {
                        _logger.LogInformation($"FileName = {file.FileName}");
                    }
                    for (var i = 0; i < inputModel.Files.Count; ++i)
                    {
                        var file = inputModel.Files[i];
                        Image image;
                        using (var stream = file.OpenReadStream())
                        {
                            image = Image.FromStream(stream); // Read image
                        }
                        var extension = image.RawFormat.GetFileExtension();
                        var fileName = extension.GenerateRandomFileName();
                        var path = Path.Combine(_environment.WebRootPath, _configuration["PostImagesPath"], fileName);
                        image.Save(path, image.RawFormat);
                        var url = Path.Combine("/", _configuration["PostImagesPath"], fileName);
                        post.Images.Add(new PostImage()
                        {
                            Url = url,
                            Index = i
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = from p in _context.Post.Include(p => p.Images) where p.Id == id select p;
            var post = await query.SingleOrDefaultAsync();
            if (post is null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind] Post post)
        {
            _logger.LogInformation(post.Id.ToString());
            _logger.LogInformation(post.Content);

            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var query = from pst in _context.Post where pst.Id == id select pst;
                    var p = await query.SingleAsync();
                    if (p != null)
                    {
                        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
                        p.LastModifiedTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
                        p.Content = post.Content;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Details", new { id });
                    }
                    return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var post = await _context.Post.FindAsync(id);
            var query = from p in _context.Post.Include(p => p.Images)
                        where p.Id == id
                        select p;
            var post = await query.SingleOrDefaultAsync();
            if (post != null)
            {
                foreach (var image in post.Images)
                {
                    var path = Path.Combine(_environment.WebRootPath, image.Url.Substring(1));
                    System.IO.File.Delete(path);
                }
                _context.Post.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
        
        private IQueryable<Post> SearchPosts(String search)
        {
            var posts = _context.Post.AsQueryable();
            if (!String.IsNullOrEmpty(search))
                posts = posts.Where(e => e.Content.Contains(search));

            var ascending = GetAscending();

            if (ascending)
                posts = posts.OrderBy(e => e.CreationTime);
            else
                posts = posts.OrderByDescending(e => e.CreationTime);
            return posts;
        }

        private Boolean GetAscending()
        {
            var currentAscending = _httpContextAccessor.HttpContext.Request.Cookies["ascending"];
            var ascendBool = false;
            if (!String.IsNullOrEmpty(currentAscending))
                ascendBool = bool.Parse(currentAscending);
            return ascendBool;
        }

        public IActionResult ToggleOrder()
        {
            var ascendBool = GetAscending();
            WriteCookie("ascending", (!ascendBool).ToString(), true);
            return RedirectToAction("Index");
        }

        private void WriteCookie(string key, string value, bool isPersistent)
        {
            CookieOptions options = new CookieOptions();
            if (isPersistent)
                options.Expires = DateTime.Now.AddDays(1);
            else
                options.Expires = DateTime.Now.AddSeconds(10);
            _httpContextAccessor.HttpContext.Response.Cookies.Append
            (key, value, options);
        }
    }
}
