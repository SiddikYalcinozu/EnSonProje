using EnSonProje.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EnSonProje.Controllers.Api;

[ApiController]
[Route("api")]
[Route("api/[controller]")]
public class HomeApiController : ControllerBase
{
    /// <summary>
    /// API durum bilgisini ve temel endpoint adreslerini dondurur.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiStatusResponse), StatusCodes.Status200OK)]
    public IActionResult Index()
    {
        return Ok(new ApiStatusResponse
        {
            Status = "ok",
            Message = "EnSonProje API calisiyor.",
            Timestamp = DateTime.UtcNow,
            Endpoints = new Dictionary<string, string>
            {
                ["swagger"] = "/swagger",
                ["kategoriler"] = "/api/kategoriler",
                ["kitaplar"] = "/api/kitaplar",
                ["siniflar"] = "/api/siniflar",
                ["ogrenciler"] = "/api/ogrenciler",
                ["emanetler"] = "/api/emanetler"
            }
        });
    }
}
