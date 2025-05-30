using System;
using System.Data;
using System.Windows.Forms;

namespace StakingForm
{
    public static class LiquidityController
    {
        public static void LoadLiquidity(DataGridView dgv)
        {
            try
            {
                var data = LiquidityService.GetAllLiquidity();
                dgv.DataSource = data;

                if (dgv.Columns.Contains("id"))
                {
                    dgv.Columns["id"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void AddLiquidity(string coin, string amountText)
        {
            if (!decimal.TryParse(amountText, out decimal amount))
                throw new ArgumentException("Fill Amount");

            LiquidityService.AddLiquidity(coin, amount);
        }

        public static void UpdateLiquidity(int id, string coin, string amountText)
        {
            if (!decimal.TryParse(amountText, out decimal amount))
                throw new ArgumentException("Jumlah tidak valid");

            LiquidityService.UpdateLiquidity(id, coin, amount);
        }

        public static void DeleteLiquidity(int id)
        {
            LiquidityService.DeleteLiquidity(id);
        }
    }
}
