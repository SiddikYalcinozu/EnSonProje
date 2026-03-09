using System.ComponentModel.DataAnnotations;

namespace EnSonProje.Models
{
    public class Kitap
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap adı boş bırakılamaz.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Kitap adı 2 ile 100 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9ğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Kitap adı sadece harf ve sayı içerebilir.")]
        public string Ad { get; set; } = null!;

        [Required(ErrorMessage = "Yazar adı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Yazar adı en fazla 50 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Yazar adı sadece harf içerebilir.")]
        public string Yazar { get; set; } = null!;

        [Required(ErrorMessage = "Yayınevi adı boş bırakılamaz.")]
        [StringLength(50, ErrorMessage = "Yayınevi adı en fazla 50 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Yayınevi adı sadece harf içerebilir.")]
        public string YayınEvi { get; set; } = null!;

        [Required(ErrorMessage = "Lütfen bir kategori seçiniz.")]
        public int KategoriId { get; set; }

        
        public virtual Kategori? Kategori { get; set; }
    }
}