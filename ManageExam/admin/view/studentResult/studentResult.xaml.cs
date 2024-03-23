using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for studentResult.xaml
    /// </summary>
    public partial class studentResult : Window
    {
        string connectstring = @"";
        SqlConnection connection;
        SqlCommand cmd;
        SqlDataAdapter adt;
        DataTable dt = new DataTable();
        public studentResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(connectstring);
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                cmd = new SqlCommand("select * from ketqua", connection);
                adt = new SqlDataAdapter(cmd);
                adt.Fill(dt);
                datagrid.ItemsSource = dt.DefaultView;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
