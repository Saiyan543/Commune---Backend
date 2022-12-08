using Main.Global;
using Main.Global.Helpers;
using Main.Slices.Rota.Models.Rota;
using Microsoft.AspNetCore.Mvc;

namespace Main.Slices.Rota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class RotaController : ControllerBase
    {
        private readonly IServiceManager _services;

        public RotaController(IServiceManager services) => _services = services;

        [HttpGet]
        [Route("scheduled/security")]
        public async Task<IActionResult> GetScheduled([FromQuery] string id)
        {
            var result = await _services.Rota.GetSchedule<Schedule_Security>(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("scheduled/club")]
        public async Task<IActionResult> GetScheduledClub([FromQuery] string id)
        {
            var result = await _services.Rota.GetSchedule<Schedule_Club>(id);
            return Ok(result);
        }

        [HttpPut("schedule/security/{id}")]
        public async Task<IActionResult> UpdateScheduleSec(string id, [FromBody] Schedule_Security dto)
        {
            await _services.Rota.ManipulateSchedule(id, dto.Start.Value, dto.Serialize());
            return NoContent();
        }

        [HttpPut("schedule/club/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] Schedule_Club dto)
        {
            await _services.Rota.ManipulateSchedule(id, dto.Start.Value, dto.Serialize());
            return NoContent();
        }

        [HttpGet]
        [Route("shifts/security")]
        public async Task<IActionResult> GetShifts([FromQuery] string id)
        {
            var result = await _services.Rota.GetSecurityShifts(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("shifts/club")]
        public async Task<IActionResult> GetShiftss([FromQuery] string id)
        {
            var result = await _services.Rota.GetClubShifts(id);
            return Ok(result);
        }

        [HttpPut("shifts/{id}")]
        public async Task<IActionResult> UpdateShift(string id, [FromBody] UpdateShiftDto dto)
        {
            await _services.Rota.UpdateShiftDetails(id, dto);
            return NoContent();
        }

        [HttpPut("shifts/attendance/{id}")]
        public async Task<IActionResult> UpdateShiftPersonel(string id, [FromBody] UpdateAttendanceDto dto)
        {
            await _services.Rota.UpdateAttendance(id, dto);

            return NoContent();
        }
    }
}