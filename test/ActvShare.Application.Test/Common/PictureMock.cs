using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;

namespace ActvShare.Application.Test.Common
{
    public static class PictureMock
    {
        public static IFormFile GetPicture()
        {
            var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
            IFormFile file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt");
            return file;
        }

    }
}
