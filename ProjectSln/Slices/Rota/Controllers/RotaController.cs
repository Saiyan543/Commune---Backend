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
  //  [Authorize(AuthenticationSchemes = "Bearer")]
    public class RotaController : ControllerBase
    {
        private readonly IServiceManager _services;
        public RotaController(IServiceManager services) => _services = services;


        [HttpGet]
        [Route("scheduled/security")]
        public async Task<IActionResult> GetScheduled([FromQuery] string id)
        {
            return Ok(await _services.Rota.GetSchedule<RetrieveSecurityUp>(id));
        }

        [HttpGet]
        [Route("scheduled/club")]
        public async Task<IActionResult> GetScheduledClub([FromQuery] string id)
        {
            return Ok(await _services.Rota.GetSchedule<RetrieveClubUp>(id));
        }
        [HttpPut("schedule/security/{id}")]
        public async Task<IActionResult> UpdateScheduleSec(string id, [FromBody] SecurityUpcomingRota dto)
        {
            await _services.Rota.ManipulateSchedule(id, dto.Start, dto.Serialize());
            return NoContent();
        }

        [HttpPut("schedule/club/{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] ClubUpcomingRota dto)
        {
            await _services.Rota.ManipulateSchedule(id, dto.Start, dto.Serialize());
            return NoContent();
        }


        [HttpGet]
        [Route("shifts/security")]
        public async Task<IActionResult> GetShifts([FromQuery] string id)
        {
            return Ok(await _services.Rota.GetSecurityShifts(id));
        }



        [HttpGet]
        [Route("shifts/club")]
        public async Task<IActionResult> GetShiftss([FromQuery] string id)
        {
            return Ok(await _services.Rota.GetClubShifts(id));
        }


        [HttpPut("shifts/{id}")]
        public async Task<IActionResult> UpdateShift(string id, [FromBody] UpdateShiftDto dto)
        {
            await _services.Rota.UpdateShiftDetails(id, dto);
            return Ok();
        }

        [HttpPut("shifts/attendance/{id}")]
        public async Task<IActionResult> UpdateSecurityShift(string id, [FromBody] UpdatePersonelAttendanceDto dto)
        {
            await _services.Rota.SecurityUpdateAttendance(id, dto);

            return Ok();
        }
















         //--- Controls

        

        [HttpPut("control")]
        public async Task<IActionResult> Inc()
        {
            await _services.Rota.X();
            return NoContent();
        }
    }
}
