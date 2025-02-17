using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Dtos;
using Web.Application.Dtos.Clubs;
using Web.Infrastructure.DBContext;
using Web.Application.Services;
using System.Net;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/clubs")]
    public class ClubsController : ControllerBase
    {
        private readonly IClubService _clubService;
        private readonly IMapper _mapper;
        public ClubsController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet("{clubId}")]
        public async Task<IActionResult> GetClubById([FromHeader(Name = "Club-ID")] string clubId)
        {
            var club = await _clubService.GetByIdAsync(clubId);
            if (club == null) return NotFound();

            var dto = _mapper.Map<ClubDto>(club);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub([FromHeader(Name = "Player-ID")] int playerId, [FromBody] CreateClubDto request)
        {
            try
            {
                if (await _clubService.IsNameExistedAsync(request.Name))
                {
                    return Conflict(new { message = $"An existing club with the name '{request.Name}' was already found." });
                }
                var club = await _clubService.CreateAsync(request.Name, playerId);
                return CreatedAtAction(nameof(CreateClub),club);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{clubId}/members")]
        public async Task<IActionResult> JoinClub(string clubId, [FromBody] JoinClubDto request)
        {
            var success = await _clubService.AddPlayerToClubAsync(clubId, request.PlayerId);
            if (!success) return BadRequest("Could not join the club.");
            return NoContent();
        }
    }

}
