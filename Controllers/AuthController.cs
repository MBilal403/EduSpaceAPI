using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private JWTGenerator jwtGenerator;
        public AuthController(JWTGenerator generator) {
            jwtGenerator = generator;
        }

        // GET: Auth/Getlist
        [HttpGet]
        public IEnumerable<string> Getlist() // For Testing
        {
            return new string[] { "value1", "value2" };
        }

        // GET: Auth/UserRole -> with id + Name + desc
        public IEnumerable<string> UserRoles() // For Testing
        {
            return new string[] { "value1", "value2" };
        }


        // POST api/<AuthController>
        [HttpPost]
        public string Login([FromBody] LoginModel model)
        {
           
            

            return jwtGenerator.GetToken(model);
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

    }
}
