using EnSonProje.Contracts;
using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnSonProje.Controllers.Api;

[ApiController]
[Route("api/kategoriler")]
[Route("api/[controller]")]
public class KategoriApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public KategoriApiController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tum kategorileri listeler.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<KategoriDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<KategoriDto>>> GetAll()
    {
        var kategoriler = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .Select(k => new KategoriDto
            {
                Id = k.Id,
                Ad = k.Ad,
                KitapSayisi = k.Kitaplar != null ? k.Kitaplar.Count : 0
            })
            .ToListAsync();

        return kategoriler;
    }

    /// <summary>
    /// Id'ye gore kategori getirir.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(KategoriDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<KategoriDto>> GetById(int id)
    {
        var kategori = await _context.Kategoriler
            .Include(k => k.Kitaplar)
            .Where(k => k.Id == id)
            .Select(k => new KategoriDto
            {
                Id = k.Id,
                Ad = k.Ad,
                KitapSayisi = k.Kitaplar != null ? k.Kitaplar.Count : 0
            })
            .FirstOrDefaultAsync();

        if (kategori is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Kategori bulunamadi." });
        }

        return kategori;
    }

    /// <summary>
    /// Yeni kategori ekler.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(KategoriDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<KategoriDto>> Create([FromBody] KategoriUpsertRequest request)
    {
        var kategori = new Kategori
        {
            Ad = request.Ad.Trim()
        };

        _context.Kategoriler.Add(kategori);
        await _context.SaveChangesAsync();

        var response = new KategoriDto
        {
            Id = kategori.Id,
            Ad = kategori.Ad,
            KitapSayisi = 0
        };

        return CreatedAtAction(nameof(GetById), new { id = kategori.Id }, response);
    }

    /// <summary>
    /// Mevcut kategori bilgilerini gunceller.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] KategoriUpsertRequest request)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Kategori bulunamadi." });
        }

        kategori.Ad = request.Ad.Trim();
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Kategori kaydini siler.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var kategori = await _context.Kategoriler.FindAsync(id);
        if (kategori is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Kategori bulunamadi." });
        }

        _context.Kategoriler.Remove(kategori);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
