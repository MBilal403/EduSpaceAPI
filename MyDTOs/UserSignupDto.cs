using EduSpaceAPI.Models;

namespace EduSpaceAPI.MyDTOs
{
    public class UserSignupDto
    {

        public IFormFile? UserImage { get; set; }
        public IFormFile? Resume { get; set; }
        public string userModel { get; set; }    
    }
}
