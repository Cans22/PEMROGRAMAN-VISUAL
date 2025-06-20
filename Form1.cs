using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.Json;
using System.Net.Http;


namespace StakingForm
{
    public partial class Form1 : Form
    {
        int selectedId = -1;
        public class WalletData
        {
            public string Type { get; set; }
            public string Address { get; set; }
            public string Balance { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

            cmbCoinName.Items.AddRange(new string[] { "BTC", "ETH", "SOL" });
            cmbCoinName.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCoinName.SelectedIndex = 0;

            LiquidityController.LoadLiquidity(dgvLiquidity);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                LiquidityController.AddLiquidity(cmbCoinName.SelectedItem.ToString(), txtAmount.Text);
                LiquidityController.LoadLiquidity(dgvLiquidity);
                txtAmount.Clear();
                cmbCoinName.SelectedIndex = 0;
                MessageBox.Show("Staking Added Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvLiquidity_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedId = Convert.ToInt32(dgvLiquidity.Rows[e.RowIndex].Cells["id"].Value);
                cmbCoinName.SelectedItem = dgvLiquidity.Rows[e.RowIndex].Cells["Coin"].Value.ToString();
                txtAmount.Text = dgvLiquidity.Rows[e.RowIndex].Cells["Amount"].Value.ToString();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Select data before edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var confirm = MessageBox.Show("Edit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                LiquidityController.UpdateLiquidity(selectedId, cmbCoinName.SelectedItem.ToString(), txtAmount.Text);
                LiquidityController.LoadLiquidity(dgvLiquidity);
                txtAmount.Clear();
                cmbCoinName.SelectedIndex = 0;
                selectedId = -1;
                MessageBox.Show("Staking Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Select data before delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var confirm = MessageBox.Show("Withdraw?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                LiquidityController.DeleteLiquidity(selectedId);
                LiquidityController.LoadLiquidity(dgvLiquidity);
                txtAmount.Clear();
                cmbCoinName.SelectedIndex = 0;
                selectedId = -1;
                MessageBox.Show("Withdraw Successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out _) && !string.IsNullOrWhiteSpace(txtAmount.Text))
                txtAmount.BackColor = Color.MistyRose;
            else
                txtAmount.BackColor = Color.SkyBlue;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Ambil wallet address dari file PHP
                    string walletAddress = await client.GetStringAsync("http://localhost/staking-api/wallet.txt");
                    walletAddress = walletAddress.Trim();

                    if (string.IsNullOrWhiteSpace(walletAddress) || walletAddress.Length < 10)
                    {
                        lblWalletStatus.Text = "❌ Wallet not connected";
                        lblBalance.Text = "-";
                        lblWalletAddress.Text = "-";
                        return;
                    }

                    // Wallet sudah terkoneksi
                    lblWalletStatus.Text = "Wallet connected";
                    lblWalletAddress.Text = "Wallet: " + walletAddress;

                    // Ambil saldo
                    string json = await client.GetStringAsync("http://localhost/staking-api/get_balance.php");
                    using var doc = JsonDocument.Parse(json);
                    string hexBalance = doc.RootElement.GetProperty("result").GetString();

                    // Konversi hex ke ETH
                    decimal wei = Convert.ToInt64(hexBalance, 16);
                    decimal eth = wei / 1000000000000000000m;

                    lblBalance.Text = "Balance: " + eth.ToString("0.0000") + " ETH";
                }
            }
            catch (Exception ex)
            {
                lblWalletStatus.Text = "Gagal load wallet";
                lblBalance.Text = "-";
                lblWalletAddress.Text = "-";
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnConnectWallet_Click(object sender, EventArgs e)
        {
            try
            {
                // URL ke wallet.html di folder XAMPP htdocs
                string walletUrl = "http://localhost/wallet.html";

                // Buka URL dengan browser default
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = walletUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal membuka browser: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        }

    }

