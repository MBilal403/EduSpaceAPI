using EduSpaceAPI.Models;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class SemesterRepository
    {
        private readonly string _connectionString;
        private IConfiguration _configuration;
        IWebHostEnvironment _webHostEnvironment;

        public SemesterRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        public IEnumerable<SemesterModel> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SemesterId, SemesterName FROM Semester", connection);
                var reader = command.ExecuteReader();
                var semesters = new List<SemesterModel>();
                while (reader.Read())
                {
                    var semester = new SemesterModel
                    {
                        SemesterId = reader.GetInt32(0),
                        SemesterName = reader.GetString(1)
                    };
                    semesters.Add(semester);
                }

                return semesters;
            }
        }

        public SemesterModel GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SemesterId, SemesterName FROM Semester WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var semester = new SemesterModel
                    {
                        SemesterId = reader.GetInt32(0),
                        SemesterName = reader.GetString(1)
                    };
                    return semester;
                }

                return null;
            }
        }

        public void Add(SemesterModel semester)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Semester (SemesterName) VALUES (@SemesterName)", connection);
                command.Parameters.AddWithValue("@SemesterName", semester.SemesterName);
                command.ExecuteNonQuery();
            }
        }

        public void Update(SemesterModel semester)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Semester SET SemesterName = @SemesterName WHERE SemesterId = @Id", connection);
                command.Parameters.AddWithValue("@SemesterName", semester.SemesterName);
                command.Parameters.AddWithValue("@Id", semester.SemesterId);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Semester WHERE SemesterId = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
