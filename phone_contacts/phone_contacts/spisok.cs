using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace phone_contacts
{
    public partial class spisok : Form
    {
        public spisok()
        {
            InitializeComponent();
        }

        private void spisok_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            phc phc = new phc();
            phc.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void spisok_Load_1(object sender, EventArgs e)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM contacts";

                    using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
