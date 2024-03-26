using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using ManageExam.database;
using System.Collections.ObjectModel;


namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for DSND.xaml
    /// </summary>
    public partial class AddU : UserControl
    {
        public AddU()
        {
            InitializeComponent();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            string hotenText = ten.Text;
            string sdtText = sdt.Text;
            DateTime? ngaysinh = date.SelectedDate;
            string emailText = email.Text;
            string matkhauText = matkhau.Text;
            string thanhphoText = thanhpho.Text;

            if (string.IsNullOrEmpty(hotenText) || string.IsNullOrEmpty(sdtText) || ngaysinh == null || string.IsNullOrEmpty(emailText) || string.IsNullOrEmpty(matkhauText) || string.IsNullOrEmpty(thanhphoText))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra định dạng của số điện thoại
            Regex phoneRegex = new Regex(@"^\d{1,11}$");
            if (!phoneRegex.IsMatch(sdtText))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập đúng định dạng!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra định dạng của email
            Regex emailRegex = new Regex(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$");
            if (!emailRegex.IsMatch(emailText))
            {
                MessageBox.Show("Email không hợp lệ! Vui lòng nhập đúng định dạng!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        // Kiểm tra xem số điện thoại đã tồn tại trong cơ sở dữ liệu chưa
                        string checkExistQuery = "SELECT COUNT(*) FROM user WHERE sdtUSER = @sdt";
                        MySqlCommand checkExistCmd = new MySqlCommand(checkExistQuery, conn.connection);
                        checkExistCmd.Parameters.AddWithValue("@sdt", sdtText);

                        int existCount = Convert.ToInt32(checkExistCmd.ExecuteScalar());
                        if (existCount == 0)
                        {
                            // Số điện thoại không tồn tại, thêm mới thông tin người dùng
                            string insertQuery = "INSERT INTO user (nameUSER, sdtUSER, ngaysinhUSER, emailUSER, matkhauUSER, tinhtp) VALUES (@name, @sdt, @ngaysinh, @email, @matkhau, @tinhtp)";
                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn.connection);
                            insertCmd.Parameters.AddWithValue("@name", hotenText);
                            insertCmd.Parameters.AddWithValue("@sdt", sdtText);
                            insertCmd.Parameters.AddWithValue("@ngaysinh", ngaysinh);
                            insertCmd.Parameters.AddWithValue("@email", emailText);
                            insertCmd.Parameters.AddWithValue("@matkhau", matkhauText);
                            insertCmd.Parameters.AddWithValue("@tinhtp", thanhphoText);

                            int rowsAffected = insertCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thêm thông tin người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                ten.Text = "";
                                sdt.Text = "";
                                date.SelectedDate = null;
                                email.Text = "";
                                matkhau.Text = "";
                                thanhpho.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Thêm thông tin người dùng thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Tài khoản của số điện thoại này đã tồn tại trên hệ thống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm thông tin người dùng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox5_TextChanged()
        {

        }
    }
}
