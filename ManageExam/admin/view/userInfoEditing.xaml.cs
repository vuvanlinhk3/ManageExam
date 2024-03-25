using MySql.Data.MySqlClient;
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
using ManageExam.database;

namespace ManageExam.admin.view.mUser
{
    /// <summary>
    /// Interaction logic for edituser.xaml
    /// </summary>
    public partial class edituser : Window
    {
        public edituser()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void screenMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void screenClose_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            using (Connection conn = new Connection())
            {
                if (string.IsNullOrWhiteSpace(HovaTen.Text) ||
                    string.IsNullOrWhiteSpace(gioiTinh.Text) ||
                    string.IsNullOrWhiteSpace(birthInput.Text) ||
                    string.IsNullOrWhiteSpace(quocTich.Text) ||
                    string.IsNullOrWhiteSpace(tinhTP.Text) ||
                    string.IsNullOrWhiteSpace(quanHuyen.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                try
                {
                    conn.OpenConnection();
                    string dataInsert = "INSERT INTO user (firstLastName, gioitinhUser, ngaysinhUser, quoctich, tinhTp, quanhuyen) VALUES (@fullname, @gender, @birth, @national, @city, @town)";
                    MySqlCommand cmd = new MySqlCommand(dataInsert, conn.connection);
                    cmd.Parameters.AddWithValue("@fullname", HovaTen.Text);
                    cmd.Parameters.AddWithValue("@gender", gioiTinh.Text);
                    cmd.Parameters.AddWithValue("@birth", birthInput.Text);
                    cmd.Parameters.AddWithValue("@national", quocTich.Text);
                    cmd.Parameters.AddWithValue("@city", tinhTP.Text);
                    cmd.Parameters.AddWithValue("@town", quanHuyen.Text);
                    cmd.ExecuteNonQuery();
                    conn.CloseConnection();
                    HovaTen.Text = "";
                    gioiTinh.Text = "";
                    birthInput.Text = "";
                    quocTich.Text = "";
                    tinhTP.Text = "";
                    quanHuyen.Text = "";
                    
                    MessageBox.Show("CẬP NHẬT THÀNH CÔNG", "Success");

                    Home home = new Home();
                    home.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
