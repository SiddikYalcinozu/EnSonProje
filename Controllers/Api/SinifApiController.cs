using EnSonProje.Contracts;
using EnSonProje.Data;
using EnSonProje.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnSonProje.Controllers.Api;

[ApiController]
[Route("api/siniflar")]
[Route("api/[controller]")]
public class SinifApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public SinifApiController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tum siniflari listeler.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SinifDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SinifDto>>> GetAll()
    {
        var siniflar = await _context.Siniflar
            .Select(s => new SinifDto
            {
                Id = s.Id,
                Ad = s.Ad
            })
            .ToListAsync();

        return siniflar;
    }

    /// <summary>
    /// Id'ye gore sinif getirir.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SinifDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SinifDto>> GetById(int id)
    {
        var sinif = await _context.Siniflar
            .Where(s => s.Id == id)
            .Select(s => new SinifDto
            {
                Id = s.Id,
                Ad = s.Ad
            })
            .FirstOrDefaultAsync();

        if (sinif is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Sinif bulunamadi." });
        }

        return sinif;
    }

    /// <summary>
    /// Yeni sinif ekler.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SinifDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SinifDto>> Create([FromBody] SinifUpsertRequest request)
    {
        var sinif = new Sinif
        {
            Ad = request.Ad.Trim()
        };

        _context.Siniflar.Add(sinif);
        await _context.SaveChangesAsync();

        var response = new SinifDto
        {
            Id = sinif.Id,
            Ad = sinif.Ad
        };

        return CreatedAtAction(nameof(GetById), new { id = sinif.Id }, response);
    }

    /// <summary>
    /// Sinif bilgisini gunceller.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] SinifUpsertRequest request)
    {
        var sinif = await _context.Siniflar.FindAsync(id);
        if (sinif is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Sinif bulunamadi." });
        }

        sinif.Ad = request.Ad.Trim();
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Sinif kaydini siler.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var sinif = await _context.Siniflar.FindAsync(id);
        if (sinif is null)
        {
            return NotFound(new ApiMessageResponse { Message = "Sinif bulunamadi." });
        }

        _context.Siniflar.Remove(sinif);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
