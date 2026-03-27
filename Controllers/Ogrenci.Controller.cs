using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnSonProje.Models;
using EnSonProje.Data; // Namespace adını projenle eşleştir

namespace EnSonProje.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly AppDbContext _context; // Context adın farklıysa (örn: RouteDbContext) değiştir

        public OgrenciController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME (Index)
        public async Task<IActionResult> Index()
        {
            // .Include(x => x.Sinif) ile öğrencinin sınıf adını da çekiyoruz
            var ogrenciler = await _context.Ogrenciler
                .Include(x => x.Sinif)
                .ToListAsync();
            return View(ogrenciler);
        }

        // 2. EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Ekle()
        {
            // Sınıfları dropdown listesi için hazırlıyoruz
            ViewBag.SinifListesi = new SelectList(_context.Siniflar, "Id", "Ad");
            return View();
        }

        // 3. EKLEME İŞLEMİ (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ekle(Ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ogrenci);
                await _context.SaveChangesAsync();
                
                // Başarıyla kaydedince Index sayfasına (listeye) döner
                return RedirectToAction(nameof(Index)); 
            }

            // Hata varsa sınıfları tekrar yükle ki dropdown boş kalmasın
            ViewBag.SinifListesi = new SelectList(_context.Siniflar, "Id", "Ad", ogrenci.SinifId);
            return View(ogrenci);
        }

        // 4. GÜNCELLEME SAYFASI (GET)
        [HttpGet]
        public async Task<IActionResult> Guncelle(int? id)
        {
            if (id == null) return NotFound();

            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci == null) return NotFound();

            // Güncelleme sayfasında mevcut sınıfın seçili gelmesini sağlar
            ViewBag.SinifListesi = new SelectList(_context.Siniflar, "Id", "Ad", ogrenci.SinifId);
            return View(ogrenci);
        }

        // 5. GÜNCELLEME İŞLEMİ (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Guncelle(int id, Ogrenci ogrenci)
        {
            if (id != ogrenci.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(ogrenci);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SinifListesi = new SelectList(_context.Siniflar, "Id", "Ad", ogrenci.SinifId);
            return View(ogrenci);
        }

        // 6. SİLME İŞLEMİ
        public async Task<IActionResult> Sil(int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if (ogrenci != null)
            {
                _context.Ogrenciler.Remove(ogrenci);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}