using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImageApi.Models;
using System.IO;
using ImageProcessor.Imaging.Formats;
using System.Drawing;
using ImageProcessor;

namespace ImageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ImageContext _context;

        public ImagesController(ImageContext context)
        {
            _context = context;

            if (_context.Images.Count() == 0)
            {
                _context.Images.Add(new ImageData { ImageBytes = System.IO.File.ReadAllBytes("Beach.jpg") });
                _context.SaveChanges();
            }
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageData>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageData>> GetImage(long id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // GET: api/Images/5/Brightness/5
        [HttpGet("{id}/Brightness/{amount}")]
        public async Task<ActionResult<ImageData>> GetImageBrightness(long id,int amount)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            byte[] photoBytes = image.ImageBytes;
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(inStream)
                                    .Brightness(amount)
                                    .Save(outStream);
                    }
                    image.ImageBytes = outStream.ToArray();
                }
            }
            await _context.SaveChangesAsync();

            return image;
        }

        // GET: api/Images/5/Hue/5
        [HttpGet("{id}/Hue/{amount}")]
        public async Task<ActionResult<ImageData>> GetImageHue(long id, int amount)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            byte[] photoBytes = image.ImageBytes;
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        imageFactory.Load(inStream)
                                    .Hue(amount)
                                    .Save(outStream);
                    }
                    image.ImageBytes = outStream.ToArray();
                }
            }

            await _context.SaveChangesAsync();

            return image;
        }

        // GET: api/Images/5/Reset
        [HttpGet("{id}/Reset")]
        public async Task<ActionResult<ImageData>> GetImageReset(long id, int amount)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            image.ImageBytes = System.IO.File.ReadAllBytes("Beach.jpg");

            await _context.SaveChangesAsync();

            return image;
        }

        // PUT: api/Images/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(long id, ImageData image)
        {
            if (id != image.Id)
            {
                return BadRequest();
            }

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Images
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ImageData>> PostImage(ImageData image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImage", new { id = image.Id }, image);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImageData>> DeleteImage(long id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return image;
        }

        private bool ImageExists(long id)
        {
            return _context.Images.Any(e => e.Id == id);
        }


    }
}
