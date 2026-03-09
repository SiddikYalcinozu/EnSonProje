using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class KategoriController : Controller
{
    private readonly AppDbContext _context;

    public KategoriController(AppDbContext context)
    {
        _context = context;
    }

    // Index - Kategori listesi
    public async Task<IActionResult> Index()
    {
        var kategoriler = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .ToListAsync();
        return View(kategoriler);
    }

    // Create - Ekleme sayfası
    public IActionResult Ekle()
    {
        return View();
    }

    // Create - Ekleme işlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ekle(Kategori kategori)
    {
        if (ModelState.IsValid)
        {
            _context.Kategoriler.Add(kategori);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    // Edit - Güncelleme sayfası
    public async Task<IActionResult> Güncelle(int? id)
    {
        if (id == null) return NotFound();

        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori == null) return NotFound();

        return View(kategori);
    }

    // Edit - Güncelleme işlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Güncelle(int id, Kategori kategori)
    {
        if (id != kategori.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(kategori);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategoriExists(kategori.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(kategori);
    }

    // Delete - Silme
    public async Task<IActionResult> Sil(int? id)
    {
        if (id == null) return NotFound();

        var kategori = await _context.Kategoriler
            .FirstOrDefaultAsync(m => m.Id == id);
        if (kategori == null) return NotFound();

        _context.Kategoriler.Remove(kategori);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Helper
    private bool KategoriExists(int id)
    {
        return _context.Kategoriler.Any(e => e.Id == id);
    }
}