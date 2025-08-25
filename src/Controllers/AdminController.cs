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

        [HttpGet("shortlinks/{id}/statistics")]
        public async Task<ActionResult<object>> GetShortLinkStatistics(int id)
        {
            try
            {
                var shortLink = await _processService.GetById(id);
                if (shortLink == null)
                {
                    return NotFound($"Short link with ID {id} not found.");
                }

                var details = shortLink.Details ?? new List<DtoShortLinkDetail>();

                // Group by date for daily statistics
                var dailyStats = details
                    .GroupBy(d => d.VisitDate.Date)
                    .Select(g => new { Date = g.Key, Clicks = g.Count() })
                    .OrderBy(x => x.Date)
                    .ToList();

                // Group by country
                var countryStats = details
                    .Where(d => !string.IsNullOrEmpty(d.Country))
                    .GroupBy(d => d.Country)
                    .Select(g => new { Country = g.Key, Clicks = g.Count() })
                    .OrderByDescending(x => x.Clicks)
                    .Take(10)
                    .ToList();

                // Group by OS
                var osStats = details
                    .Where(d => !string.IsNullOrEmpty(d.OS))
                    .GroupBy(d => d.OS)
                    .Select(g => new { OS = g.Key, Clicks = g.Count() })
                    .OrderByDescending(x => x.Clicks)
                    .ToList();

                // Group by Browser
                var browserStats = details
                    .Where(d => !string.IsNullOrEmpty(d.Browser))
                    .GroupBy(d => d.Browser)
                    .Select(g => new { Browser = g.Key, Clicks = g.Count() })
                    .OrderByDescending(x => x.Clicks)
                    .ToList();

                // Group by hour for hourly distribution
                var hourlyStats = details
                    .GroupBy(d => d.VisitDate.Hour)
                    .Select(g => new { Hour = g.Key, Clicks = g.Count() })
                    .OrderBy(x => x.Hour)
                    .ToList();

                var statistics = new
                {
                    ShortLink = MapToResponseDto(shortLink),
                    TotalClicks = details.Count,
                    UniqueCountries = details.Where(d => !string.IsNullOrEmpty(d.Country)).Select(d => d.Country).Distinct().Count(),
                    DailyStats = dailyStats,
                    CountryStats = countryStats,
                    OSStats = osStats,
                    BrowserStats = browserStats,
                    HourlyStats = hourlyStats,
                    FirstClick = details.Any() ? details.Min(d => d.VisitDate) : (DateTime?)null,
                    LastClick = details.Any() ? details.Max(d => d.VisitDate) : (DateTime?)null
                };

                return Ok(statistics);
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
