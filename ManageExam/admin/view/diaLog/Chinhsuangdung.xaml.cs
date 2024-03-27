using ManageExam.database;
using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace ManageExam.admin.view.diaLog
{
    /// <summary>
    /// Interaction logic for Chinhsuangdung.xaml
    /// </summary>
    public partial class Chinhsuangdung : Window
    {
        public int UserID { get; set; }
        public Chinhsuangdung(int UserIDs)
        {
            UserID = UserIDs;
            InitializeComponent();
            Load();
        }
        public void Load()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())

                    {
                        string query = "SELECT * FROM user WHERE IDUSER = @id";
                        MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                        cmd.Parameters.AddWithValue("@id", UserID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Gán thông tin của người dùng vào các trường tương ứng trên form
                                hoten.Text = reader.GetString("nameUSER");
                                gioitinh.Text = reader.GetString("gioitinhUSER");
                                email.Text = reader.GetString("emailUSER");
                                sdt.Text = reader.GetString("sdtUSER");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin người dùng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.Message);
            }
        }


        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            string hotenText = hoten.Text;
            string gioitinhText = gioitinh.Text;
            string emailText = email.Text;
            string sdtText = sdt.Text;
            string matkhauText = matkhau.Text;
            DateTime? ngaysinh = date.SelectedDate;

            if (string.IsNullOrEmpty(hotenText) || string.IsNullOrEmpty(gioitinhText) || string.IsNullOrEmpty(emailText) || string.IsNullOrEmpty(sdtText) || string.IsNullOrEmpty(matkhauText) || ngaysinh == null)
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
                        if (existCount > 0)
                        {
                            // Số điện thoại đã tồn tại, thực hiện cập nhật thông tin
                            string updateQuery = "UPDATE user SET nameUSER = @name, gioitinhUSER = @gioitinh, emailUSER = @email, matkhauUSER = @matkhau, ngaysinhUSER = @ngaysinh WHERE sdtUSER = @sdt";
                            MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn.connection);
                            updateCmd.Parameters.AddWithValue("@name", hotenText);
                            updateCmd.Parameters.AddWithValue("@gioitinh", gioitinhText);
                            updateCmd.Parameters.AddWithValue("@email", emailText);
                            updateCmd.Parameters.AddWithValue("@matkhau", matkhauText);
                            updateCmd.Parameters.AddWithValue("@ngaysinh", ngaysinh);
                            updateCmd.Parameters.AddWithValue("@sdt", sdtText);

                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cập nhật thông tin người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                hoten.Text = "";
                                sdt.Text = "";
                                date.SelectedDate = null;
                                email.Text = "";
                                matkhau.Text = "";
                                gioitinh.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Cập nhật thông tin người dùng thất bại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Số điện thoại không tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin người dùng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
