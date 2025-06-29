using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace StakingForm
{
    public static class LiquidityService
    {
        public static DataTable GetAllLiquidity()
        {
            try
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
            catch (MySqlException ex)
            {
                
                throw new ApplicationException("Gagal mengambil data dari database: " + ex.Message);
            }
        }

        

        public static void UpdateLiquidity(int id, string coin, decimal amount)
        {
            try
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
            catch (MySqlException ex)
            {
                throw new ApplicationException("Gagal memperbarui data: " + ex.Message);
            }
        }

        public static void DeleteLiquidity(int id)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM liquidity WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw new ApplicationException("Gagal menghapus data: " + ex.Message);
            }
        }
    }
}
