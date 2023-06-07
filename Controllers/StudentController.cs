
using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using EduSpaceAPI.MyDTOs;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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


        [HttpPost]
        public async Task<ActionResult<StudentModel>> Login(LoginDto request)
        {
            var student = await _studentRepository.IsAuthenticateUser(request);

            if (student == null)
            {
                return NotFound("Invalid email or password");
            }

            return Ok(student);
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentCount()
        {
            try
            {
                int count = await _studentRepository.GetStudentCount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost]
            public async Task<IActionResult> GetPasswordMail(ForgotPasswordDto dto)
            {
                string password = await _studentRepository.GetPassword(dto);

                if (password != null)
                {
                string subject = "Login Information (Password Recovered)";
                string body = "Dear User,\n\n"
                            + "Your login information is as follows:\n"
                            + "Email: " + dto.Email + "\n"
                            +  "Password : "+password + "\n\n"
                            + "Please use this information to log into your account.\n\n"
                            + "Best regards,\n"
                            + "EduSpace LMS";
                var msg = SendVerificationCode.SendCode(dto.Email!, subject, body);

                return Ok(msg);
            }

                else
                {
                    return NotFound("No matching record found.");
                }
            
         

        }


        /* ([FullName]
           ,[Email]
         
           ,[Password]
           ,[DateOFBirth]
           ,[Department]
           ,[Program]
        ,[City]
           ,[Address]       these are compulsory
          
           ,[ContactNumber]
           ,[Semester]
         
           ,[RollNumber]*/



        [HttpPost]
        public MyResponse<string> Register([FromBody] StudentModel user)
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

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var isEmailRegistered = await _studentRepository.IsEmailRegistered(email);
            return Ok(isEmailRegistered);
        }

    }
}
