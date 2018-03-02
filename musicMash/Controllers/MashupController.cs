using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get(string id = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("You need to supply an artist id.");
            }
            try
            {
                var response = await _mashupService.GetMashup(id);
                return Ok(response);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(500);
            }
        }
    }
}
