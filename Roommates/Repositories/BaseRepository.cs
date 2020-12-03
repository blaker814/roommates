using Microsoft.Data.SqlClient;

namespace Roommates.Repositories
{
    public class BaseRepository
    {
        private string _connectionString;
        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected SqlConnection Connection => new SqlConnection(_connectionString);     
    }
}
