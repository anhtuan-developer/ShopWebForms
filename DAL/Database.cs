using System.Configuration;
using System.Data.SqlClient;

namespace web_ban_hang2.DAL
{
    public class Database
    {
        private readonly string connectionString;

        public Database()
        {
            connectionString =
                ConfigurationManager
                .ConnectionStrings["ShopWebFormsConnection"]
                .ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}