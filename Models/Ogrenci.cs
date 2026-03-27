namespace EnSonProje.Models;

public class Ogrenci
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string OkulNo { get; set; } = string.Empty;

    // Sınıf ile ilişki
    public int SinifId { get; set; } // Veri tabanında tutulacak ID
    public Sinif? Sinif { get; set; } // Sınıf bilgilerine erişmek için
}
