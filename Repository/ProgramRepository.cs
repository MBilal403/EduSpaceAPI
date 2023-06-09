﻿using static EduSpaceAPI.Repository.ProgramRepository;
using System.Data.SqlClient;
using EduSpaceAPI.Models;
using System;

namespace EduSpaceAPI.Repository
{
    public class ProgramRepository
    {
          private readonly string _connectionString;
        private IConfiguration _configuration;
   
        SPRepository _spRepository;

        public ProgramRepository(IConfiguration configuration,SPRepository sPRepository, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
           
            _spRepository = sPRepository;
            _connectionString = _configuration["ConnectionString:DBx"];
            
        }
        public List<ProgramModel> GetAllPrograms()
        {
            List<ProgramModel> programs = new List<ProgramModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM [dbo].[Program]";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ProgramModel program = new ProgramModel
                        {
                            ProgramId = Convert.ToInt32(reader["ProgramId"]),
                            ProgramName = reader["ProgramName"].ToString(),
                            ProgramShortName = reader["ProgramShortName"].ToString(),
                            DepartFId = Convert.ToInt32(reader["DepartFId"]),
                            Status = Convert.ToBoolean(reader["Status"]),
                            Duration = Convert.ToInt32(reader["Duration"]),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        };

                        programs.Add(program);
                    }

                    reader.Close();
                }
            }

            return programs;
        }

        public IEnumerable<ProgramModel> GetAllProgramById()
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM [Program] ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
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
                var id = GetAllProgramById().FirstOrDefault(t => t.ProgramName == program.ProgramName);
                    for (int i = 1; i <= program.Duration; i++)
                {
                    _spRepository.Add(new SPModel()
                    {
                        SemesterFId = i,
                        ProgramFId = id.ProgramId,
                    }) ;
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
