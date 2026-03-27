using EnSonProje.Contracts;
using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnSonProje.Controllers.Api;

[ApiController]
[Route("api/kitaplar")]
[Route("api/[controller]")]
public class KitapApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public KitapApiController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tum kitaplari kategori bilgisiyle birlikte listeler.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<KitapDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KitapDto>>> GetAll()
    {
        var kitaplar = await _context.Kitaplar
            .Include(k => k.Kategori)
            .Select(k => new KitapDto
            {
                Id = k.Id,
                Ad = k.Ad,
                Yazar = k.Yazar,
                YayinEvi = k.YayınEvi,
                KategoriId = k.KategoriId,
                KategoriAd = k.Kategori != null ? k.Kategori.Ad : null
            })
            .ToListAsync();

        return kitaplar;
    }

    /// <summary>
    /// Id'ye gore kitap getirir.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(KitapDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KitapDto>> GetById(int id)
    {
        var kitap = await _context.Kitaplar
            .Include(k => k.Kategori)
            .Where(k => k.Id == id)
            .Select(k => new KitapDto
            {
                Id = k.Id,
                Ad = k.Ad,
                Yazar = k.Yazar,
                YayinEvi = k.YayınEvi,
                KategoriId = k.KategoriId,
                KategoriAd = k.Kategori != null ? k.Kategori.Ad : null
            })
            .FirstOrDefaultAsync();

        if (kitap is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Kitap bulunamadi." });
        }

        return kitap;
    }

    /// <summary>
    /// Yeni kitap ekler.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(KitapDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<KitapDto>> Create([FromBody] KitapUpsertRequest request)
    {
        var kategoriVar = await _context.Kategoriler.AnyAsync(k => k.Id == request.KategoriId);
        if (!kategoriVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Gecersiz kategori." });
        }

        var kitap = new Kitap
        {
            Ad = request.Ad.Trim(),
            Yazar = request.Yazar.Trim(),
            YayınEvi = request.YayinEvi.Trim(),
            KategoriId = request.KategoriId
        };

        _context.Kitaplar.Add(kitap);
        await _context.SaveChangesAsync();

        var response = await _context.Kitaplar
            .Include(k => k.Kategori)
            .Where(k => k.Id == kitap.Id)
            .Select(k => new KitapDto
            {
                Id = k.Id,
                Ad = k.Ad,
                Yazar = k.Yazar,
                YayinEvi = k.YayınEvi,
                KategoriId = k.KategoriId,
                KategoriAd = k.Kategori != null ? k.Kategori.Ad : null
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = kitap.Id }, response);
    }

    /// <summary>
    /// Kitap bilgisini gunceller.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] KitapUpsertRequest request)
    {
        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Kitap bulunamadi." });
        }

        var kategoriVar = await _context.Kategoriler.AnyAsync(k => k.Id == request.KategoriId);
        if (!kategoriVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Gecersiz kategori." });
        }

        kitap.Ad = request.Ad.Trim();
        kitap.Yazar = request.Yazar.Trim();
        kitap.YayınEvi = request.YayinEvi.Trim();
        kitap.KategoriId = request.KategoriId;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Kitap kaydini siler.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var kitap = await _context.Kitaplar.FindAsync(id);
        if (kitap is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Kitap bulunamadi." });
        }

        _context.Kitaplar.Remove(kitap);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
