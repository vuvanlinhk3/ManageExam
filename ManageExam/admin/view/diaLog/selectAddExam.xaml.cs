using ManageExam.database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManageExam.admin.view.diaLog
{
    /// <summary>
    /// Interaction logic for selectAddExam.xaml
    /// </summary>
    public partial class selectAddExam : Window
    {
        public selectAddExam()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        DateTime timestamp = DateTime.Now;
                        string action = "Tin tức mới : " + tintuc.Text;

                        string insertActionQuery = "INSERT INTO tintuc (hanhdong, time) VALUES (@Action, @Timestamp)";
                        MySqlCommand insertActionCommand = new MySqlCommand(insertActionQuery, conn.connection);
                        insertActionCommand.Parameters.AddWithValue("@Action", action);
                        insertActionCommand.Parameters.AddWithValue("@Timestamp", timestamp);
                        insertActionCommand.ExecuteNonQuery();
                        conn.connection.Close();
                        DialogResult = true;

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
