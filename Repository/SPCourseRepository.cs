using EduSpaceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class SPCourseRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;

        public SPCourseRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {

            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];

        }

        public IEnumerable<SPCourseModel> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SPCourseId, CourseFId, UserFId, SPFId FROM SPCourse", connection);
                var reader = command.ExecuteReader();

                var spCourseList = new List<SPCourseModel>();
                while (reader.Read())
                {
                    var spCourse = new SPCourseModel
                    {
                        SPCourseId = reader.GetInt32(0),
                        CourseFId = reader.GetInt32(1),
                        UserFId = reader.GetInt32(2),
                        SPFId = reader.GetInt32(3)
                    };
                    spCourseList.Add(spCourse);
                }

                return spCourseList;
            }
        }

        public SPCourseModel GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SPCourseId, CourseFId, UserFId, SPFId FROM SPCourse WHERE SPCourseId = @SPCourseId", connection);
                command.Parameters.AddWithValue("@SPCourseId", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var spCourse = new SPCourseModel
                    {
                        SPCourseId = reader.GetInt32(0),
                        CourseFId = reader.GetInt32(1),
                        UserFId = reader.GetInt32(2),
                        SPFId = reader.GetInt32(3)
                    };
                    return spCourse;
                }

                return null;
            }
        }

        public void Add(SPCourseModel spCourse)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO SPCourse (CourseFId, UserFId, SPFId) VALUES (@CourseFId, @UserFId, @SPFId)", connection);
                command.Parameters.AddWithValue("@CourseFId", spCourse.CourseFId);
                command.Parameters.AddWithValue("@UserFId", spCourse.UserFId);
                command.Parameters.AddWithValue("@SPFId", spCourse.SPFId);
                command.ExecuteNonQuery();
            }
        }

        public void Update(SPCourseModel spCourse)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE SPCourse SET CourseFId = @CourseFId, UserFId = @UserFId, SPFId = @SPFId WHERE SPCourseId = @SPCourseId", connection);
                command.Parameters.AddWithValue("@CourseFId", spCourse.CourseFId);
                command.Parameters.AddWithValue("@UserFId", spCourse.UserFId);
                command.Parameters.AddWithValue("@SPFId", spCourse.SPFId);
                command.Parameters.AddWithValue("@SPCourseId", spCourse.SPCourseId);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM SPCourse WHERE SPCourseId = @SPCourseId", connection);
                command.Parameters.AddWithValue("@SPCourseId", id);
                command.ExecuteNonQuery();
            }
        }

    }
}
