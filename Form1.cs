using System;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Forms;

namespace StakingForm
{
    public partial class Form1 : Form
    {
        int selectedId = -1;

        public Form1()
        {
            InitializeComponent();
            cmbCoinName.Items.AddRange(new string[] { "SEP" });
            cmbCoinName.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCoinName.SelectedIndex = 0;

            LiquidityController.LoadLiquidity(dgvLiquidity);
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await UpdateWalletInfo();
        }

        private async System.Threading.Tasks.Task UpdateWalletInfo()
        {
            try
            {
                using var client = new HttpClient();

                // Ambil alamat wallet dari database melalui PHP
                string jsonWallet = await client.GetStringAsync("http://localhost/staking-api/get_wallet.php");
                using var docWallet = JsonDocument.Parse(jsonWallet);
                string walletAddress = docWallet.RootElement.GetProperty("address").GetString();

                if (string.IsNullOrWhiteSpace(walletAddress) || walletAddress.Length < 10)
                {
                    lblWalletStatus.Text = "❌ Wallet not connected";
                    lblBalance.Text = "-";
                    return;
                }

                lblWalletStatus.Text = $"✅ Wallet connected";
                lblBalance.Text = "Loading balance...";

                // Ambil balance melalui get_balance.php (Alchemy)
                string jsonBalance = await client.GetStringAsync("http://localhost/staking-api/get_balance.php");
                using var docBalance = JsonDocument.Parse(jsonBalance);
                string hexBalance = docBalance.RootElement.GetProperty("result").GetString();

                decimal wei = Convert.ToInt64(hexBalance, 16);
                decimal eth = wei / 1_000_000_000_000_000_000m;

                lblBalance.Text = $"Balance: {eth:0.0000} ETH";
            }
            catch (Exception ex)
            {
                lblWalletStatus.Text = "⚠️ Failed Get wallet";
                lblBalance.Text = "-";
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnConnectWallet_Click(object sender, EventArgs e)
        {
            try
            {
                // Buka wallet.html lewat browser default
                string walletUrl = "http://localhost/wallet.html";
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = walletUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Open Browser: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string amount = txtAmount.Text;

            if (string.IsNullOrWhiteSpace(amount))
            {
                MessageBox.Show("Fill Amount");
                return;
            }

            try
            {
                // Kirim parameter amount ke wallet.html melalui URL
                string walletUrl = $"http://localhost/wallet.html?amount={amount}";

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = walletUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Open Browser: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvLiquidity_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedId = Convert.ToInt32(dgvLiquidity.Rows[e.RowIndex].Cells["id"].Value);
                cmbCoinName.SelectedItem = dgvLiquidity.Rows[e.RowIndex].Cells["coin"].Value.ToString();
                txtAmount.Text = dgvLiquidity.Rows[e.RowIndex].Cells["amount"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Select data before edit.");
                return;
            }

            var confirm = MessageBox.Show("Edit?", "Confirmation", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                LiquidityController.UpdateLiquidity(selectedId, cmbCoinName.SelectedItem.ToString(), txtAmount.Text);
                LiquidityController.LoadLiquidity(dgvLiquidity);
                txtAmount.Clear();
                cmbCoinName.SelectedIndex = 0;
                selectedId = -1;
                MessageBox.Show("Staking Updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Select data before delete.");
                return;
            }

            var confirm = MessageBox.Show("Withdraw?", "Confirmation", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                LiquidityController.DeleteLiquidity(selectedId);
                LiquidityController.LoadLiquidity(dgvLiquidity);
                txtAmount.Clear();
                cmbCoinName.SelectedIndex = 0;
                selectedId = -1;
                MessageBox.Show("Withdraw Successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Error");
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out _) && !string.IsNullOrWhiteSpace(txtAmount.Text))
                txtAmount.BackColor = Color.MistyRose;
            else
                txtAmount.BackColor = Color.SkyBlue;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
              
                LiquidityController.LoadLiquidity(dgvLiquidity);

            
                await UpdateWalletInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal refresh: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
