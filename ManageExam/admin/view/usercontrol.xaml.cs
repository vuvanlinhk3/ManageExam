using ManageExam.database;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for usercontrol.xaml
    /// </summary>
    public partial class usercontrol : Window
    {
        public usercontrol()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DSND,2);
            Panel.SetZIndex(AU,1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(AU,2);
            Panel.SetZIndex(DSND,1);
            
                Connection connection = new Connection();
                DataTable dataTable = connection.Select(); // Thực hiện truy vấn và lấy dữ liệu từ database

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    // Lấy tên người dùng từ dòng đầu tiên của DataTable
                    string username = dataTable.Rows[0]["IDUSER"].ToString();

                    // Hiển thị tên người dùng
                    Console.WriteLine("Username: " + username);

                    // Lưu tên người dùng vào biến a
                    string a = username;
                }
                else
                {
                    Console.WriteLine("Không có dữ liệu trả về từ truy vấn.");
                }

        }
    }
}
