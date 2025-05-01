using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StakingForm
{
    public partial class Form1 : Form
    {
        List<Liquidity> liquidityList = new List<Liquidity>();
        int selectedId = -1;
        int nextId = 1;

        public Form1()
        {
            InitializeComponent();

            // Tambah pilihan koin ke ComboBox
            cmbCoinName.Items.AddRange(new string[] { "BTC", "ETH", "SOL" });
            cmbCoinName.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCoinName.SelectedIndex = 0;

            RefreshGrid();
        }

        void RefreshGrid()
        {
            dgvLiquidity.DataSource = null;
            dgvLiquidity.DataSource = liquidityList.Select(l => new
            {
                l.Id,
                l.CoinName,
                l.Amount
            }).ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string coin = cmbCoinName.SelectedItem.ToString();

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Amount harus berupa angka.");
                return;
            }

            liquidityList.Add(new Liquidity
            {
                Id = nextId++,
                CoinName = coin,
                Amount = amount
            });

            RefreshGrid();
            txtAmount.Clear();
            cmbCoinName.SelectedIndex = 0;
        }


        private void dgvLiquidity_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedId = (int)dgvLiquidity.Rows[e.RowIndex].Cells["Id"].Value;
                var selected = liquidityList.FirstOrDefault(l => l.Id == selectedId);
                if (selected != null)
                {
                    cmbCoinName.SelectedItem = selected.CoinName;
                    txtAmount.Text = selected.Amount.ToString();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var liquidity = liquidityList.FirstOrDefault(l => l.Id == selectedId);
            if (liquidity != null && decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                liquidity.CoinName = cmbCoinName.SelectedItem.ToString();
                liquidity.Amount = amount;
                RefreshGrid();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var liquidity = liquidityList.FirstOrDefault(l => l.Id == selectedId);
            if (liquidity != null)
            {
                liquidityList.Remove(liquidity);
                RefreshGrid();
                txtAmount.Clear();
                cmbCoinName.SelectedIndex = 0;
                selectedId = -1;
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            // Kamu bisa isi validasi angka di sini, atau biarkan kosong
            // Contoh validasi:
            if (!decimal.TryParse(txtAmount.Text, out _) && !string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                txtAmount.BackColor = Color.MistyRose; // error input
            }
            else
            {
                txtAmount.BackColor = Color.SkyBlue; // valid
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

    