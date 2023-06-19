using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SPCourseController : ControllerBase
    {
        private SPCourseRepository _spCourseRepository;
        private CourseRepository _courseRepository;
        private UserRepository _userRepository;
        public SPCourseController(SPCourseRepository sPCourseRepository,UserRepository userRepository, CourseRepository courseRepository)
        {
            _spCourseRepository = sPCourseRepository;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<SPCourseModel>> Get(int id)
        {
            var spCourseList = _spCourseRepository.GetAll();
            var kist = spCourseList.Where(t => t.SPFId == id);

            List<SPCourseModel> result = new List<SPCourseModel>(); 
            foreach (SPCourseModel spCourse in kist)
            {
                var course = _courseRepository.GetCourseById(spCourse.CourseFId);
                var teacher = _userRepository.GetAllUsers().FirstOrDefault(t => t.UserRole == "Teacher" && t.UserId == spCourse.UserFId);
                var SPCourseid = spCourse.SPCourseId;
                result.Add(new SPCourseModel()
                {
                    Course = course,
                    SPCourseId = SPCourseid,
                    User = teacher
                }); ;
            }

            return Ok(result);

        }
/*
        [HttpGet("{id}")]
        public ActionResult<SPCourseModel> Get(int id)
        {
            var spCourse = _spCourseRepository.GetById(id);
            if (spCourse == null)
            {
                return NotFound();
            }
            return Ok(spCourse);
        }*/

        [HttpPost]
        public ActionResult AddCourse(SPCourseModel spCourse)
        {
            _spCourseRepository.Add(spCourse);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, SPCourseModel spCourse)
        {
            var existingSPCourse = _spCourseRepository.GetById(id);
            if (existingSPCourse == null)
            {
                return NotFound();
            }

            existingSPCourse.CourseFId = spCourse.CourseFId;
            existingSPCourse.UserFId = spCourse.UserFId;
            existingSPCourse.SPFId = spCourse.SPFId;
            _spCourseRepository.Update(existingSPCourse);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingSPCourse = _spCourseRepository.GetById(id);
            if (existingSPCourse == null)
            {
                return NotFound();
            }

            _spCourseRepository.Delete(id);
            return Ok();
        }
    }
}
