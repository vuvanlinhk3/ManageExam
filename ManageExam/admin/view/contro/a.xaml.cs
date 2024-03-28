using ManageExam.database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using ManageExam.admin.view.viewExam;
using System.Diagnostics;


namespace ManageExam.admin.view.contro
{
    /// <summary>
    /// Interaction logic for filecauhoi.xaml
    /// </summary>
    public partial class a : UserControl
    {
        public a()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {

        }
        private void xemfile_Click(object sender, RoutedEventArgs e)
        {
            // Lấy idCauHoi từ hàm GetIDCauHoi
            int? idCauHoi = SingerExam.GetIDCH();

            if (!idCauHoi.HasValue)
            {
                // getIdCauHoi() trả về giá trị null
                MessageBox.Show("Không tìm thấy ID câu hỏi.");
                return;
            }

            try
            {
                using (Connection con = new Connection())
                {
                    con.OpenConnection();

                    // Truy vấn để lấy idFilecauhoi từ bảng cauhoi
                    string queryCauHoi = "SELECT idFilecauhoi FROM cauhoi WHERE idCAUHOI = @idCauHoi";
                    using (MySqlCommand cmdCauHoi = new MySqlCommand(queryCauHoi, con.connection))
                    {
                        cmdCauHoi.Parameters.AddWithValue("@idCauHoi", idCauHoi);

                        object idFilecauhoi = cmdCauHoi.ExecuteScalar();
                        if (idFilecauhoi != null)
                        {
                            // Truy vấn để lấy tên tệp từ bảng filecauhoi
                            string queryFileName = "SELECT file FROM filecauhoi WHERE idFilecauhoi = @idFilecauhoi";
                            using (MySqlCommand cmdFileName = new MySqlCommand(queryFileName, con.connection))
                            {
                                cmdFileName.Parameters.AddWithValue("@idFilecauhoi", idFilecauhoi);

                                object fileData = cmdFileName.ExecuteScalar();
                                if (fileData != null)
                                {
                                    byte[] fileBytes = (byte[])fileData;

                                    // Tạo một tệp tạm thời để lưu trữ dữ liệu file
                                    string tempFilePath = System.IO.Path.GetTempFileName() + ".pdf"; // Đổi ".pdf" thành phần mở rộng tệp tương ứng

                                    // Ghi dữ liệu vào tệp tạm thời
                                    File.WriteAllBytes(tempFilePath, fileBytes);

                                    // Tạo một đối tượng ProcessStartInfo để cấu hình quá trình mở tệp
                                    ProcessStartInfo psi = new ProcessStartInfo(tempFilePath)
                                    {
                                        UseShellExecute = true // Sử dụng ShellExecute để hiển thị hộp thoại lựa chọn trình duyệt
                                    };

                                    // Mở tệp tạm thời trong trình duyệt web
                                    Process.Start(psi);
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy dữ liệu tệp.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy ID tệp.");
                        }
                    }

                    con.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }




        private void doifile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? idCauHoi = SingerExam.GetIDCH();

                if (!idCauHoi.HasValue)
                {
                    // Nếu không có id câu hỏi, không thể thực hiện truy vấn
                    return;
                }

                using (Connection con = new Connection())
                {
                    con.OpenConnection();

                    // Truy vấn để lấy idFilecauhoi từ bảng cauhoi
                    string queryCauHoi = "SELECT idFilecauhoi FROM cauhoi WHERE idCAUHOI = @idCauHoi";
                    using (MySqlCommand cmdCauHoi = new MySqlCommand(queryCauHoi, con.connection))
                    {
                        cmdCauHoi.Parameters.AddWithValue("@idCauHoi", idCauHoi);

                        int? idFilecauhoi = cmdCauHoi.ExecuteScalar() as int?;
                        if (idFilecauhoi.HasValue)
                        {
                            // Mở hộp thoại để chọn file mới
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif)|*.jpg;*.jpeg;*.png;*.gif";
                            if (openFileDialog.ShowDialog() == true)
                            {
                                // Lấy đường dẫn của file mới được chọn
                                string newFilePath = openFileDialog.FileName;
                                string newFileName =System.IO.Path.GetFileName(newFilePath);
                                // Đọc dữ liệu từ file mới
                                byte[] imageData = File.ReadAllBytes(newFilePath);

                                // Cập nhật dữ liệu ảnh trong cơ sở dữ liệu
                                string updateQuery = "UPDATE filecauhoi SET nameFile = @newFileName, file = @file WHERE idFilecauhoi = @idFilecauhoi";
                                using (MySqlCommand cmdUpdate = new MySqlCommand(updateQuery, con.connection))
                                {
                                    cmdUpdate.Parameters.AddWithValue("@newFileName", newFileName);
                                    cmdUpdate.Parameters.AddWithValue("@file", imageData);
                                    cmdUpdate.Parameters.AddWithValue("@idFilecauhoi", idFilecauhoi);
                                    cmdUpdate.ExecuteNonQuery();

                                    MessageBox.Show("File ảnh và tên file đã được cập nhật thành công.");
                                }
                            }
                        }
                        else
                        {
                            // Xử lý khi không tìm thấy idFilecauhoi
                            MessageBox.Show("Không tìm thấy idFilecauhoi liên kết với câu hỏi này.");
                        }
                    }

                    con.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi cập nhật file ảnh: {ex.Message}");
            }
        }


        private void load(object sender, RoutedEventArgs e)
        {
            int? idCauHoi = SingerExam.GetIDCH();
            Console.WriteLine("id2 " + idCauHoi);

            if (!idCauHoi.HasValue)
            {
                // getIdCauHoi() trả về giá trị null
            }
            else
            {
                // getIdCauHoi() trả về một giá trị khác null
                try
                {
                    using (Connection con = new Connection())
                    {
                        con.OpenConnection();

                        // Truy vấn để lấy idFilecauhoi từ bảng cauhoi
                        string queryCauHoi = "SELECT idFilecauhoi FROM cauhoi WHERE idCAUHOI = @idCauHoi";
                        using (MySqlCommand cmdCauHoi = new MySqlCommand(queryCauHoi, con.connection))
                        {
                            cmdCauHoi.Parameters.AddWithValue("@idCauHoi", idCauHoi);

                            int? idFilecauhoi = cmdCauHoi.ExecuteScalar() as int?;
                            if (idFilecauhoi.HasValue)
                            {
                                // Truy vấn để lấy dữ liệu ảnh từ bảng filecauhoi
                                string queryFileCauhoi = "SELECT file FROM filecauhoi WHERE idFilecauhoi = @idFilecauhoi";
                                using (MySqlCommand cmdFileCauhoi = new MySqlCommand(queryFileCauhoi, con.connection))
                                {
                                    cmdFileCauhoi.Parameters.AddWithValue("@idFilecauhoi", idFilecauhoi);

                                    byte[] imageData = cmdFileCauhoi.ExecuteScalar() as byte[];
                                    if (imageData != null && imageData.Length > 0)
                                    {
                                        // Gán dữ liệu ảnh vào thẻ img
                                        BitmapImage bitmap = new BitmapImage();
                                        bitmap.BeginInit();
                                        bitmap.StreamSource = new MemoryStream(imageData);
                                        bitmap.EndInit();

                                        anhcauhoi.Source = bitmap; // Thay yourImageControl bằng tên thật của thẻ img trong XAML của bạn
                                    }
                                    else
                                    {
                                        // Xử lý khi không có dữ liệu ảnh
                                    }
                                }
                            }
                            else
                            {
                                // Xử lý khi không tìm thấy idFilecauhoi
                            }
                        }

                        con.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi khi thêm đáp án vào cơ sở dữ liệu: {ex.Message}");
                    Console.WriteLine("file   " + ex.Message);
                }
            }
        }
    }
}
