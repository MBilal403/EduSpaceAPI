using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Security.Claims;
using System.Text;
using EduSpaceAPI.Repository;
using EduSpaceAPI.MyDTOs;
using System.Text.Json.Serialization;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly FileManager _fileManager;
        private readonly ILogger<AuthController> _logger;
        private UserRepository _userRepository;
        private JWTGenerator _jwtGenerator;

        public AuthController(JWTGenerator generator,FileManager fileManager, UserRepository userRepository, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger) {
            _jwtGenerator = generator;
            _fileManager = fileManager;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Auth/Getlist
        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public IEnumerable<string> Getlist() // For Testing
        {
            _logger.LogInformation("hello");
            return new string[] { "value1", "value2" };
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginDto loginModel)
        {
            // _logger.LogInformation(");
            try
            {
                // Check if the login credentials are valid
                MyResponse<UserModel> user = _userRepository.IsAuthenticateUser(loginModel);

                if (user.Response.UserId > 0)
                {
                    // Return a success response
                    // Now we need to generate Token
                    user.Token = _jwtGenerator.GetToken(new UserModel
                    {
                        UserId = user.Response.UserId,
                        UserRole = user.Response.UserRole,
                        Email = user.Response.Email
                    });
                    return Ok(user);
                }
                else
                {
                    // Return an unauthorized response
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                // Handle the exception and return an appropriate response
                // Return an internal server error response with a meaningful message
                var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred while processing your request."),
                    ReasonPhrase = "Internal Server Error" + ex.Message
                };

                throw new System.Web.Http.HttpResponseException(errorResponse);
            }
        }



        [HttpPost]
        public async Task<IActionResult> GetPasswordMail(ForgotPasswordDto dto)
        {
            string password = await _userRepository.GetPassword(dto);

            if (password != null)
            {
                string subject = "Login Information (Password Recovered)";
                string body = "Dear User,\n\n"
                            + "Your login information is as follows:\n"
                            + "Email: " + dto.Email + "\n"
                            + "Password : " + password + "\n\n"
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
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet]
        public IActionResult AdminUsers()
        {
            try
            { 
            var users = _userRepository.GetAllUsers();
            var searchResults = users.Where(d => d.UserRole == "Admin");
            return Ok(searchResults);
            }catch(Exception ex)
            {
              return  StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }
        [HttpGet]
        public IActionResult TeacherUsers()
        {
            try
            {
            var users = _userRepository.GetAllUsers();
            var searchResults = users.Where(d => d.UserRole == "Teacher");
            return Ok(searchResults);
        }catch(Exception ex)
            {
              return  StatusCode(500, $"An error occurred: {ex.Message}");
    }
}


        [HttpGet("{id}")]
        public MyResponse<UserModel> UserById(int id) 
        {

            return _userRepository.GetUserById(id);
        }

        [HttpGet("{path}")]
        public HttpResponseMessage GetImage(string path)
        {
            byte[] imageBytes = _userRepository.GetUserImageByImagePath(path);

          /*  if (imageBytes == null)
            {
                // Image not found in the database
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Image not found.");
            }*/

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(imageBytes);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg"); // Adjust the content type based on your image format

            return response;
        }

       /* [HttpPost]
        public IActionResult Registration([FromBody] LoginModel emailModel)
        {
            if (SendVerificationCode.SendCode(emailModel.Email) == "successfully sent.")
            {
                return Ok("Verification code sent successfully.");
            }
            return StatusCode(500, "Failed to send verification code. Error");

        }*/
        [HttpPost]
        public string UploadImages(IFormFile file)
        {
            byte[] imageBytes;
            string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "DefaultImage.jpg");
            // Read the image file as a byte array
            if (System.IO.File.Exists(defaultImagePath))
            {
                imageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
            }
            else
            {
                // Handle the case when the image file does not exist
                // For example, you can assign a default image byte array
                imageBytes = GetDefaultImageBytes();
            }

            // Execute your SQL query with the image byte array
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-TKFMBUI;Initial Catalog=fyp;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand("INSERT INTO [dbo].[new] ([Images]) VALUES (@Images)", connection))
                {
                    command.Parameters.Add("@Images", SqlDbType.VarBinary, -1).Value = imageBytes;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return "b";
        }
        [NonAction]
        private byte[] GetDefaultImageBytes()
        {
            // Assuming you have a default image stored in your project or as a resource
            // Load the default image file or generate a default image byte array

            // Option 1: Load default image file from disk
            string defaultImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads", "DefaultImage.jpg");
            byte[] defaultImageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
            return defaultImageBytes;
        }

        [HttpPost]
        public MyResponse<string> UserSignup([FromBody] UserModel user)
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
                if (string.IsNullOrEmpty(user!.Email!) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.UserRole)) {
                    return new MyResponse<string>()
                    {
                        Message = "Enter the Email, Password & Role is Compulsory",
                        IsSuccess = false
                    };
                }

              // model.ImagePath =  _fileManager.GetFilePath(user.Image!);
             //  model.ResumePath =  _fileManager.GetFilePath(user.Resume!);
                      
                MyResponse<string> response = _userRepository.InsertUser(user);
                if(response.IsSuccess)
                {
                    string subject = "Login Information";
                    string body = "Dear User,\n\n"
                                + "Your login information is as follows:\n"
                                + "Email: "+ user.Email +"\n"
                                + user.Password +"\n\n"
                                + "Please use this information to log into your account.\n\n"
                                + "Best regards,\n"
                                + "EduSpace LMS";
                    SendVerificationCode.SendCode(user.Email, subject, body);
                    return response;
                }
                return response;
            }



        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {

            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file.");
            }

            // Generate a unique filename
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Get the uploads folder path
            string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads");

            // Create the uploads folder if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // Combine the uploads folder path with the file name
            string filePath = Path.Combine(uploadsFolder, fileName);
            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            byte[] filebyte = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.WriteAllBytes(filePath, filebyte);

            return Ok($"File uploaded successfully. File path: {filePath} + {filebyte}");
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var isEmailRegistered = await _userRepository.IsEmailRegistered(email);
            return Ok(isEmailRegistered);
        }

    }

}

