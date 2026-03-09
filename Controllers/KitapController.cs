using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class KitapController : Controller
{
    private readonly AppDbContext _context;
    public KitapController(AppDbContext context) { _context = context; }

    public async Task<IActionResult> Index() => 
        View(await _context.Kitaplar.Include(k => k.Kategori).ToListAsync());

    [HttpGet]
    public IActionResult Ekle() {
        ViewBag.Kategoriler = _context.Kategoriler.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Ekle(Kitap kitap) {
        if (ModelState.IsValid) {
            _context.Add(kitap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Kategoriler = _context.Kategoriler.ToList();
        return View(kitap);
    }

    [HttpGet]
    public async Task<IActionResult> Guncelle(int id) {
        var kitap = await _context.Kitaplar.FindAsync(id);
        ViewBag.Kategoriler = _context.Kategoriler.ToList();
        return View(kitap);
    }

    [HttpPost]
    public async Task<IActionResult> Guncelle(Kitap kitap) {
        if (ModelState.IsValid) {
            _context.Update(kitap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Kategoriler = _context.Kategoriler.ToList();
        return View(kitap);
    }

    public async Task<IActionResult> Sil(int id) {
        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap != null) {
            _context.Kitaplar.Remove(kitap);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}