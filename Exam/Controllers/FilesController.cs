using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exam.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{ 
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ApplicationDbContext _dbContext;

        public FilesController(ApplicationDbContext dbContext,
            IHostingEnvironment appEnvironment)
        {

            _appEnvironment = appEnvironment;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(model: _dbContext.Files.ToList());
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View(model: new FileModel());
        }

        [HttpPost]
        public async Task<IActionResult> Upload(FileModel filesInfo, IFormFile file)
        {
            var path = _appEnvironment.WebRootPath + "/Files";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
            }

            if (_dbContext.Files.Any(x => x.UserDownloadString == filesInfo.UserDownloadString))
            {
                return View(model: filesInfo);
            }

            if (file != null)
            {
                filesInfo.Slug = Guid.NewGuid().ToString() + file.FileName;
                filesInfo.PathToFile = "/Files/" + "_" + filesInfo.Slug;
                filesInfo.FileName = file.FileName;
                filesInfo.Extension = file.FileName.Split('.').LastOrDefault();
                filesInfo.UploadedDate = DateTime.Now;
                filesInfo.ContentType = file.ContentType;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filesInfo.PathToFile, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                _dbContext.Files.Add(filesInfo);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var file = _dbContext.Files.FirstOrDefault(x => x.Id == id);
            return View(model: file);
        }

        [HttpGet]
        public async Task<IActionResult> Download(string slug)
        {
            var file = _dbContext.Files
                 .Where(x => x.Slug == slug || x.UserDownloadString == slug)
                 .FirstOrDefault();
            if (file != null)
            {
                var path = _appEnvironment.WebRootPath + file.PathToFile;
                file.DownloadedCount++;
                await _dbContext.SaveChangesAsync();
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, file.ContentType, fileDownloadName: file.FileName);
            }
            return NotFound();
        }
    }
}