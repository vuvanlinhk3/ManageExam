using MySql.Data.MySqlClient;
using System.Data;
using ManageExam.database;
using System;


namespace ManageExam.database
{
    internal class Connection : IDisposable
    {
        public MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        // Constructor
        public Connection()
        {
            Initialize();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }
        }
        // Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "sdfsfffff";
            uid = "root";
            password = "";

            string connectionString;
            connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

        // Open connection to the database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException)
            {
                // Handle exception
                return false;
            }
        }

        // Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                // Handle exception
                return false;
            }
        }

        // Example method to execute query
        public DataTable Select()
        {
            string query = "SELECT * FROM user";

            // DataTable to hold the result
            DataTable dataTable = new DataTable();

            // Open connection
            if (this.OpenConnection())
            {
                // Create MySqlCommand
                MySqlCommand cmd = new MySqlCommand(query, connection);

                // Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                // Load DataTable
                dataTable.Load(dataReader);

                // Close DataReader
                dataReader.Close();

                // Close connection
                this.CloseConnection();

                // Return DataTable
                return dataTable;
            }
            else
            {
                // Return null if cannot open connection
                return null;
            }
        }
    }
}
