using EduSpaceAPI.Models;
using System.Data.SqlClient;

namespace EduSpaceAPI.Helpers
{
    public class StudentMapper
    {
        public static StudentModel MapStudent(SqlDataReader reader)
        {
            StudentModel student = new StudentModel();
            student.StudentId = Convert.ToInt32(reader["StudentId"]);
            student.FullName = reader["FullName"].ToString();
            student.Email = reader["Email"].ToString();
            student.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
            student.Department = reader["Department"].ToString();
            student.Program = reader["Program"].ToString();
            student.FatherName = reader["FatherName"].ToString();
            student.City = reader["City"].ToString();
            student.Address = reader["Address"].ToString();
            student.Session = reader["Session"].ToString();
            student.ContactNumber = reader["ContactNumber"].ToString();
            student.Semester = Convert.ToInt32(reader["Semester"]);
            student.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            student.RollNumber = reader["RollNumber"].ToString();

            // Check if the Image and ImagePath columns are not null
            if (!reader.IsDBNull(reader.GetOrdinal("Image")))
                student.Image = (byte[])reader["Image"];

            if (!reader.IsDBNull(reader.GetOrdinal("ImagePath")))
                student.ImagePath = reader["ImagePath"].ToString();

            // Handle DBNull values for UserImage column
            if (reader["Image"] != DBNull.Value)
            {
                student.Image = (byte[])reader["Image"];
            }
            else
            {
                student.Image = null;
            }

            return student;
        }

    }
}
