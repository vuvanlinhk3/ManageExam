using ManageExam.admin.view.diaLog;
using ManageExam.database;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ManageExam.admin.view.contro
{
    /// <summary>
    /// Interaction logic for txtcauhoi.xaml
    /// </summary>
    public partial class txtcauhoi : UserControl
    {
        public txtcauhoi()
        {
            InitializeComponent();
            Load();
        }
        public void Load()
        {
            
            
        }
        private void suacauhoi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? idCauHoi = nhapcauhoi.getIdCauHoi();
                Console.WriteLine("id " + idCauHoi);

                // Kiểm tra xem idCauHoi có giá trị không
                if (idCauHoi.HasValue)
                {
                    // Hiển thị hộp thoại nhập nội dung mới
                    if (text.Text !=null)
                    {string newContent = text.Text;



                        using (Connection con = new Connection())
                        {
                            con.OpenConnection();

                            // Truy vấn để cập nhật nội dung mới vào bảng txtcauhoi dựa trên idCauHoi
                            string updateQuery = "UPDATE txtcauhoi SET noidung = @newContent WHERE idTxtcauhoi = (SELECT idTxtcauhoi FROM cauhoi WHERE idCAUHOI = @idCauHoi)";

                            using (MySqlCommand command = new MySqlCommand(updateQuery, con.connection))
                            {
                                command.Parameters.AddWithValue("@newContent", newContent);
                                command.Parameters.AddWithValue("@idCauHoi", idCauHoi);

                                // Thực thi truy vấn cập nhật
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Nếu cập nhật thành công, hiển thị thông báo và cập nhật lại nội dung trên giao diện
                                    MessageBox.Show("Nội dung câu hỏi đã được cập nhật thành công!");
                                    text.Text = newContent;
                                }
                                else
                                {
                                    // Xử lý khi không có bản ghi nào được cập nhật
                                    MessageBox.Show("Không thể cập nhật nội dung câu hỏi!");
                                }
                            }

                            // Đóng kết nối
                            con.CloseConnection();
                        }
                    }
                }
                else
                {
                    // Xử lý khi không tìm thấy ID câu hỏi
                    MessageBox.Show("Không tìm thấy ID câu hỏi!");
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi khi sửa nội dung câu hỏi: {ex.Message}");
            }
        }


        private void load(object sender, RoutedEventArgs e)
        {
            int? idCauHoi = nhapcauhoi.getIdCauHoi();
            Console.WriteLine("id " + idCauHoi);
            if (idCauHoi.HasValue)
            {

                try
                {
                    using (Connection con = new Connection())
                    {
                        con.OpenConnection();

                        // Truy vấn để lấy idTxtcauhoi từ bảng cauhoi dựa trên idCauHoi
                        string queryIdTxtCauHoi = "SELECT idTxtcauhoi FROM cauhoi WHERE idCAUHOI = @idCauHoi";

                        using (MySqlCommand commandIdTxtCauHoi = new MySqlCommand(queryIdTxtCauHoi, con.connection))
                        {
                            commandIdTxtCauHoi.Parameters.AddWithValue("@idCauHoi", idCauHoi);
                            int idTxtCauHoi = Convert.ToInt32(commandIdTxtCauHoi.ExecuteScalar());

                            // Truy vấn để lấy nội dung từ bảng txtcauhoi dựa trên idTxtcauhoi
                            string queryNoiDungCauHoi = "SELECT noidung FROM txtcauhoi WHERE idTxtcauhoi = @idTxtCauHoi";

                            using (MySqlCommand commandNoiDungCauHoi = new MySqlCommand(queryNoiDungCauHoi, con.connection))
                            {
                                commandNoiDungCauHoi.Parameters.AddWithValue("@idTxtCauHoi", idTxtCauHoi);
                                text.Text = Convert.ToString(commandNoiDungCauHoi.ExecuteScalar());
                            }
                        }

                        // Đóng kết nối
                        con.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show($"Đã xảy ra lỗi khi lấy nội dung câu hỏi: {ex.Message}");
                }
            }
        }
    }
}
