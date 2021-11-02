using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ImageApi.Models
{
    public class ImageData
    {
        public long Id { get; set; }
        public byte[] ImageBytes { get; set; }
        //public File ImgaeFile { get; set; }
    }
}
