using System;
using System.Data;
using System.Windows;
using ManageExam.database;
using MySql.Data.MySqlClient;

namespace ManageExam.admin.view
{
    public partial class studentResult : Window
    {
        public studentResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.ItemsSource = null;
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            using (Connection conn = new Connection())
            {
                try
                {
                    conn.OpenConnection();
                    string sqlQuery = "SELECT * FROM ketqua";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, conn.connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            datagrid.ItemsSource = dataTable.DefaultView;
                            conn.CloseConnection();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error connecting to database: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
