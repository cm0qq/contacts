using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace phone_contacts
{
    public partial class phc : Form
    {
        

        public phc()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private string CleanPhoneNumber(string phoneNumber)
        {
            // Удалить все символы, кроме цифр
            return new string(phoneNumber.Where(char.IsDigit).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim(); // Удалить пробелы из имени
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Номер FROM contacts WHERE REPLACE(Имя, ' ', '') = @Имя";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Имя", name.Replace(" ", ""));

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            // Очистить номер от символов форматирования
                            string phoneNumber = CleanPhoneNumber(result.ToString());
                            maskedTextBox1.Text = phoneNumber;
                        }
                        else
                        {
                            MessageBox.Show("Номер не найден для указанного имени.");
                            maskedTextBox1.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            spisok spisok = new spisok();
            spisok.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string phnumber = maskedTextBox1.Text;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            try
            {

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO contacts (Имя, Номер) VALUES (@Имя, @Номер)";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Имя", name);
                        command.Parameters.AddWithValue("@Номер", phnumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись успешно добавлена!");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при добавлении записи.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string phnumber = maskedTextBox1.Text;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            try
            {

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();


                    string deleteQuery = "DELETE FROM contacts WHERE Имя = @Имя OR Номер = @Номер";


                    using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Имя", name);
                        command.Parameters.AddWithValue("@Номер", phnumber);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Записи успешно удалены!");
                        }
                        else
                        {
                            MessageBox.Show("Записи не найдены для удаления.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim(); // Удалить пробелы из имени
            string phnumber = maskedTextBox1.Text;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE contacts SET Номер = @Номер WHERE REPLACE(Имя, ' ', '') = @Имя";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Имя", name.Replace(" ", ""));

                        // Очистить номер от символов форматирования
                        string phoneNumber = CleanPhoneNumber(phnumber);
                        command.Parameters.AddWithValue("@Номер", phoneNumber);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Изменения сохранены!");
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена для указанного имени.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string phnumber = maskedTextBox1.Text;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT Имя FROM contacts WHERE Номер = @Номер";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Номер", phnumber);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            textBox1.Text = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Имя не найден для указанного номера.");
                            textBox1.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string phnumber = maskedTextBox1.Text;
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE contacts SET Имя = @Имя WHERE Номер = @Номер";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Номер", phnumber);
                        command.Parameters.AddWithValue("@Имя", name);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Изменения сохранены!");
                        }
                        else
                        {
                            MessageBox.Show("Запись не найдена для указанного имени.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
