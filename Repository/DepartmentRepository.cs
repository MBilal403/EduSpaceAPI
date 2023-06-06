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
        private string _connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public DepartmentRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        // Add new Department 
        public MyResponse<DepartmentModel> AddDepartment(DepartmentModel department)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
        // Get department only where status is true that department are active
        public IEnumerable<DepartmentModel> GetActiveDepartments()
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM [Department] WHERE [Status] = 1", connection))
                {
                    List<DepartmentModel> departments = new List<DepartmentModel>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DepartmentModel department = new DepartmentModel
                            {
                                DepartId = Convert.ToInt32(reader["DepartId"]),
                                DepartName = Convert.ToString(reader["DepartName"]),
                                InchargeName = Convert.ToString(reader["InchargeName"]),
                                AdminName = Convert.ToString(reader["AdminName"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Status = Convert.ToBoolean(reader["Status"])
                            };

                            departments.Add(department);
                        }
                    }

                    return departments;
                }
            }

        }
        public void UpdateInchargeAndAdminNames(int DepartId, string inchargeName, string adminName)
        {
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE [dbo].[Department] " +
                             "SET [InchargeName] = @InchargeName, [AdminName] = @AdminName " +
                             "WHERE [DepartId] = @DepartId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@InchargeName", inchargeName);
                    command.Parameters.AddWithValue("@AdminName", adminName);
                    command.Parameters.AddWithValue("@DepartId", DepartId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        // Get department only where status is true that department are non active
        public IEnumerable<DepartmentModel> GetNonActiveDepartments()
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM [Department] WHERE [Status] = 0", connection))
                {
                    List<DepartmentModel> departments = new List<DepartmentModel>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DepartmentModel department = new DepartmentModel
                            {
                                DepartId = Convert.ToInt32(reader["DepartId"]),
                                DepartName = Convert.ToString(reader["DepartName"]),
                                InchargeName = Convert.ToString(reader["InchargeName"]),
                                AdminName = Convert.ToString(reader["AdminName"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Status = Convert.ToBoolean(reader["Status"])
                            };

                            departments.Add(department);
                        }
                    }

                    return departments;
                }
            }

        }
        // Get department only // Retrieve departments where either InchargeName or AdminName is null
        public IEnumerable<DepartmentModel> GetDepartmentsWithNullNames()
        {

            string query = "SELECT * FROM [Department] WHERE [InchargeName] IS NULL OR [AdminName] IS NULL";
            return ExecuteQuery(query);
        }
        private IEnumerable<DepartmentModel> ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    List<DepartmentModel> departments = new List<DepartmentModel>();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DepartmentModel department = new DepartmentModel
                            {
                                DepartId = Convert.ToInt32(reader["DepartId"]),
                                DepartName = Convert.ToString(reader["DepartName"]),
                                InchargeName = Convert.ToString(reader["InchargeName"]),
                                AdminName = Convert.ToString(reader["AdminName"]),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                                Status = Convert.ToBoolean(reader["Status"])
                            };

                            departments.Add(department);
                        }
                    }

                    return departments;
                }
            }

        }
        // Update the department by id 
        public void UpdateDepartment(DepartmentModel department)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE [Department] SET [DepartName] = @DepartName, [InchargeName] = @InchargeName, " +
                               "[AdminName] = @AdminName, [CreatedAt] = @CreatedAt, [Status] = @Status " +
                               "WHERE [DepartId] = @DepartId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DepartName", department.DepartName);
                    command.Parameters.AddWithValue("@InchargeName", department.InchargeName);
                    command.Parameters.AddWithValue("@AdminName", department.AdminName);
                    command.Parameters.AddWithValue("@CreatedAt", department.CreatedAt);
                    command.Parameters.AddWithValue("@Status", department.Status);
                    command.Parameters.AddWithValue("@DepartId", department.DepartId);
                    command.ExecuteNonQuery();
                }
            }
        }
        // Update the department status
        public void UpdateDepartmentStatus(int departId, bool status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE [dbo].[Department] " +
                                "SET [InchargeName] = NULL, " +
                                "[AdminName] = NULL, " +
                                "[Status] = @Status " +
                                "WHERE [DepartID] = @DepartId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@DepartId", departId);
                    command.ExecuteNonQuery();
                }
                string updateQuery = "UPDATE [fyppugc].[dbo].[Program] " +
                               "SET [Status] = @status " +
                               "WHERE [DepartFId] = @departId";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@departId", departId);
                    command.ExecuteNonQuery();
                }
            }


        }
        // Update departemnt name
        public void UpdateDepartName(int departmentId, string newDepartName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE [dbo].[Department] " +
                             "SET [DepartName] = @DepartName " +
                             "WHERE [DepartId] = @DepartId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DepartName", newDepartName);
                    command.Parameters.AddWithValue("@DepartId", departmentId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
