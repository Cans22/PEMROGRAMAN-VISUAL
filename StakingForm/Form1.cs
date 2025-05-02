using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace StakingForm
{
    public partial class Form1 : Form
    {
        string connStr = "server=localhost;user=root;password=;database=staking_db;";
        int selectedId = -1;

        public Form1()
        {
            InitializeComponent();

            cmbCoinName.Items.AddRange(new string[] { "BTC", "ETH", "SOL" });
            cmbCoinName.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCoinName.SelectedIndex = 0;

            LoadData();
        }

        private void LoadData()
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, coin_name AS 'Coin', amount AS 'Amount' FROM liquidity", conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dgvLiquidity.DataSource = table;
                dgvLiquidity.DataSource = table;
                if (dgvLiquidity.Columns.Contains("id"))
                {
                    dgvLiquidity.Columns["id"].Visible = false;
                }

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string coin = cmbCoinName.SelectedItem.ToString();

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Fill Number", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO liquidity (coin_name, amount) VALUES (@coin, @amount)", conn);
                cmd.Parameters.AddWithValue("@coin", coin);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.ExecuteNonQuery();
            }

            LoadData();
            txtAmount.Clear();
            cmbCoinName.SelectedIndex = 0;

            MessageBox.Show("Staking Added Succesfully.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Fill Number", "Wrong Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE liquidity SET coin_name = @coin, amount = @amount WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@coin", cmbCoinName.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.ExecuteNonQuery();
            }

            LoadData();
            txtAmount.Clear();
            cmbCoinName.SelectedIndex = 0;
            selectedId = -1;

            MessageBox.Show("Staking Update Succesfully", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Select Data Before Edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var confirm = MessageBox.Show("Withdraw?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM liquidity WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", selectedId);
                cmd.ExecuteNonQuery();
            }

            LoadData();
            txtAmount.Clear();
            cmbCoinName.SelectedIndex = 0;
            selectedId = -1;

            MessageBox.Show("Withdraw Succesfully.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out _) && !string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                txtAmount.BackColor = Color.MistyRose;
            }
            else
            {
                txtAmount.BackColor = Color.SkyBlue;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Tidak perlu isi apa pun kalau tidak digunakan
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Tidak perlu isi apa pun kalau tidak digunakan
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Tidak perlu isi apa pun kalau tidak digunakan
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Kosongkan atau isi sesuai kebutuhan
            // MessageBox.Show("Button 2 clicked!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kosongkan atau isi sesuai kebutuhan
            // MessageBox.Show("Button 1 clicked!");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Kosongkan atau isi sesuai kebutuhan
            // MessageBox.Show("Button 4 clicked!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Kosongkan atau isi sesuai kebutuhan
            // MessageBox.Show("Button 3 clicked!");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kosongkan atau isi sesuai kebutuhan
            // MessageBox.Show("Cell clicked!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Tidak perlu isi apa pun kalau tidak digunakan
        }



    }

    public class Liquidity
    {
        public int Id { get; set; }
        public string CoinName { get; set; }
        public decimal Amount { get; set; }
    }
}

    