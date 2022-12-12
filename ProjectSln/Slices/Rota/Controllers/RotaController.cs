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

        //[HttpPut("schedule/security/{id}/{day}")]
        //public async Task<IActionResult> UpdateScheduleSec(string id, [FromBody] Schedule_Security dto)
        //{
        //    if (day < 7 | day > 14)
        //        return BadRequest("Invalid date");

        //    await _services.Rota.ManipulateSchedule(id, dto.D, dto.Serialize());
        //    return NoContent();
        //}

        [HttpPut("schedule/club/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpdateSchedule_ClubDto dto)
        {
          
            if (validateScedule(dto.Date))
                return BadRequest("Invalid date, schedules can be changed between 7-14 days from the current date.");
            
            await _services.Rota.ManipulateSchedule(id, dto.Date, dto.Serialize());
            return NoContent();
        }

        Func<DateTime, bool> validateScedule = (date) => date > DateTime.UtcNow.AddDays(7) && date <= DateTime.UtcNow.AddDays(14);
        Func<DateTime, bool> validateShift = (date) => date <= DateTime.UtcNow.AddDays(7) | date >= DateTime.UtcNow.AddMinutes(5);


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
        public async Task<IActionResult> DeleteShift(string id, DeleteShiftDto dto)
        {
            if (validateScedule(dto.Date))
                return BadRequest("Invalid date.");
            await _services.Rota.DeleteShift(id, dto.Date);
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