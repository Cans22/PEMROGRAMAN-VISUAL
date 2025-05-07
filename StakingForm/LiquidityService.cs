using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace StakingForm
{
    public class LiquidityService
    {
        public static DataTable GetAllLiquidity()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, coin_name AS 'Coin', amount AS 'Amount' FROM liquidity", conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public static void AddLiquidity(string coin, decimal amount)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO liquidity (coin_name, amount) VALUES (@coin, @amount)", conn);
                cmd.Parameters.AddWithValue("@coin", coin);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateLiquidity(int id, string coin, decimal amount)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE liquidity SET coin_name = @coin, amount = @amount WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@coin", coin);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteLiquidity(int id)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM liquidity WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
