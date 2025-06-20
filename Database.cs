using MySql.Data.MySqlClient;

namespace StakingForm
{
    public static class Database
    {
        private static string connStr = "server=localhost;user=root;password=;database=staking_db;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}
