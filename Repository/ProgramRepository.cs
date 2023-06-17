using static EduSpaceAPI.Repository.ProgramRepository;
using System.Data.SqlClient;
using EduSpaceAPI.Models;
using System;

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
        public async Task<int> GetActiveProgramcount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT COUNT(*) FROM [dbo].[Program] WHERE [Status] = 1", connection))
                {
                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }
        public void AddProgram(ProgramModel program)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO [Program] ([ProgramName], [ProgramShortName], [DepartFId], [Status] ,[CreatedAt],[Duration]) " +
                                   "VALUES (@ProgramName, @ProgramShortName, @DepartFId, @Status, @CreatedAt,@Duration)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProgramName", program.ProgramName);
                        command.Parameters.AddWithValue("@ProgramShortName", program.ProgramShortName);
                        command.Parameters.AddWithValue("@DepartFId", program.DepartFId);
                        command.Parameters.AddWithValue("@Status", program.Status);
                        command.Parameters.AddWithValue("@CreatedAt", program.CreatedAt);
                        command.Parameters.AddWithValue("@Duration", program.Duration);
                        command.ExecuteNonQuery();
                    }
                }
            }

  
        

        public IEnumerable<ProgramModel> GetProgramById(int id)
            {
         
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM [Program] WHERE [DepartFId] = @DepartFId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DepartFId", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                List<ProgramModel> programs = new List<ProgramModel>();
                                    while (reader.Read())
                                    {
                                        programs.Add(new ProgramModel
                                        {

                                            ProgramId = (int)reader["ProgramId"],
                                            ProgramName = (string)reader["ProgramName"],
                                            ProgramShortName = (string)reader["ProgramShortName"],
                                            DepartFId = (int)reader["DepartFId"],
                                            Duration = (int)reader["Duration"],
                                            Status = (bool)reader["Status"],
                                            CreatedAt = (DateTime)reader["CreatedAt"]
                                        });
                                    }
                            return programs;
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
                                   "[DepartFId] = @DepartFId, [Status],[Duration] = @Status WHERE [ProgramId] = @ProgramId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProgramName", program.ProgramName);
                        command.Parameters.AddWithValue("@ProgramShortName", program.ProgramShortName);
                        command.Parameters.AddWithValue("@DepartFId", program.DepartFId);
                        command.Parameters.AddWithValue("@Status", program.Status);
                        command.Parameters.AddWithValue("@ProgramId", program.ProgramId);
                        command.Parameters.AddWithValue("@Duration", program.Duration);
                        command.ExecuteNonQuery();
                    }
                }
            }
        // Update the department status
        public void UpdateProgramStatus(int ProgramId, bool status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE [dbo].[Program] " +
                                "SET" +
                                "[Status] = @Status " +
                                "WHERE [ProgramId] = @ProgramId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@ProgramId", ProgramId);
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
