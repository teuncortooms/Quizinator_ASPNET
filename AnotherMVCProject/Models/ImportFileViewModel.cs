using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnotherMVCProject.Models
{
    public class ImportFileViewModel
    {
        public IFormFile MyFile { set; get; }
    }
}
