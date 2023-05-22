using EduSpaceAPI.Models;
using EduSpaceAPI.MyDTOs;
using Microsoft.AspNetCore.Hosting;
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
        private string connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public UserRepository(IConfiguration configuration ,IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;   
        }
        // Inside UserRepository.cs

        // A methid to check User is Authentic
        public int IsAuthenticateUser(UserModel model)
        {
            int userId = -1; // Default value indicating invalid credentials
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserId FROM [User] WHERE Email = @Email AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", (model.Email.ToLower() != null) ? model.Email.ToLower() : DBNull.Value);
                    command.Parameters.AddWithValue("@Password", model.Password);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId  = reader.GetInt32(0); // Assuming UserId is stored as an int in the database
                            
                        }
                    }
                }
            }

            return userId;
        }
        // Inser user to the data base
        public bool InsertUser(UserModel user)
        {
           // (user.ImageName != null) ? user.ImageName : GetDefaultImageBytes();
            if (user  == null) 
                return false;
            else
            {
                Hashtable parameters = new Hashtable
                {
                    { "@Email", user.Email },
                    { "@Password", user.Password },
                    { "@UserRole", user.UserRole },
                    { "@FullName", user.FullName },
                    { "@VerificationCode", user.VerificationCode },
                    { "@UserImage", user.UserImage   },
                    { "@ContactNumber", user.ContactNumber }, 
                    { "@IsVerified", user.IsVerified },
                    { "@Address", user.Address },
                    { "@Resume", user.Resume },
                    { "@ImageName",  user.ImageName  },
                    { "@ResumeName", user.ResumeName }
                };
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("InsertUser", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Set the parameters
                            foreach (DictionaryEntry entry in parameters)
                            {
                                string parameterName = (string)entry.Key;
                                if (parameterName == "@UserImage" || parameterName == "@Resume")
                                {
                                    object parameterValue = (entry.Value != null) ? entry.Value : GetDefaultImageBytes();
                                    command.Parameters.Add(parameterName, SqlDbType.VarBinary, -1).Value = DBNull.Value;

                                }
                                else
                                {
                                    object parameterValue = (entry.Value != null) ? entry.Value : DBNull.Value;
                                    command.Parameters.AddWithValue(parameterName, parameterValue);
                                }
                            }
                            connection.Open();
                            command.ExecuteNonQuery();
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log the error, throw a custom exception, etc.)
                    // You can also rethrow the exception if you want to bubble it up to the caller
                    // For simplicity, we'll just throw a new exception with the original message
                    throw new Exception("An error occurred while inserting the user.", ex);
                }
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

    
