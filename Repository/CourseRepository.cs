using EduSpaceAPI.Models;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class CourseRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        DepartmentRepository _departmentRepository;
        IWebHostEnvironment _webHostEnvironment;
        public CourseRepository(IConfiguration configuration,DepartmentRepository departmentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _departmentRepository = departmentRepository;   
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<CourseModel> GetAllCourses()
        {
            var courses = new List<CourseModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Course", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var course = new CourseModel
                        {
                            CourseId = (int)reader["CourseId"],
                            CourseCode = reader["CourseCode"].ToString(),
                            CourseTitle = reader["CourseTitle"].ToString(),
                            CourseCreditHours = (int)reader["CourseCreditHours"],
                            DepartmentFId = (int)reader["DepartmentFId"]
                        };
                        var data = _departmentRepository.GetDepartments();
                        var searchResults = data.FirstOrDefault(d => d.DepartId == course.DepartmentFId);
                        course.DepartName = searchResults!.DepartName;


                        courses.Add(course);
                    }
                }

                reader.Close();
            }

            return courses;
        }

        public CourseModel GetCourseById(int id)
        {
            CourseModel course = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Course WHERE CourseId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    course = new CourseModel
                    {
                        CourseId = (int)reader["CourseId"],
                        CourseCode = reader["CourseCode"].ToString(),
                        CourseTitle = reader["CourseTitle"].ToString(),
                        CourseCreditHours = (int)reader["CourseCreditHours"],
                        DepartmentFId = (int)reader["DepartmentFId"]
                    };
                }

                reader.Close();
            }

            return course;
        }

        public void AddCourse(CourseModel course)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("INSERT INTO Course (CourseCode, CourseTitle, CourseCreditHours, DepartmentFId) VALUES (@CourseCode, @CourseTitle, @CourseCreditHours, @DepartmentFId)", connection);
                command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                command.Parameters.AddWithValue("@CourseTitle", course.CourseTitle);
                command.Parameters.AddWithValue("@CourseCreditHours", course.CourseCreditHours);
                command.Parameters.AddWithValue("@DepartmentFId", course.DepartmentFId);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateCourse(CourseModel course)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("UPDATE Course SET CourseCode = @CourseCode, CourseTitle = @CourseTitle, CourseCreditHours = @CourseCreditHours, DepartmentFId = @DepartmentFId WHERE CourseId = @Id", connection);
                command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                command.Parameters.AddWithValue("@CourseTitle", course.CourseTitle);
                command.Parameters.AddWithValue("@CourseCreditHours", course.CourseCreditHours);
                command.Parameters.AddWithValue("@DepartmentFId", course.DepartmentFId);
                command.Parameters.AddWithValue("@Id", course.CourseId);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteCourse(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("DELETE FROM Course WHERE CourseId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }


}
