using Homestead.Slices.Rota.Services.Rota;
using Main.Global;
using Main.Global.Helpers;
using Main.Global.Library.AutoMapper;
using Main.Slices.Rota.Models.Base;
using Main.Slices.Rota.Models.Db;
using Main.Slices.Rota.Models.Dtos.In;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Homestead.Slices.Rota.Services.Rota.RotaService;

namespace Main.Slices.Rota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotaController : ControllerBase
    {
        private readonly IServiceManager _services;
        public RotaController(IServiceManager services) => _services = services;

        
        [HttpGet]
        [Route("scheduled")]
        public async Task<IActionResult> GetScheduled([FromQuery]string id)
        {
            return Ok(await _services.Rota.GetSchedule<SecurityUpcomingRota>(id));
        }


        [HttpGet]
        [Route("shifts")]
        public async Task<IActionResult> GetShifts([FromQuery] string id)
        {
            return Ok(await _services.Rota.GetShifts<ShiftModel>(id));
        }
        

        [HttpPut]
        [Route("schedule/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpcomingRota dto)
        {
            await _services.Rota.ManipulateSchedule(id, dto);
            return Ok();
        }


        [HttpPut]
        [Route("schedule/{id}")]
        public async Task<IActionResult> UpdateShift(string id, [FromBody] UpdateShiftDto dto)
        {
            await _services.Rota.ClubManipulateShifts(id, dto.Start, (x) => { x.Status = dto.status; return x; });
            return Ok();
        }

















        // --- Controls

        [HttpPost("control/{id}")]
        public async Task<IActionResult> Init(string id)
        {
            await _services.Rota.InitialiseRota(id);

            return Ok();
        }

        [HttpDelete("control/{id}")]
        public async Task<IActionResult> Del(string id)
        {

            await _services.Rota.DeleteRota(id);
            return NoContent();
        }


        [HttpPut("control/{id}")]
        public async Task<IActionResult> Inc(string id)
        {

            await _services.Rota.Increment(id);
            return NoContent();
        }
    }
}
