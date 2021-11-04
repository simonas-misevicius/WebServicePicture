using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ImageApi.Models
{
    public class ImageData
    {
        public long Id { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
