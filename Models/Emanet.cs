using System.ComponentModel.DataAnnotations;

namespace EnSonProje.Models
{
    public class Emanet
    {
        public int Id { get; set; }

        [Display(Name = "Öğrenci")]
        [Range(1, int.MaxValue, ErrorMessage = "Gecerli bir ogrenci seciniz.")]
        public int OgrenciId { get; set; }
        public Ogrenci? Ogrenci { get; set; }

        [Display(Name = "Kitap")]
        [Range(1, int.MaxValue, ErrorMessage = "Gecerli bir kitap seciniz.")]
        public int KitapId { get; set; }
        public Kitap? Kitap { get; set; }

        [Display(Name = "Veriliş Tarihi")]
        public DateTime VerilisTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Teslim Tarihi")]
        public DateTime? TeslimTarihi { get; set; } // Boşsa kitap henüz dönmemiş demektir
    }
}
