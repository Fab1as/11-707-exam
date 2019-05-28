using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Entity
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string PathToFile { get; set; }
        public string Link { get; set; }
        [MaxLength(30, ErrorMessage = "Максимум 30 символов")]
        public string Description { get; set; }
        public string FullDescription { get; set; }
        public DateTime UploadedDate { get; set; }
        public int DownloadedCount { get; set; }
        public string ContentType { get; set; }
        public string UserDownloadString { get; set; }
        public bool IsPassworded { get; set; }
    }
}
