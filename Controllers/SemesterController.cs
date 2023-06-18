using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private SemesterRepository _semesterRepository;
        public SemesterController(SemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }
        [HttpGet("{id}")]
        public IActionResult GetByProgramId(int id)
        {
            var semester = _semesterRepository.GetAllSemesterByprogramId(id);
            if (semester == null)
            {
                return NotFound();
            }
            return Ok(semester);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var semesters = _semesterRepository.GetAll();
            return Ok(semesters);
        }

        [HttpPost]
        public IActionResult Create([FromBody] SemesterModel semester)
        {
            _semesterRepository.Add(semester);
            return Ok();
        }

       /* [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SemesterModel semester)
        {
            var existingSemester = _semesterRepository.GetById(id);
            if (existingSemester == null)
            {
                return NotFound();
            }

            existingSemester.ProgramFid = semester.ProgramFid;
            existingSemester.CourseFId = semester.CourseFId;
            existingSemester.TeacherFid = semester.TeacherFid;
            existingSemester.TimeTable = semester.TimeTable;

            _semesterRepository.Update(existingSemester);
            return Ok();
        }
*/
       /* [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var semester = _semesterRepository.GetById(id);
            if (semester == null)
            {
                return NotFound();
            }

            _semesterRepository.Delete(id);
            return Ok();
        }*/

    }
}
