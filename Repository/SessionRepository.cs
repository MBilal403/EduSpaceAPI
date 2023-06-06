using EduSpaceAPI.Models;
using System.Data.SqlClient;

namespace EduSpaceAPI.Repository
{
    public class SessionRepository
    {
        private IConfiguration _configuration;
        private string _connectionString;
        IWebHostEnvironment _webHostEnvironment;
        public SessionRepository(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionString:DBx"];
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IEnumerable<SessionModel>> GetAllSessionsAsync()
        {
            var sessions = new List<SessionModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM [Session]";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var session = new SessionModel
                        {
                            SessionId = Convert.ToInt32(reader["SessionId"]),
                            SessionStart = Convert.ToDateTime(reader["SessionStart"]),
                            SessionEnd = Convert.ToDateTime(reader["SessionEnd"])
                        };

                        sessions.Add(session);
                    }
                }
            }

            return sessions;
        }

        public async Task<SessionModel> GetSessionByIdAsync(int SessionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM [Session] WHERE SessionId = @SessionId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionId", SessionId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var session = new SessionModel
                            {
                                SessionId = Convert.ToInt32(reader["SessionId"]),
                                SessionStart = Convert.ToDateTime(reader["SessionStart"]),
                                SessionEnd = Convert.ToDateTime(reader["SessionEnd"])
                            };

                            return session;
                        }
                    }
                }
            }

            return null;
        }

        public async Task<int> AddSessionAsync(SessionModel session)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO [Session] ([SessionStart], [SessionEnd]) VALUES (@SessionStart, @SessionEnd); SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionStart", session.SessionStart);
                    command.Parameters.AddWithValue("@SessionEnd", session.SessionEnd);

                    int insertedId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return insertedId;
                }

            }
        }
        public async Task UpdateSessionAsync(int SessionId, SessionModel session)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE [Session] SET [SessionStart] = @SessionStart, [SessionEnd] = @SessionEnd WHERE [SessionId] = @SessionId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionStart", session.SessionStart);
                    command.Parameters.AddWithValue("@SessionEnd", session.SessionEnd);
                    command.Parameters.AddWithValue("@SessionId", SessionId);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Session not found."); // Handle the case where the session with the specified ID is not found
                    }
                }
            }
        }

        public async Task DeleteSessionAsync(int SessionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM [Session] WHERE SessionId = @SessionId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionId", SessionId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
