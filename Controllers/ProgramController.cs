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

        [HttpDelete("programs/{id}")]
        public IActionResult DeleteProgram(int id)
        {
            try
            {
                _programRepository.DeleteProgram(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        // GET: api/<ProgramController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProgramController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProgramController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProgramController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProgramController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
