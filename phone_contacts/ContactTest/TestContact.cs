using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Reflection;
using System.Data.Common;
using Npgsql;

namespace ContactTest
{
    [TestClass]
    public class TestContact
    {
        [TestMethod]
        public void TestDatabaseConnection()
        {
            // Arrange
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            // Act
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Assert
                    Assert.AreEqual(ConnectionState.Open, connection.State);
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.Fail($"Failed to connect to the database. Error: {ex.Message}");
                }
            }
        }

        [TestMethod]
        public void TestDbSelect()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            // Act
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT * FROM contacts";

                    using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectQuery, connection))
                    {
                        DataTable dataTable = new DataTable();
                        dataAdapter.Fill(dataTable);
                        Assert.IsTrue(dataTable.Rows.Count > 0, "Таблица не содержит строк");
                        Assert.IsNotNull(dataTable.Rows[0]["Имя"], "Столбец 'Имя' не должен быть пустым");
                        Assert.IsNotNull(dataTable.Rows[0]["Номер"], "Столбец 'Номер' не должен быть пустым");
                    }

                    // Assert
                    Assert.AreEqual(ConnectionState.Open, connection.State);

                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.Fail($"Failed to connect to the database. Error: {ex.Message}");
                }
            }
        }

        [TestMethod]
        public void TestDbInsert()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            // Act
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string name = "пользователь";
                    string phoneNumber = "9519523456";

                    string insertQuery = "INSERT INTO contacts (Имя, Номер) VALUES (@param1, @param2)";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("param1", name);
                        command.Parameters.AddWithValue("param2", phoneNumber);

                        // Выполнить запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // Assert
                        Assert.AreEqual(ConnectionState.Open, connection.State);
                        Assert.AreEqual(1, rowsAffected, "Ожидалась одна вставленная строка");

                        // Проверка, что данные действительно добавлены в базу данных
                        string selectQuery = "SELECT * FROM contacts WHERE Имя = @param1 AND Номер = @param2";

                        using (NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(selectQuery, connection))
                        {
                            dataAdapter.SelectCommand.Parameters.AddWithValue("param1", name);
                            dataAdapter.SelectCommand.Parameters.AddWithValue("param2", phoneNumber);

                            DataTable dataTable = new DataTable();
                            dataAdapter.Fill(dataTable);

                            Assert.IsTrue(dataTable.Rows.Count > 0, "Таблица не содержит добавленной строки");
                            Assert.AreEqual(name, dataTable.Rows[0]["Имя"].ToString().Trim(), "Неверное значение в столбце 'Имя'");
                            Assert.AreEqual(phoneNumber, dataTable.Rows[0]["Номер"].ToString().Trim(), "Неверное значение в столбце 'Номер'");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Assert
                    Assert.Fail($"Failed to execute the insert query or verify the inserted data. Error: {ex.Message}");
                }
            }
        }

        [TestMethod]
        public void TestDbUpdate()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Предполагается, что в базе данных уже есть запись с именем "TestUpdate" и номером "1111111111"
                    string initialName = "TestUpdate";
                    string updatedPhoneNumber = "9876543210";

                    // Обновление номера телефона
                    string updateQuery = "UPDATE contacts SET Номер = @param2 WHERE REPLACE(Имя, ' ', '') = @param1";

                    using (NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("param1", initialName.Replace(" ", ""));
                        updateCommand.Parameters.AddWithValue("param2", updatedPhoneNumber);

                        // Выполнить запрос
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        // Assert
                        Assert.AreEqual(ConnectionState.Open, connection.State);
                        Assert.AreEqual(1, rowsAffected, "Ожидалась одна обновленная строка");
                    }

                    // Проверка, что данные действительно обновлены в базе данных
                    string selectQuery = "SELECT COUNT(*) FROM contacts WHERE REPLACE(Имя, ' ', '') = @param1 AND Номер = @param2";

                    using (NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("param1", initialName.Replace(" ", ""));
                        selectCommand.Parameters.AddWithValue("param2", updatedPhoneNumber);

                        int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                        // Assert
                        Assert.AreEqual(1, count, "Ожидалась одна обновленная строка");
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Failed to execute the update query or verify the updated data. Error: {ex.Message}");
                }
            }
        }

        [TestMethod]
        public void TestDbDelete()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1111;Database=phoncon";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string nameToDelete = "TestDelete";
                    string phoneNumberToDelete = "1234567890";

                    // Удаление записи
                    string deleteQuery = "DELETE FROM contacts WHERE REPLACE(Имя, ' ', '') = @param1 OR Номер = @param2";

                    using (NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("param1", nameToDelete.Replace(" ", ""));
                        deleteCommand.Parameters.AddWithValue("param2", phoneNumberToDelete);

                        // Выполнить запрос
                        int rowsAffected = deleteCommand.ExecuteNonQuery();

                        // Assert
                        Assert.AreEqual(ConnectionState.Open, connection.State);
                        Assert.AreEqual(1, rowsAffected, "Ожидалось удаление одной строки");
                    }

                    // Проверка, что данные действительно удалены из базы данных
                    string selectQuery = "SELECT COUNT(*) FROM contacts WHERE REPLACE(Имя, ' ', '') = @param1 OR Номер = @param2";

                    using (NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("param1", nameToDelete.Replace(" ", ""));
                        selectCommand.Parameters.AddWithValue("param2", phoneNumberToDelete);

                        int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                        // Assert
                        Assert.AreEqual(0, count, "Ожидалось, что запись удалена");
                    }
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Failed to execute the delete query or verify the deleted data. Error: {ex.Message}");
                }
            }
        }
    }
}
