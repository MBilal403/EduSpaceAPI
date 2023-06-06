using EduSpaceAPI.Helpers;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileManager _fileManager;
        private readonly ILogger<AuthController> _logger;
        private UserRepository _userRepository;
        private JWTGenerator _jwtGenerator;

        public AnnouncementController(JWTGenerator generator, FileManager fileManager, UserRepository userRepository, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger)
        {
            _jwtGenerator = generator;
            _fileManager = fileManager;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        // GET: api/<AnnouncementController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AnnouncementController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AnnouncementController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AnnouncementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AnnouncementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
