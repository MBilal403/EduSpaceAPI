using System.Collections;
using System.Data.SqlClient;
using System.Data;
using EduSpaceAPI.Models;
using EduSpaceAPI.Helpers;

namespace EduSpaceAPI.Repository
{
    public class StudentRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public StudentRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        // Inser Student to the data base
        public MyResponse<string> InsertUser(StudentModel student)
        {
            int rowsAffected = 0;
            Hashtable parameters = new Hashtable
            {
                { "@FullName", student.FullName },
                { "@Email", student.Email },
                { "@Password", student.Password },
                { "@DateOfBirth", student.DateOfBirth },
                { "@Department", student.Department },
                { "@Program", student.Program },
                { "@FatherName", student.FatherName },
                { "@Image", student.Image },
                { "@ImagePath", student.ImagePath },
                { "@City", student.City },
                { "@Address", student.Address },
                { "@Session", student.Session },
                { "@ContactNumber", student.ContactNumber },
                { "@Semester", student.Semester },
                { "@CreatedAt", student.CreatedAt },
                { "@RollNumber", student.RollNumber }
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("InsertStudent", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Set the parameters
                        foreach (DictionaryEntry entry in parameters)
                        {
                            string parameterName = (string)entry.Key;
                            if (parameterName == "@Image")
                            {
                                if (student.ImagePath == null)
                                {
                                    object parameterValue = (entry.Value != null) ? entry.Value : GetDefaultImageBytes();
                                    command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = parameterValue;
                                }
                                else
                                {
                                    object parameterValue = student.Image!;
                                    command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = parameterValue;
                                }

                            }
                            else if (parameterName == "@CreatedAt")
                            {
                                command.Parameters.AddWithValue(parameterName, entry.Value);
                            }
                            else
                            {
                                object parameterValue = (entry.Value != null) ? entry.Value : DBNull.Value;
                                command.Parameters.AddWithValue(parameterName, parameterValue);
                            }
                        }
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected != 0)
                            {

                                return new MyResponse<string>()
                                {
                                    Message = "Rows Effected",
                                    IsSuccess = true
                                };
                            }
                            else
                                return new MyResponse<string>()
                                {
                                    Message = "Now Row Effected",
                                    IsSuccess = false
                                };
                        }
                        else
                        {
                            return new MyResponse<string>()
                            {
                                Message = "Connection is not Open",
                                IsSuccess = false
                            };

                        }

                    }
                }
            }


            catch (Exception ex)
            {
                // Handle the exception (e.g., log the error, throw a custom exception, etc.)
                // You can also rethrow the exception if you want to bubble it up to the caller
                // For simplicity, we'll just throw a new exception with the original message
                //Connection Auti Close
                return new MyResponse<string>()
                {
                    Message = "Exception Occur Due to " + ex.Message,
                    IsSuccess = false
                };
                throw new Exception("An error occurred while inserting the user.", ex);
            }

        }


        // GET All Student
        public List<StudentModel> GetAllUsers()
        {
            var users = new List<StudentModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM [Student]", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            users.Add(StudentMapper.MapStudent(reader));
                        }
                    }
                }
            }

            return users;
        }

        //
        public async Task<StudentModel> IsAuthenticateUser(LoginDto model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM [dbo].[Student] WHERE [Email] = @Email AND [Password] = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Email!.ToLower().Trim());
                    command.Parameters.AddWithValue("@Password", model.Password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var student = StudentMapper.MapStudent(reader);

                            return student;
                        }
                    }
                }
            }

            return null; // No student found
        }
        //
        public async Task<bool> IsEmailRegistered(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT COUNT(*) FROM Student WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                var result = await command.ExecuteScalarAsync();

                return (int)result > 0;
            }
        }























        // Generate Default Image in Byte
        private byte[] GetDefaultImageBytes()
        {
            // Assuming you have a default image stored in your project or as a resource
            // Load the default image file or generate a default image byte array
            // Option 1: Load default image file from disk
            string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "DefaultImage.jpg");
            // string defaultImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", "DefaultImage.jpg");
            byte[] defaultImageBytes = File.ReadAllBytes(defaultImagePath);
            return defaultImageBytes;
        }

    }
}
