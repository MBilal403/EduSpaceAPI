using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using EduSpaceAPI.MyDTOs;
using EduSpaceAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace EduSpaceAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
       
        // POST api/<DepartmentController>
        [HttpPost]
        public IActionResult SendMail([FromBody] MailDto dto)
        {
            string subject = "Contact Form Submission From EduSpace LMS";
            string body = "Name: " + dto.Name + "\n"
                          + "Email: " + dto.Email + "\n"
                          + "Message: " + dto.Message;
           var msg =  SendVerificationCode.SendCode("devbilalimran@gmail.com", subject, body);

            return Ok(msg);
        }

    



    }

}
