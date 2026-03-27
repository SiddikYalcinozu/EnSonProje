using System.ComponentModel.DataAnnotations;

namespace EnSonProje.Contracts;

public sealed class ApiMessageResponse
{
    public string Message { get; set; } = string.Empty;
}

public sealed class ApiStatusResponse
{
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Dictionary<string, string> Endpoints { get; set; } = new();
}

public sealed class KategoriDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public int KitapSayisi { get; set; }
}

public sealed class KategoriUpsertRequest
{
    [Required(ErrorMessage = "Kategori adi bos birakilamaz.")]
    [StringLength(30, ErrorMessage = "Kategori adi en fazla 30 karakter olabilir.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Kategori adi sadece harf icerebilir.")]
    public string Ad { get; set; } = string.Empty;
}

public sealed class KitapDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Yazar { get; set; } = string.Empty;
    public string YayinEvi { get; set; } = string.Empty;
    public int KategoriId { get; set; }
    public string? KategoriAd { get; set; }
}

public sealed class KitapUpsertRequest
{
    [Required(ErrorMessage = "Kitap adi bos birakilamaz.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Kitap adi 2 ile 100 karakter arasinda olmalidir.")]
    [RegularExpression(@"^[a-zA-Z0-9ğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Kitap adi sadece harf ve sayi icerebilir.")]
    public string Ad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yazar adi bos birakilamaz.")]
    [StringLength(50, ErrorMessage = "Yazar adi en fazla 50 karakter olabilir.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Yazar adi sadece harf icerebilir.")]
    public string Yazar { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yayinevi adi bos birakilamaz.")]
    [StringLength(50, ErrorMessage = "Yayinevi adi en fazla 50 karakter olabilir.")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]*$", ErrorMessage = "Yayinevi adi sadece harf icerebilir.")]
    public string YayinEvi { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Gecerli bir kategori seciniz.")]
    public int KategoriId { get; set; }
}

public sealed class SinifDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
}

public sealed class SinifUpsertRequest
{
    [Required(ErrorMessage = "Sinif adi bos birakilamaz.")]
    [StringLength(20, ErrorMessage = "Sinif adi en fazla 20 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;
}

public sealed class OgrenciDto
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string OkulNo { get; set; } = string.Empty;
    public int SinifId { get; set; }
    public string? SinifAd { get; set; }
}

public sealed class OgrenciUpsertRequest
{
    [Required(ErrorMessage = "Ogrenci adi bos birakilamaz.")]
    [StringLength(50, ErrorMessage = "Ogrenci adi en fazla 50 karakter olabilir.")]
    public string Ad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ogrenci soyadi bos birakilamaz.")]
    [StringLength(50, ErrorMessage = "Ogrenci soyadi en fazla 50 karakter olabilir.")]
    public string Soyad { get; set; } = string.Empty;

    [Required(ErrorMessage = "Okul numarasi bos birakilamaz.")]
    [StringLength(20, ErrorMessage = "Okul numarasi en fazla 20 karakter olabilir.")]
    public string OkulNo { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Gecerli bir sinif seciniz.")]
    public int SinifId { get; set; }
}

public sealed class EmanetDto
{
    public int Id { get; set; }
    public int OgrenciId { get; set; }
    public string? OgrenciAd { get; set; }
    public int KitapId { get; set; }
    public string? KitapAd { get; set; }
    public DateTime VerilisTarihi { get; set; }
    public DateTime? TeslimTarihi { get; set; }
    public bool TeslimEdildi { get; set; }
}

public sealed class EmanetCreateRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Gecerli bir ogrenci seciniz.")]
    public int OgrenciId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Gecerli bir kitap seciniz.")]
    public int KitapId { get; set; }

    public DateTime? VerilisTarihi { get; set; }
}
