using System.ComponentModel.DataAnnotations;

namespace EnSonProje.Models
{
    public class Kategori
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [StringLength(30, ErrorMessage = "Kategori adı en fazla 30 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Kategori adı sadece harf içerebilir.")]
        public string Ad { get; set; } = null!;

        // Bir kategoride birden fazla kitap olabilir
        public virtual ICollection<Kitap>? Kitaplar { get; set; }
    }
}