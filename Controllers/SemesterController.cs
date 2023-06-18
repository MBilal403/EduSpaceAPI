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
        private ProgramRepository _programRepository;
        private UserRepository _userRepository;
        private CourseRepository _courseRepository;
        private SemesterRepository _semesterRepository;
        public SemesterController(SemesterRepository semesterRepository, ProgramRepository programRepository, CourseRepository courseRepository, UserRepository userRepository)
        {
            _programRepository = programRepository;
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _semesterRepository = semesterRepository;
        }
        [HttpGet("{id}")]
        public IActionResult GetByProgramId(int id)
        {
            /* var semester = _semesterRepository.GetAllSemesterByprogramId(id);*/
            var semester = _semesterRepository.GetAll().Where(t=>t.ProgramFid == id);
            foreach(var s in semester)
            {
            var T = semester.Where(t=> t.SemesterNo == s.SemesterNo);

            }

            var v=  semester.Where(t => t.ProgramFid == id).GroupBy(t=>t.SemesterNo);
            if (semester == null || v == null)
            {
                return NotFound();
            }

           
      /*      var Course = _courseRepository.GetCourseById(v.CourseFId);

            var User = _userRepository.GetAllUsers().FirstOrDefault(t => t.UserId == v.TeacherFid);
            var program = _programRepository.GetAllProgramById().FirstOrDefault(t => t.ProgramId == id);
*/

         
            return Ok(v);
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
