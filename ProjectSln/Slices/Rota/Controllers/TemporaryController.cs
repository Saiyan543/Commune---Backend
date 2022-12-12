using Main.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Rota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemporaryController : ControllerBase
    {
        private readonly IDailyRotaUpdate _services;

        public TemporaryController(IDailyRotaUpdate services) => _services = services;

        [HttpPut]
        public IActionResult Init()
        {
            _services.Init();
            return NoContent();
        }

    }
}
