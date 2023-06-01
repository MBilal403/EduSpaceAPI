using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class DepartmentRepository
    {
        private IConfiguration _configuration;
        private string connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public DepartmentRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        // Add new Department 
        public MyResponse<DepartmentModel> AddDepartment(Models.DepartmentModel department)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("InsertDepartment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DepartName", department.DepartName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@InchargeName", department.InchargeName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AdminName", department.AdminName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CreatedAt", department.CreatedAt);
                    command.Parameters.AddWithValue("@Status", department.Status);
                    command.ExecuteNonQuery();
                }
            }
            return new MyResponse<DepartmentModel>()
            {
                Message = "Department Add Succesfully",
                IsSuccess = true
            };
        }
        // get All Department
        public IEnumerable<DepartmentModel> GetDepartments()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT DepartId, DepartName, InchargeName, AdminName, CreatedAt, Status FROM Department", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<DepartmentModel> departments = new List<DepartmentModel>();
                        while (reader.Read())
                        {
                            departments.Add(new DepartmentModel
                            {
                                DepartId = (int)reader["DepartId"],
                                DepartName = reader["DepartName"] != DBNull.Value ? (string)reader["DepartName"] : null,
                                InchargeName = reader["InchargeName"] != DBNull.Value ? (string)reader["InchargeName"] : null,
                                AdminName = reader["AdminName"] != DBNull.Value ? (string)reader["AdminName"] : null,
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                Status = (bool)reader["Status"]
                            });
                        }
                        return departments;
                    }
                }
            }
        }


    }

}
