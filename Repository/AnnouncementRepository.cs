using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduSpaceAPI.Helpers;
using EduSpaceAPI.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnnouncementRepository 
    {
        private IConfiguration _configuration;
        private string _connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public AnnouncementRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<int> Addannouncement(AnnouncementModel announcement)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("INSERT INTO [dbo].[Announcement] ([Message], [UserName], [UserRole], [CreatedAt], [Title], [UserEmail])" +
                                                   "VALUES (@Message, @UserName, @UserRole, @CreatedAt, @Title, @UserEmail);" +
                                                   "SELECT CAST(scope_identity() AS int);", connection))
                {
                    command.Parameters.AddWithValue("@Message", announcement.Message);
                    command.Parameters.AddWithValue("@UserName", announcement.UserName);
                    command.Parameters.AddWithValue("@UserRole", announcement.UserRole);
                    command.Parameters.AddWithValue("@CreatedAt", announcement.CreatedAt);
                    command.Parameters.AddWithValue("@Title", announcement.Title);
                    command.Parameters.AddWithValue("@UserEmail", announcement.UserEmail);

                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<List<AnnouncementModel>> GetannouncementsByRole(string role)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM [dbo].[Announcement] WHERE [UserRole] = @UserRole", connection))
                {
                    command.Parameters.AddWithValue("@UserRole", role);

                    var announcements = new List<AnnouncementModel>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var announcement = new AnnouncementModel
                            {
                                AnnouncementId = (int)reader["AnnouncementId"],
                                Message = (string)reader["Message"],
                                UserName = (string)reader["UserName"],
                                UserRole = (string)reader["UserRole"],
                                CreatedAt = (DateTime)reader["CreatedAt"],
                                Title = (string)reader["Title"],
                                UserEmail = (string)reader["UserEmail"]
                            };

                            announcements.Add(announcement);
                        }
                    }

                    return announcements;
                }
            }
        }

        public async Task<int> Deleteannouncement(int announcementId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("DELETE FROM [dbo].[Announcement] WHERE [AnnouncementId] = @AnnouncementId", connection))
                {
                    command.Parameters.AddWithValue("@AnnouncementId", announcementId);

                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }


}

