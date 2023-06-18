using EduSpaceAPI.Models;
using System.Data.SqlClient;
using System.Xml;

namespace EduSpaceAPI.Repository
{
    public class SemesterRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        private ProgramRepository _programRepository;
        private UserRepository _userRepository;
        private CourseRepository _courseRepository;
        IWebHostEnvironment _webHostEnvironment;
        public SemesterRepository(IConfiguration configuration,ProgramRepository programRepository,CourseRepository courseRepository,UserRepository userRepository, IWebHostEnvironment webHostEnvironment)
        {
            _programRepository = programRepository;
            _userRepository = userRepository;
            _courseRepository  = courseRepository;
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        // take the program id  and return its all semester detail
        public IEnumerable<SemesterModel> GetAllSemesterByprogramId(int id)
        {
            List<SemesterModel> semesterModels = new List<SemesterModel>(); 
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Semester WHERE ProgramFId = @id";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                  
                        var semester = new SemesterModel(){
                            SemesterNo = (int)reader["SemesterNo"],
                            ProgramFid = (int)reader["ProgramFid"],
                            CourseFId = (int)reader["CourseFId"],
                            TeacherFid = (int)reader["TeacherFid"],
                            TimeTable = (DateTime)reader["TimeTable"],
                           
                       
                    };
                        var Course = _courseRepository.GetCourseById(semester.CourseFId);
                       var  User = _userRepository.GetAllUsers().FirstOrDefault(t => t.UserId == semester.TeacherFid);
                        var program = _programRepository.GetAllProgramById().FirstOrDefault(t => t.ProgramId == semester.ProgramFid);
                        semester.Course = Course;
                        semester.User = User;
                        semester.Program = program; 


                        semesterModels.Add(semester);


                        return semesterModels;
                    }
                }
            }

            return null;
        }

        public IEnumerable<SemesterModel> GetAll()
        {
            var semesters = new List<SemesterModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Semester";
                var command = new SqlCommand(query, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesters.Add(MapSemesterFromReader(reader));
                    }
                }
            }

            return semesters;
        }

        public void Add(SemesterModel semester)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Semester (SemesterNo, ProgramFid, CourseFId, TeacherFid, TimeTable) VALUES (@SemesterNo, @ProgramFid, @CourseFId, @TeacherFid, @TimeTable)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SemesterNo", semester.SemesterNo);
                command.Parameters.AddWithValue("@ProgramFid", semester.ProgramFid);
                command.Parameters.AddWithValue("@CourseFId", semester.CourseFId);
                command.Parameters.AddWithValue("@TeacherFid", semester.TeacherFid);
                command.Parameters.AddWithValue("@TimeTable", semester.TimeTable);

                command.ExecuteNonQuery();
            }
        }

        public void Update(SemesterModel semester)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE Semester SET ProgramFid = @ProgramFid, CourseFId = @CourseFId, TeacherFid = @TeacherFid, TimeTable = @TimeTable WHERE SemesterNo = @SemesterNo";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProgramFid", semester.ProgramFid);
                command.Parameters.AddWithValue("@CourseFId", semester.CourseFId);
                command.Parameters.AddWithValue("@TeacherFid", semester.TeacherFid);
                command.Parameters.AddWithValue("@TimeTable", semester.TimeTable);
                command.Parameters.AddWithValue("@SemesterNo", semester.SemesterNo);

                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM Semester WHERE SemesterNo = @SemesterNo";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SemesterNo", id);

                command.ExecuteNonQuery();
            }
        }

        private SemesterModel MapSemesterFromReader(SqlDataReader reader)
        {
            return new SemesterModel
            {
                SemesterNo = (int)reader["SemesterNo"],
                ProgramFid = (int)reader["ProgramFid"],
                CourseFId = (int)reader["CourseFId"],
                TeacherFid = (int)reader["TeacherFid"],
                TimeTable = (DateTime)reader["TimeTable"]
            };
        }



    }
}
