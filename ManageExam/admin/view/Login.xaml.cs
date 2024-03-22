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
        {   string TK= tk.Text;
            string PASS = pass.Password;
            if (String.IsNullOrEmpty(TK) || String.IsNullOrEmpty(PASS))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng nhập!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Thực hiện truy vấn cơ sở dữ liệu để kiểm tra thông tin đăng nhập
            string query = "SELECT COUNT(*) FROM user WHERE id = @id AND mkUSER = @mkUSER";
            string connectionString = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", TK);
                command.Parameters.AddWithValue("@mkUSER", PASS);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    if (count > 0)
                    {                  
                        MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                    }
                    else
                    {                   
                        MessageBox.Show("Tài khoản không tồn tại!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }
}
