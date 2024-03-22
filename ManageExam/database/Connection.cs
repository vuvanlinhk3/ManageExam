using MySql.Data.MySqlClient;
using System.Data;

namespace ManageExam.database
{
    internal class Connection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        // Constructor
        public Connection()
        {
            Initialize();
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
            catch (MySqlException ex)
            {
                // Handle exception
                return false;
            }
        }

        // Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
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
