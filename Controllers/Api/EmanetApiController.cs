using EnSonProje.Contracts;
using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnSonProje.Controllers.Api;

[ApiController]
[Route("api/emanetler")]
[Route("api/[controller]")]
public class EmanetApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public EmanetApiController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tum emanet kayitlarini listeler.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EmanetDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EmanetDto>>> GetAll()
    {
        var emanetler = await _context.Emanetler
            .Include(e => e.Ogrenci)
            .Include(e => e.Kitap)
            .Select(e => new EmanetDto
            {
                Id = e.Id,
                OgrenciId = e.OgrenciId,
                OgrenciAd = e.Ogrenci != null ? e.Ogrenci.Ad + " " + e.Ogrenci.Soyad : null,
                KitapId = e.KitapId,
                KitapAd = e.Kitap != null ? e.Kitap.Ad : null,
                VerilisTarihi = e.VerilisTarihi,
                TeslimTarihi = e.TeslimTarihi,
                TeslimEdildi = e.TeslimTarihi != null
            })
            .ToListAsync();

        return emanetler;
    }

    /// <summary>
    /// Id'ye gore emanet kaydi getirir.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmanetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmanetDto>> GetById(int id)
    {
        var emanet = await _context.Emanetler
            .Include(e => e.Ogrenci)
            .Include(e => e.Kitap)
            .Where(e => e.Id == id)
            .Select(e => new EmanetDto
            {
                Id = e.Id,
                OgrenciId = e.OgrenciId,
                OgrenciAd = e.Ogrenci != null ? e.Ogrenci.Ad + " " + e.Ogrenci.Soyad : null,
                KitapId = e.KitapId,
                KitapAd = e.Kitap != null ? e.Kitap.Ad : null,
                VerilisTarihi = e.VerilisTarihi,
                TeslimTarihi = e.TeslimTarihi,
                TeslimEdildi = e.TeslimTarihi != null
            })
            .FirstOrDefaultAsync();

        if (emanet is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Emanet kaydi bulunamadi." });
        }

        return emanet;
    }

    /// <summary>
    /// Yeni emanet kaydi olusturur.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EmanetDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmanetDto>> Create([FromBody] EmanetCreateRequest request)
    {
        var ogrenciVar = await _context.Ogrenciler.AnyAsync(o => o.Id == request.OgrenciId);
        if (!ogrenciVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Gecersiz ogrenci." });
        }

        var kitapVar = await _context.Kitaplar.AnyAsync(k => k.Id == request.KitapId);
        if (!kitapVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Gecersiz kitap." });
        }

        var aktifEmanetVar = await _context.Emanetler
            .AnyAsync(e => e.KitapId == request.KitapId && e.TeslimTarihi == null);
        if (aktifEmanetVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Bu kitap zaten teslim edilmemis bir emanet kaydina sahip." });
        }

        var emanet = new Emanet
        {
            OgrenciId = request.OgrenciId,
            KitapId = request.KitapId,
            VerilisTarihi = request.VerilisTarihi ?? DateTime.Now
        };

        _context.Emanetler.Add(emanet);
        await _context.SaveChangesAsync();

        var response = await _context.Emanetler
            .Include(e => e.Ogrenci)
            .Include(e => e.Kitap)
            .Where(e => e.Id == emanet.Id)
            .Select(e => new EmanetDto
            {
                Id = e.Id,
                OgrenciId = e.OgrenciId,
                OgrenciAd = e.Ogrenci != null ? e.Ogrenci.Ad + " " + e.Ogrenci.Soyad : null,
                KitapId = e.KitapId,
                KitapAd = e.Kitap != null ? e.Kitap.Ad : null,
                VerilisTarihi = e.VerilisTarihi,
                TeslimTarihi = e.TeslimTarihi,
                TeslimEdildi = e.TeslimTarihi != null
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = emanet.Id }, response);
    }

    /// <summary>
    /// Emanet kaydini teslim alindi olarak isaretler.
    /// </summary>
    [HttpPut("{id:int}/teslim-al")]
    [ProducesResponseType(typeof(EmanetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TeslimAl(int id)
    {
        var emanet = await _context.Emanetler.FindAsync(id);
        if (emanet is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Emanet kaydi bulunamadi." });
        }

        if (emanet.TeslimTarihi != null)
        {
            return BadRequest(new ApiMessageResponse { Message = "Bu emanet zaten teslim alinmis." });
        }

        emanet.TeslimTarihi = DateTime.Now;
        await _context.SaveChangesAsync();

        var response = await _context.Emanetler
            .Include(e => e.Ogrenci)
            .Include(e => e.Kitap)
            .Where(e => e.Id == id)
            .Select(e => new EmanetDto
            {
                Id = e.Id,
                OgrenciId = e.OgrenciId,
                OgrenciAd = e.Ogrenci != null ? e.Ogrenci.Ad + " " + e.Ogrenci.Soyad : null,
                KitapId = e.KitapId,
                KitapAd = e.Kitap != null ? e.Kitap.Ad : null,
                VerilisTarihi = e.VerilisTarihi,
                TeslimTarihi = e.TeslimTarihi,
                TeslimEdildi = e.TeslimTarihi != null
            })
            .FirstAsync();

        return Ok(response);
    }

    /// <summary>
    /// Emanet kaydini siler.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var emanet = await _context.Emanetler.FindAsync(id);
        if (emanet is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Emanet kaydi bulunamadi." });
        }

        _context.Emanetler.Remove(emanet);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
