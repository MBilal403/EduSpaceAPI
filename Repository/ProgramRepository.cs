using static EduSpaceAPI.Repository.ProgramRepository;
using System.Data.SqlClient;
using EduSpaceAPI.Models;

namespace EduSpaceAPI.Repository
{
    public class ProgramRepository
    {
          private readonly string _connectionString;
        private IConfiguration _configuration;
        IWebHostEnvironment _webHostEnvironment;

        public ProgramRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }

        // Existing code...

        public void AddProgram(ProgramModel program)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO [Program] ([ProgramName], [ProgramShortName], [DepartFId], [Status]) " +
                                   "VALUES (@ProgramName, @ProgramShortName, @DepartFId, @Status)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProgramName", program.ProgramName);
                        command.Parameters.AddWithValue("@ProgramShortName", program.ProgramShortName);
                        command.Parameters.AddWithValue("@DepartFId", program.DepartFId);
                        command.Parameters.AddWithValue("@Status", program.Status);
                        command.ExecuteNonQuery();
                    }
                }
            }

            public ProgramModel? GetProgramById(int id)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM [Program] WHERE [ProgramId] = @ProgramId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new ProgramModel
                                {
                                    ProgramId = (int)reader["ProgramId"],
                                    ProgramName = (string)reader["ProgramName"],
                                    ProgramShortName = (string)reader["ProgramShortName"],
                                    DepartFId = (int)reader["DepartFId"],
                                    Status = (bool)reader["Status"]
                                };
                            }
                        }
                    }
                }
                return null;
            }

            public void UpdateProgram(ProgramModel program)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE [Program] SET [ProgramName] = @ProgramName, [ProgramShortName] = @ProgramShortName, " +
                                   "[DepartFId] = @DepartFId, [Status] = @Status WHERE [ProgramId] = @ProgramId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProgramName", program.ProgramName);
                        command.Parameters.AddWithValue("@ProgramShortName", program.ProgramShortName);
                        command.Parameters.AddWithValue("@DepartFId", program.DepartFId);
                        command.Parameters.AddWithValue("@Status", program.Status);
                        command.Parameters.AddWithValue("@ProgramId", program.ProgramId);
                        command.ExecuteNonQuery();
                    }
                }
            }

            public void DeleteProgram(int id)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM [Program] WHERE [ProgramId] = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProgramId", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

}
