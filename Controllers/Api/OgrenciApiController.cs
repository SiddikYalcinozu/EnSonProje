using EnSonProje.Contracts;
using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnSonProje.Controllers.Api;

[ApiController]
[Route("api/ogrenciler")]
[Route("api/[controller]")]
public class OgrenciApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public OgrenciApiController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tum ogrencileri sinif bilgisiyle birlikte listeler.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OgrenciDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OgrenciDto>>> GetAll()
    {
        var ogrenciler = await _context.Ogrenciler
            .Include(o => o.Sinif)
            .Select(o => new OgrenciDto
            {
                Id = o.Id,
                Ad = o.Ad,
                Soyad = o.Soyad,
                OkulNo = o.OkulNo,
                SinifId = o.SinifId,
                SinifAd = o.Sinif != null ? o.Sinif.Ad : null
            })
            .ToListAsync();

        return ogrenciler;
    }

    /// <summary>
    /// Id'ye gore ogrenci getirir.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OgrenciDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OgrenciDto>> GetById(int id)
    {
        var ogrenci = await _context.Ogrenciler
            .Include(o => o.Sinif)
            .Where(o => o.Id == id)
            .Select(o => new OgrenciDto
            {
                Id = o.Id,
                Ad = o.Ad,
                Soyad = o.Soyad,
                OkulNo = o.OkulNo,
                SinifId = o.SinifId,
                SinifAd = o.Sinif != null ? o.Sinif.Ad : null
            })
            .FirstOrDefaultAsync();

        if (ogrenci is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Ogrenci bulunamadi." });
        }

        return ogrenci;
    }

    /// <summary>
    /// Yeni ogrenci ekler.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OgrenciDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OgrenciDto>> Create([FromBody] OgrenciUpsertRequest request)
    {
        var sinifVar = await _context.Siniflar.AnyAsync(s => s.Id == request.SinifId);
        if (!sinifVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Gecersiz sinif." });
        }

        var ogrenci = new Ogrenci
        {
            Ad = request.Ad.Trim(),
            Soyad = request.Soyad.Trim(),
            OkulNo = request.OkulNo.Trim(),
            SinifId = request.SinifId
        };

        _context.Ogrenciler.Add(ogrenci);
        await _context.SaveChangesAsync();

        var response = await _context.Ogrenciler
            .Include(o => o.Sinif)
            .Where(o => o.Id == ogrenci.Id)
            .Select(o => new OgrenciDto
            {
                Id = o.Id,
                Ad = o.Ad,
                Soyad = o.Soyad,
                OkulNo = o.OkulNo,
                SinifId = o.SinifId,
                SinifAd = o.Sinif != null ? o.Sinif.Ad : null
            })
            .FirstAsync();

        return CreatedAtAction(nameof(GetById), new { id = ogrenci.Id }, response);
    }

    /// <summary>
    /// Ogrenci bilgisini gunceller.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] OgrenciUpsertRequest request)
    {
        var ogrenci = await _context.Ogrenciler.FindAsync(id);
        if (ogrenci is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Ogrenci bulunamadi." });
        }

        var sinifVar = await _context.Siniflar.AnyAsync(s => s.Id == request.SinifId);
        if (!sinifVar)
        {
            return BadRequest(new ApiMessageResponse { Message = "Gecersiz sinif." });
        }

        ogrenci.Ad = request.Ad.Trim();
        ogrenci.Soyad = request.Soyad.Trim();
        ogrenci.OkulNo = request.OkulNo.Trim();
        ogrenci.SinifId = request.SinifId;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Ogrenci kaydini siler.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var ogrenci = await _context.Ogrenciler.FindAsync(id);
        if (ogrenci is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Ogrenci bulunamadi." });
        }

        _context.Ogrenciler.Remove(ogrenci);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
