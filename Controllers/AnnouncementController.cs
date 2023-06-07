using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileManager _fileManager;
        private readonly ILogger<AuthController> _logger;
        private AnnouncementRepository _announcementRepository;
        private JWTGenerator _jwtGenerator;

        public AnnouncementController(JWTGenerator generator, FileManager fileManager, AnnouncementRepository announcementRepository, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger)
        {
            _jwtGenerator = generator;
            _fileManager = fileManager;
            _announcementRepository = announcementRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> AddAnnouncement(AnnouncementModel announcement)
        {
            try
            {
                int announcementId = await _announcementRepository.Addannouncement(announcement);
                return Ok(announcementId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{role}")]
        public async Task<IActionResult> GetAnnouncementsByRole(string role)
        {
            try
            {
                var announcements = await _announcementRepository.GetannouncementsByRole(role);
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{announcementId}")]
        public async Task<IActionResult> DeleteAnnouncement(int announcementId)
        {
            try
            {
                int affectedRows = await _announcementRepository.Deleteannouncement(announcementId);
                if (affectedRows > 0)
                    return NoContent();
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
    }

}

