using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;


namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
            private readonly CourseRepository _repository;

            public CourseController(CourseRepository repository)
            {
                _repository = repository;
            }

            [HttpGet]
            public IActionResult GetAllCourses()
            {
                var courses = _repository.GetAllCourses();
                return Ok(courses);
            }

            [HttpGet("{id}")]
            public IActionResult GetCourseById(int id)
            {
                var course = _repository.GetCourseById(id);

                if (course == null)
                {
                    return NotFound();
                }

                return Ok(course);
            }

            [HttpPost]
            public IActionResult AddCourse([FromBody] CourseModel course)
            {
                _repository.AddCourse(course);
                return Ok();
            }

            [HttpPut("{id}")]
            public IActionResult UpdateCourse(int id, [FromBody] CourseModel course)
            {
                var existingCourse = _repository.GetCourseById(id);

                if (existingCourse == null)
                {
                    return NotFound();
                }

                course.CourseId = id;
                _repository.UpdateCourse(course);

                return Ok();
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteCourse(int id)
            {
                _repository.DeleteCourse(id);

                return Ok();
            }
        
    }
}
