using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnSonProje.Models;
using EnSonProje.Data; // Model namespace'inizi kontrol edin

namespace EnSonProje.Controllers
{
    public class SinifController : Controller
    {
        private readonly AppDbContext _context; // Context adınız RouteDbContext ise öyle güncelleyin

        public SinifController(AppDbContext context)
        {
            _context = context;
        }

        // LİSTELEME
        public async Task<IActionResult> Index()
        {
            var siniflar = await _context.Siniflar.ToListAsync();
            return View(siniflar);
        }

        // EKLEME (Sayfayı Aç)
        [HttpGet]
        public IActionResult Ekle() => View();

        // EKLEME (Kaydet)
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Ekle(Sinif sinif)
{
    if (ModelState.IsValid)
    {
        // 1. Sınıfı veritabanına ekle
        _context.Siniflar.Add(sinif);
        
        // 2. Değişiklikleri kaydet
        await _context.SaveChangesAsync();
        
        // 3. KRİTİK NOKTA: Burası seni doğrudan listeye (Index) gönderir
        return RedirectToAction(nameof(Index)); 
    }

    // Eğer bir hata varsa (boş bırakıldıysa vs.) sayfada kalır ve hataları gösterir
    return View(sinif);
}

        // GÜNCELLEME (Sayfayı Aç)
        [HttpGet]
        public async Task<IActionResult> Guncelle(int? id)
        {
            if (id == null) return NotFound();
            var sinif = await _context.Siniflar.FindAsync(id);
            if (sinif == null) return NotFound();
            return View(sinif);
        }

        // GÜNCELLEME (Kaydet)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guncelle(int id, Sinif sinif)
        {
            if (id != sinif.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(sinif);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sinif);
        }

        // SİLME
        public async Task<IActionResult> Sil(int id)
        {
            var sinif = await _context.Siniflar.FindAsync(id);
            if (sinif != null)
            {
                _context.Siniflar.Remove(sinif);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}