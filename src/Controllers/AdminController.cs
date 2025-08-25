using Microsoft.AspNetCore.Mvc;
using ScissorLink.Models;
using ScissorLink.DTOs;
using ScissorLink.Services.Interface;

namespace ScissorLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IProcessService _processService;

        public AdminController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet("shortlinks")]
        public async Task<ActionResult<List<ShortLinkResponseDto>>> GetAllShortLinks()
        {
            try
            {
                var shortLinks = await _processService.GetAll();
                var response = shortLinks.Select(MapToResponseDto).ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("shortlinks/{id}")]
        public async Task<ActionResult<ShortLinkResponseDto>> GetShortLink(int id)
        {
            try
            {
                var shortLink = await _processService.GetById(id);
                if (shortLink == null)
                {
                    return NotFound($"Short link with ID {id} not found.");
                }

                var response = MapToResponseDto(shortLink);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("shortlinks")]
        public async Task<ActionResult<ShortLinkResponseDto>> CreateShortLink([FromBody] ShortLinkRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var shortLink = new DtoShortLink
                {
                    Title = request.Title,
                    Token = string.IsNullOrEmpty(request.Token) ? 
                        await _processService.GenerateUniqueToken() : request.Token,
                    OriginLink = request.OriginLink,
                    IsPublish = request.IsPublish,
                    CreateAdminID = 1, // Default admin ID
                    CreateAdminDate = DateTime.Now
                };

                var created = await _processService.Create(shortLink);
                var response = MapToResponseDto(created);
                
                return CreatedAtAction(nameof(GetShortLink), new { id = created.ID }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("shortlinks/{id}")]
        public async Task<ActionResult<ShortLinkResponseDto>> UpdateShortLink(int id, [FromBody] ShortLinkRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingShortLink = await _processService.GetById(id);
                if (existingShortLink == null)
                {
                    return NotFound($"Short link with ID {id} not found.");
                }

                existingShortLink.Title = request.Title;
                existingShortLink.OriginLink = request.OriginLink;
                existingShortLink.IsPublish = request.IsPublish;
                existingShortLink.EditAdminID = 1; // Default admin ID
                existingShortLink.EditAdminDate = DateTime.Now;

                var updated = await _processService.Update(existingShortLink);
                if (updated == null)
                {
                    return StatusCode(500, "Failed to update short link.");
                }

                var response = MapToResponseDto(updated);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("shortlinks/{id}")]
        public async Task<ActionResult> DeleteShortLink(int id)
        {
            try
            {
                var exists = await _processService.GetById(id);
                if (exists == null)
                {
                    return NotFound($"Short link with ID {id} not found.");
                }

                var deleted = await _processService.Delete(id);
                if (!deleted)
                {
                    return StatusCode(500, "Failed to delete short link.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("shortlinks/{id}/toggle-publish")]
        public async Task<ActionResult<ShortLinkResponseDto>> TogglePublish(int id)
        {
            try
            {
                var shortLink = await _processService.GetById(id);
                if (shortLink == null)
                {
                    return NotFound($"Short link with ID {id} not found.");
                }

                shortLink.IsPublish = !shortLink.IsPublish;
                shortLink.EditAdminID = 1; // Default admin ID
                shortLink.EditAdminDate = DateTime.Now;

                var updated = await _processService.Update(shortLink);
                if (updated == null)
                {
                    return StatusCode(500, "Failed to toggle publish status.");
                }

                var response = MapToResponseDto(updated);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private static ShortLinkResponseDto MapToResponseDto(DtoShortLink shortLink)
        {
            return new ShortLinkResponseDto
            {
                ID = shortLink.ID,
                Title = shortLink.Title,
                Token = shortLink.Token,
                OriginLink = shortLink.OriginLink,
                IsPublish = shortLink.IsPublish,
                CreateAdminDate = shortLink.CreateAdminDate,
                EditAdminDate = shortLink.EditAdminDate,
                ClickCount = shortLink.Details?.Count ?? 0,
                RecentClicks = shortLink.Details?.OrderByDescending(d => d.VisitDate)
                    .Take(10)
                    .Select(d => new ShortLinkDetailDto
                    {
                        ID = d.ID,
                        ShortLinkID = d.ShortLinkID,
                        VisitDate = d.VisitDate,
                        Country = d.Country,
                        OS = d.OS,
                        Browser = d.Browser
                    }).ToList() ?? new List<ShortLinkDetailDto>()
            };
        }
    }
}
