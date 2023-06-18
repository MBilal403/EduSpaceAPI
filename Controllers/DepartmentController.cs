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

        [HttpGet]
        public async Task<IActionResult> GetDepartmentCount()
        {
            try
            {
                int count = await _departmentRepository.GetDepartmentcount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }   
        
        [HttpGet]
        public IEnumerable<DepartmentModel> GetDepartmentPrograms()
        {
            try
            {
                var count =  _departmentRepository.GetDepartmentPrograms();
                return count;
            }
            catch (Exception ex)
            {
                return (IEnumerable<DepartmentModel>)StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        // GET: api/<DepartmentController>
        [HttpGet]
        public IEnumerable<DepartmentModel> Departments()
        {
            var data  =  _departmentRepository.GetDepartments();
            return data;
        }
        // GET: api/<DepartmentController>
        [HttpGet("{id}")]
        public IEnumerable<DepartmentModel> DepartmentById(int id)
        {
            var data = _departmentRepository.GetDepartments();
            var searchResults = data.Where(d => d.DepartId == id);
            return searchResults;
        }
        // GET api/<DepartmentController>/5
        [HttpGet]
        public IActionResult ActiveDepartments()
        {
            try
            {
                var departments = _departmentRepository.GetActiveDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        // GET api/<DepartmentController>/5
        [HttpGet]
        public IActionResult NonActiveDepartments()
        {
            try
            {
                var departments = _departmentRepository.GetNonActiveDepartments();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet]
        public IActionResult DepartmentsWithNullNames()
        {
            try
            {
                var departments = _departmentRepository.GetDepartmentsWithNullNames();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet]
        public IEnumerable<DepartmentModel> DepartmentsWithName(string departName)
        {

            var data = _departmentRepository.GetDepartments();
            var searchResults = data.Where(d => d.DepartName!.Contains(departName, StringComparison.OrdinalIgnoreCase));
            return searchResults;
        }
        // Put api/<DepartmentController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, [FromBody] DepartmentModel department)
        {
            try
            {
                department.DepartId = id;
                _departmentRepository.UpdateDepartment(department);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        // ghchgc
        [HttpPut("{id}")]
        public IActionResult DepartmentStatus(int id, [FromBody] DepartmentModel department)
        {
            try
            {
                _departmentRepository.UpdateDepartmentStatus(id, department.Status);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        //
        [HttpPut("{id}")]
        public IActionResult UpdateDepartName(int id, [FromBody] DepartmentModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            _departmentRepository.UpdateDepartName(id, model.DepartName!);

            return Ok();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateInchargeAndAdminNames(int id, [FromBody] DepartmentModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            _departmentRepository.UpdateInchargeAndAdminNames(id, model.InchargeName!, model.AdminName!);

            return Ok();
        }


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
