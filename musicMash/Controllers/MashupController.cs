using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using musicMash.Exceptions;
using musicMash.Services;

namespace musicMash.Controllers
{
    [Route("api/mashup")]
    public class MashupController : Controller
    {
        readonly IMashupService _mashupService;

        public MashupController(IMashupService mashupService)
        {
            _mashupService = mashupService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var response = await _mashupService.GetMashup(id);
                return Ok(response);
            }
            catch (ArtistDoesNotExistException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
