using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using EduSpaceAPI.MyDTOs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class UserRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public UserRepository(IConfiguration configuration ,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;   
        }
        // Inside UserRepository.cs
        public async Task<bool> IsEmailRegistered(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT COUNT(*) FROM User WHERE Email = @Email";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                var result = await command.ExecuteScalarAsync();

                return (int)result > 0;
            }
        }

        // A methid to check User is Authentic
        public MyResponse<UserModel> IsAuthenticateUser(LoginDto model)
        {
          
            UserModel user = new UserModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT UserId,UserRole,Email,FullName  FROM [User] WHERE Email = @Email AND Password = @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", (model.Email!.ToLower() != null) ? model.Email.ToLower() : DBNull.Value);
                        command.Parameters.AddWithValue("@Password", (model.Password != null) ? model.Password : DBNull.Value);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user.UserId = reader.GetInt32(0); // Assuming UserId is stored as an int in the database
                                user.UserRole = reader.GetString(1);
                                user.Email = reader.GetString(2);
                                user.FullName = reader.GetString(3);
                                return new MyResponse<UserModel>()
                                {
                                    Response = user,
                                    IsSuccess = true
                                };
                            }
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
                return new MyResponse<UserModel>()
                {
                    Message = "Exception Occur Due to " + ex.Message,
                    IsSuccess = false
                };
                throw new Exception("An error occurred while inserting the user.", ex);
            }
            return new MyResponse<UserModel>()
            {
                Response = user,
                IsSuccess = false
            };
        }
        // Inser user to the data base
        public MyResponse<string> InsertUser(UserModel user)
        {
            int rowsAffected = 0;
                Hashtable parameters = new Hashtable
                {
                    { "@Email", user.Email },
                    { "@Password", user.Password },
                    { "@UserRole", user.UserRole },
                    { "@FullName", user.FullName },
                    { "@VerificationCode", user.VerificationCode },
                    { "@UserImage", user.UserImage },
                    { "@ContactNumber", user.ContactNumber }, 
                    { "@IsVerified", user.IsVerified },
                    { "@Address", user.Address },
                    { "@Resume", user.Resume },
                    { "@ImagePath",  user.ImagePath },
                    { "@ResumePath", user.ResumePath },
                    { "@City",  user.City },
                    { "@CreatedAt", user.CreatedAt },
                    { "@Gender", user.Gender },
                    { "@DateOfBirth", user.DateOfBirth },
                    { "@Qualification", user.Qualification }
                };
        
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand("InsertUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Set the parameters
                        foreach (DictionaryEntry entry in parameters)
                        {
                            string parameterName = (string)entry.Key;
                            if (parameterName == "@UserImage")
                            {
                                if (user.ImagePath == null)
                                {
                                object parameterValue = (entry.Value != null) ? entry.Value : GetDefaultImageBytes();
                                command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = parameterValue;
                                }
                                else
                                {
                                    object parameterValue = user.UserImage!;
                                    command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = parameterValue;
                                }

                            }
                            else if (parameterName == "@Resume")
                            {
                                if(user.ResumePath == null)
                                {

                                object parameterValue = (entry.Value != null) ? entry.Value : GetDefaultImageBytes();
                                command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = parameterValue;
                                }else
                                {
                                    object parameterValue = user.Resume!;
                                    command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = parameterValue;

                                }

                            }
                            else if(parameterName == "@CreatedAt")
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
        public List<UserModel> GetAllUsers()
        {
            var users = new List<UserModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM [User]", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                         
                            users.Add(UserMapper.MapUser(reader));
                        }
                    }
                }
            }

            return users;
        }

        // get User bt id 
        public MyResponse<UserModel> GetUserById(int userId)
        {
            // Replace with your actual connection string

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SelectUserById", connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add the UserId parameter to the command
                command.Parameters.AddWithValue("@UserId", userId);

                UserModel user = null;

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // Check if the reader has any rows
                        {
                            if (reader.Read())
                            {
                                // Map the reader data to the User object
                                user = UserMapper.MapUser(reader);
                            }
                        } else
                        {
                            return new MyResponse<UserModel>()
                            {
                                IsSuccess = false,
                                Message = "User Not Exist"
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine(ex.Message);
                }

                return new MyResponse<UserModel>()
                {
                    Response = user,
                    IsSuccess = true,
                    Message = "User Found"
                };
            }
        }
        public byte[] GetUserImageByImagePath(string imagePath)
        {
          
            byte[] userImage = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT [UserImage] FROM [dbo].[User] WHERE [ImagePath] = @ImagePath";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImagePath", imagePath);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                userImage = (byte[])reader["UserImage"];
                            }
                        }
                    }
                }
            }

            return userImage;
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
        private byte[] GetImageBytes(string path)
        {
            // Assuming you have a default image stored in your project or as a resource
            // Load the default image file or generate a default image byte array
            // Option 1: Load default image file from disk
           // string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "DefaultImage.jpg");
             string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", path);
            byte[] defaultImageBytes = File.ReadAllBytes(defaultImagePath);
            return defaultImageBytes;
        }

    }
}

    
