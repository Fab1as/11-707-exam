using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Entity
{
    public class PasswordedFileModel : FileModel
    {
        public string Password { get; set; }
    }
}
