using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet]
        public ActionResult<IEnumerable<SemesterModel>> GetAllSemester()
        {
            var semesters = _semesterRepository.GetAll();
            return Ok(semesters);
        }

        [HttpGet("{id}")]
        public ActionResult<SemesterModel> Get(int id)
        {
            var semester = _semesterRepository.GetById(id);
            if (semester == null)
            {
                return NotFound();
            }
            return Ok(semester);
        }

        [HttpPost]
        public ActionResult Post(SemesterModel semester)
        {
            _semesterRepository.Add(semester);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, SemesterModel semester)
        {
            var existingSemester = _semesterRepository.GetById(id);
            if (existingSemester == null)
            {
                return NotFound();
            }

            existingSemester.SemesterName = semester.SemesterName;
            _semesterRepository.Update(existingSemester);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingSemester = _semesterRepository.GetById(id);
            if (existingSemester == null)
            {
                return NotFound();
            }

            _semesterRepository.Delete(id);
            return Ok();
        }
    }
}
