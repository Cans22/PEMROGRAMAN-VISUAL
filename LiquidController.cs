using System;
using System.Data;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StakingForm
{
    public static class LiquidityController
    {
        public static void LoadLiquidity(DataGridView dgv)
        {
            using (var client = new WebClient())
            {
                string json = client.DownloadString("http://localhost/staking-api/get_staking.php");

                var table = JsonConvert.DeserializeObject<DataTable>(json);
                dgv.DataSource = table;

                if (dgv.Columns.Contains("id"))
                {
                    dgv.Columns["id"].Visible = false;
                }
            }

        }

        public static void AddLiquidity(string coin, string amountText)
        {
            if (!decimal.TryParse(amountText, out decimal amount))
                throw new ArgumentException("Fill Amount");

            using (var client = new WebClient())
            {
                var values = new System.Collections.Specialized.NameValueCollection();
                values["coin"] = coin;
                values["amount"] = amount.ToString();
                client.UploadValues("http://localhost/staking-api/save_stake.php", "POST", values);
            }
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
