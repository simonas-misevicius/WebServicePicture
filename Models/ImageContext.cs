using Microsoft.EntityFrameworkCore;

namespace ImageApi.Models
{
    public class ImageContext : DbContext
    {
        public ImageContext(DbContextOptions<ImageContext> options)
            : base(options)
        {
        }

        public DbSet<ImageData> Images { get; set; }
    }
}
