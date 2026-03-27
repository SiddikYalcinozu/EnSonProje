using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnSonProje.Models;
using EnSonProje.Data;

public class EmanetController : Controller
{
    private readonly AppDbContext _context;

    public EmanetController(AppDbContext context)
    {
        _context = context;
    }

    // LİSTELEME
    public async Task<IActionResult> Index()
    {
        var emanetler = await _context.Emanetler
            .Include(e => e.Ogrenci)
            .Include(e => e.Kitap)
            .ToListAsync();
        return View(emanetler);
    }

    // KİTAP VER (GET)
    public IActionResult Ver()
    {
        ViewBag.OgrenciListesi = new SelectList(_context.Ogrenciler, "Id", "Ad"); // Soyad eklemek istersen Model'de birleştirmen gerekir
        ViewBag.KitapListesi = new SelectList(_context.Kitaplar, "Id", "Ad");
        return View();
    }

    // KİTAP VER (POST)
    [HttpPost]
    public async Task<IActionResult> Ver(Emanet emanet)
    {
        if (ModelState.IsValid)
        {
            _context.Add(emanet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(emanet);
    }

    // KİTABI GERİ AL
    public async Task<IActionResult> TeslimAl(int id)
    {
        var emanet = await _context.Emanetler.FindAsync(id);
        if (emanet != null)
        {
            emanet.TeslimTarihi = DateTime.Now;
            _context.Update(emanet);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}