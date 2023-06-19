using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SPController : ControllerBase
    {
        private SPRepository _spRepository;
      public  SPController(SPRepository sPRepository)
        {
            _spRepository = sPRepository;
        }
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<SPModel>> Get(int id)
        {
            var spList = _spRepository.GetAll();

            return Ok(spList.Where(t=>t.ProgramFId == id));
        }

      /*  [HttpGet("{id}")]
        public ActionResult<SPModel> Get(int id)
        {
            var sp = _spRepository.GetById(id);
            if (sp == null)
            {
                return NotFound();
            }
            return Ok(sp);
        }*/

        [HttpPost]
        public ActionResult Post(SPModel sp)
        {
            _spRepository.Add(sp);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, SPModel sp)
        {
            var existingSP = _spRepository.GetById(id);
            if (existingSP == null)
            {
                return NotFound();
            }

            existingSP.ProgramFId = sp.ProgramFId;
            existingSP.SemesterFId = sp.SemesterFId;
            _spRepository.Update(existingSP);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingSP = _spRepository.GetById(id);
            if (existingSP == null)
            {
                return NotFound();
            }

            _spRepository.Delete(id);
            return Ok();
        }
    }
}
