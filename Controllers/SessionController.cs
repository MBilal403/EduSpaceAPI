using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private  SessionRepository _sessionRepository;

        public SessionController(SessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSessions()
        {
            try
            {
                var sessions = await _sessionRepository.GetAllSessionsAsync();
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                // Handle the exception, log, and return an appropriate error response
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            try
            {
                var session = await _sessionRepository.GetSessionByIdAsync(id);
                if (session == null)
                    return NotFound();

                return Ok(session);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] SessionModel session)
        {
            try
            {
               
              int id =   await _sessionRepository.AddSessionAsync(session);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id,[FromBody] SessionModel session)
        {
            try
            {
                await _sessionRepository.UpdateSessionAsync(id, session);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            try
            {
                await _sessionRepository.DeleteSessionAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
