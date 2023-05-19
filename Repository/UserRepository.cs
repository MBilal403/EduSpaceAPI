using Microsoft.Extensions.Configuration;

namespace EduSpaceAPI.Repository
{
    public class UserRepository
    {
        private IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }




    }
}
