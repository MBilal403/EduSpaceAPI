using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SemesterShowController : ControllerBase
    {

        private IConfiguration _configuration;
    
        private ProgramRepository _programRepository;
        private SPCourseRepository _spCourseRepository;
        private SPRepository _spRepository;
        private SPCourseController _spCourseController;
      
        public SemesterShowController(SPCourseController spCourseController, ProgramRepository programRepository, SPCourseRepository spCourseRepository, SPRepository spRepository)
        {
            _programRepository = programRepository;
            _spCourseRepository = spCourseRepository;
            _spRepository = spRepository;
            _spCourseController = spCourseController;

        }

        [HttpGet]
        public IActionResult GetSemesters(string programName)
        {
            List<List<MySPCourseModel>> mySPCourseModels = new List<List<MySPCourseModel>> ();   
         var programid  = _programRepository.GetAllPrograms().FirstOrDefault(t=>t.ProgramName == programName);
            var SPIDs = _spRepository.MyGetAll(programid!.ProgramId);
         
            foreach(var id in SPIDs)
            {
                mySPCourseModels.Add( _spCourseController.MyGet(id.SPId));
            }
            
            return Ok(mySPCourseModels);
        }





        // GET: api/<SemesterShowController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SemesterShowController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SemesterShowController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SemesterShowController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SemesterShowController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
