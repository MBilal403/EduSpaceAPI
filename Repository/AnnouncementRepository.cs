namespace EduSpaceAPI.Repository
{
    public class AnnouncementRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public AnnouncementRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
    }
}
