using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private DepartmentRepository _departmentRepository;
        public DepartmentController(DepartmentRepository departmentRepository)
        {
            _departmentRepository= departmentRepository;
        }


        // GET: api/<DepartmentController>
        [HttpGet]
        public IEnumerable<DepartmentModel> Departments()
        {
            var data  =  _departmentRepository.GetDepartments();
            return data;
        }

        // GET api/<DepartmentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DepartmentController>
        [HttpPost]
        public MyResponse<DepartmentModel> Create([FromBody] DepartmentModel dto)
        {

            MyResponse<DepartmentModel> myResponse = _departmentRepository.AddDepartment(dto);
            return myResponse;
        }
        // PUT api/<DepartmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DepartmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
