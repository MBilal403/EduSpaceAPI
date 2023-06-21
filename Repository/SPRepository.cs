using EduSpaceAPI.Models;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class SPRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
      
        public SPRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
           
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
           
        }
        public IEnumerable<SPModel> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SPId, ProgramFId, SemesterFId FROM SP", connection);
                var reader = command.ExecuteReader();

                var spList = new List<SPModel>();
                while (reader.Read())
                {
                    var sp = new SPModel
                    {
                        SPId = reader.GetInt32(0),
                        ProgramFId = reader.GetInt32(1),
                        SemesterFId = reader.GetInt32(2)
                    };
                    spList.Add(sp);
                }

                return spList;
            }
        }
        public IEnumerable<SPModel> MyGetAll(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SPId, ProgramFId, SemesterFId FROM SP  WHERE ProgramFId = @SPId", connection);
                command.Parameters.AddWithValue("@SPId", id);
                var reader = command.ExecuteReader();

                var spList = new List<SPModel>();
                while (reader.Read())
                {
                    var sp = new SPModel
                    {
                        SPId = reader.GetInt32(0),
                        ProgramFId = reader.GetInt32(1),
                        SemesterFId = reader.GetInt32(2)
                    };
                    spList.Add(sp);
                }

                return spList;
            }
        }

        public SPModel GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT SPId, ProgramFId, SemesterFId FROM SP WHERE ProgramFId = @SPId", connection);
                command.Parameters.AddWithValue("@SPId", id);
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var sp = new SPModel
                    {
                        SPId = reader.GetInt32(0),
                        ProgramFId = reader.GetInt32(1),
                        SemesterFId = reader.GetInt32(2)
                    };
                    return sp;
                }

                return null;
            }
        }

        public void Add(SPModel sp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO SP (ProgramFId, SemesterFId) VALUES (@ProgramFId, @SemesterFId)", connection);
                command.Parameters.AddWithValue("@ProgramFId", sp.ProgramFId);
                command.Parameters.AddWithValue("@SemesterFId", sp.SemesterFId);
                command.ExecuteNonQuery();
            }
        }

        public void Update(SPModel sp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE SP SET ProgramFId = @ProgramFId, SemesterFId = @SemesterFId WHERE SPId = @SPId", connection);
                command.Parameters.AddWithValue("@ProgramFId", sp.ProgramFId);
                command.Parameters.AddWithValue("@SemesterFId", sp.SemesterFId);
                command.Parameters.AddWithValue("@SPId", sp.SPId);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM SP WHERE SPId = @SPId", connection);
                command.Parameters.AddWithValue("@SPId", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
