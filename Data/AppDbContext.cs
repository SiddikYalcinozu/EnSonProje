using Microsoft.EntityFrameworkCore;
using EnSonProje.Models;

namespace EnSonProje.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Kitap> Kitaplar { get; set; }

        public DbSet<Kategori> Kategoriler { get; set; }

        public DbSet<Ogrenci> Ogrenciler { get; set; }

        public DbSet<Emanet> Emanetler { get; set; }

      
public DbSet<Sinif> Siniflar { get; set; }
        
    }
}