using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient;
using ManageExam.database;
using System.Collections.ObjectModel;


namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            string TK = tk.Text;
            string PASS = pass.Password;

            if (string.IsNullOrEmpty(TK) || string.IsNullOrEmpty(PASS))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng nhập!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (Connection conn = new Connection())
            {
                try
                {
                    if (conn.OpenConnection())
                    {
                        string query = "SELECT COUNT(*) FROM tkadmin WHERE taikhoanAD = @id AND matkhauAD = @mk";
                        MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                        cmd.Parameters.AddWithValue("@id", TK);
                        cmd.Parameters.AddWithValue("@mk", PASS);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int count = reader.GetInt32(0);
                                if (count > 0)
                                {
                                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Home home = new Home();
                                    home.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Tài khoản không tồn tại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thực hiện đăng nhập: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }






        private void quenmk_Click(object sender, RoutedEventArgs e)
        {
            changePassWord changePW= new changePassWord();
            changePW.Show();
            this.Close();
        }

        private void dki_Click(object sender, RoutedEventArgs e)
        {
            Signup signupForm = new Signup();

            // Mở form Signup.xaml
            signupForm.Show();

            // Đóng form hiện tại (MainPage.xaml)
            this.Close();
        }
        private void tk_TextChanged(object sender, RoutedEventArgs e)
        {

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
