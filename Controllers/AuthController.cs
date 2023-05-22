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

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<AuthController> _logger;   
        private UserRepository _userRepository;
        private JWTGenerator _jwtGenerator;

        public AuthController(JWTGenerator generator,UserRepository userRepository, IWebHostEnvironment webHostEnvironment,ILogger<AuthController> logger ) {
            _jwtGenerator = generator;
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Auth/Getlist
        [HttpGet]
        public IEnumerable<string> Getlist() // For Testing
        {
            _logger.LogInformation("hello");
            return new string[] { "value1", "value2" };
        }

        // Post: Auth/UserRole -> with id + Name + desc
        [HttpPost]
        public IEnumerable<string> UserRoles(string signupDto) // For Testing
        {

            _logger.LogInformation(signupDto);
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public IActionResult Login(UserModel loginModel)
        {
           // _logger.LogInformation(");
            try
            {
                // Check if the login credentials are valid
                int id = _userRepository.IsAuthenticateUser(loginModel);

                if (id > 0)
                {
                    // Return a success response
                    return Ok();
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
        public IActionResult Registration([FromBody] LoginModel emailModel)
        {

            if (SendVerificationCode.SendCode(emailModel.Email) == "successfully sent.")
            {
                return Ok("Verification code sent successfully.");
            }
            return StatusCode(500, "Failed to send verification code. Error");

        }
        [HttpPost]
        public string UploadImages(IFormFile file)
        {
            byte[] imageBytes;
            string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath,"Uploads", "DefaultImage.jpg");
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
        public IActionResult UserSignup([FromBody] UserModel user)
        {
           //serModel user = JsonConvert.DeserializeObject<UserModel>(dto.userModel);
            if (user == null)
            {
              
                return Ok("empty");
            }
            {

                bool result = _userRepository.InsertUser(user);
                if (result)
                {
                    return Ok("User Inserted");
                }
                else
                    return Ok("Not inserted");
                return Ok("empty");
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
            string fileName = Guid.NewGuid().ToString()+".pdf"; //+ Path.GetExtension(file.FileName);

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
    }




}

