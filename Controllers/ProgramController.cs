using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {

        private ProgramRepository _programRepository;
        public ProgramController(ProgramRepository programRepository)
        {
            _programRepository = programRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveProgramCount()
        {
            try
            {
                int count = await _programRepository.GetActiveProgramcount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult AddProgram([FromBody] ProgramModel program)
        {
            try
            {
                _programRepository.AddProgram(program);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult ProgramById(int id)
        {
            try
            {
                var program = _programRepository.GetProgramById(id);
                if (program == null)
                    return NotFound();

                return Ok(program);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
          
            }
        }
        [HttpPut("{id}")]
        public IActionResult ProgramStatus(int id, [FromBody] ProgramModel program)
        {
            try
            {
                _programRepository.UpdateProgramStatus(id, program.Status);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProgram(int id, [FromBody] ProgramModel program)
        {
            try
            {
                program.ProgramId = id;
                _programRepository.UpdateProgram(program);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

     

    
       
    }
}
