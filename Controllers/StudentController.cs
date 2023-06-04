
using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileManager _fileManager;
        private readonly ILogger<AuthController> _logger;
        private StudentRepository _studentRepository;
        private JWTGenerator _jwtGenerator;

        public StudentController(JWTGenerator generator, FileManager fileManager, StudentRepository studentRepository, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger)
        {
            _jwtGenerator = generator;
            _fileManager = fileManager;
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


        // GET: api/<StudentController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }




        [HttpPost]
        public MyResponse<string> StudentSignup([FromBody] StudentModel user)
        {
            if (user == null)
            {
                return new MyResponse<string>()
                {
                    Message = "Enter the Data Plaese",
                    IsSuccess = false
                };
            }
            else
            {
                //   UserModel model = JsonConvert.DeserializeObject<UserModel>(user)!;
                if (string.IsNullOrEmpty(user!.Email!) || string.IsNullOrEmpty(user.Password))
                {
                    return new MyResponse<string>()
                    {
                        Message = "Enter the Email, Password & Role is Compulsory",
                        IsSuccess = false
                    };
                }

                // model.ImagePath =  _fileManager.GetFilePath(user.Image!);
                //  model.ResumePath =  _fileManager.GetFilePath(user.Resume!);

                MyResponse<string> response = _studentRepository.InsertUser(user);
                if (response.IsSuccess)
                {
                    string subject = "Login Information";
                    string body = "Dear User,\n\n"
                                + "Your login information is as follows:\n"
                                + "Email: " + user.Email + "\n"
                                + user.Password + "\n\n"
                                + "Please use this information to log into your account.\n\n"
                                + "Best regards,\n"
                                + "EduSpace LMS";
                    SendVerificationCode.SendCode(user.Email, subject, body);
                    return response;
                }
                return response;
            }



        }

        [HttpGet]
        public IActionResult Students()
        {
            var users = _studentRepository.GetAllUsers();

           return Ok(users);
        }



        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
