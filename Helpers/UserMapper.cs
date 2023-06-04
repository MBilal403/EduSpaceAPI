using EduSpaceAPI.Models;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Threading;

namespace EduSpaceAPI.Helpers
{
    public class UserMapper
    {
        public static UserModel MapUser(SqlDataReader reader)
        {
            UserModel user = new UserModel
            {
                UserId = (int)reader["UserId"],
                Email = reader["Email"].ToString(),
                Password = reader["Password"].ToString(),
                UserRole = reader["UserRole"].ToString(),
                FullName = reader["FullName"].ToString(),
                VerificationCode = reader["VerificationCode"].ToString(),
                ContactNumber = reader["ContactNumber"].ToString(),
            Address = reader["Address"].ToString(),
                ImagePath = reader["ImagePath"].ToString(),
                ResumePath = reader["ResumePath"].ToString(),
                City = reader["City"].ToString(),
                CreatedAt = (DateTime)reader["CreatedAt"],
                Gender = reader["Gender"].ToString()!,
                DateOfBirth = (DateTime)reader["DateOfBirth"],
                Qualification =reader["Qualification"].ToString()!
            };
            // Handle DBNull values for UserImage column
            if (reader["UserImage"] != DBNull.Value)
            {
                user.UserImage = (byte[])reader["UserImage"];
            }
            else
            {
                user.UserImage = null;  
            }

            // Handle DBNull values for Resume column
            if (reader["Resume"] != DBNull.Value)
            {
                user.Resume = (byte[])reader["Resume"];
            }
            else
            {
                user.Resume = null;
            }

            return user;
        }
      

    }
}
