using ManageExam.database;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using static ManageExam.admin.view.themdekieukhac;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace ManageExam.admin.view.viewExam
{
    public partial class SingerExam : System.Windows.Window // Chỉ rõ namespace cho Window
    {
        public int MADE;

        public SingerExam(int madethi)
        {
            InitializeComponent();
            MADE = madethi;
            LoadData(MADE);
            Load();
        }
        public void Load()
        {
            try
            {
                using (Connection con = new Connection())
                {
                    con.OpenConnection();
                    string query = "SELECT nameDETHI, timeDETHI, monhocDETHI FROM dethi";

                    MySqlCommand command = new MySqlCommand(query, con.connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            txtTenDeThi.Text = reader["nameDETHI"].ToString();
                            txtThoiGianThi.Text = reader["timeDETHI"].ToString();
                            txtMonHoc.Text = reader["monhocDETHI"].ToString();
                        }
                    }
                    con.CloseConnection();
                }

            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi khi tải dữ liệu từ cơ sở dữ liệu: {ex.Message}");
                Console.WriteLine("looic: " + ex.Message);
            }
        }
    public void LoadData(int idDethi)
        {
            string[] dapAnList = { "A", "B", "C", "D" };

            // Gán mảng vào ItemsSource của ComboBox
            dapandung.ItemsSource = dapAnList;
            try
            {
                using (Connection con = new Connection())
                {
                    con.OpenConnection();

                    // Chuẩn bị câu lệnh SQL để lấy dữ liệu từ bảng câu hỏi cho một đề thi cụ thể
                    string query = "SELECT ch.idCAUHOI, ch.tenCAUHOI, ch.idFilecauhoi, ch.idTxtcauhoi, f.nameFile, t.noiDung, d.dapandungDAPAN " +
                "FROM cauhoi ch " +
                "LEFT JOIN filecauhoi f ON ch.idFilecauhoi = f.idFilecauhoi " +
                "LEFT JOIN txtcauhoi t ON ch.idTxtcauhoi = t.idTxtcauhoi " +
                "LEFT JOIN dapan d ON ch.idCAUHOI = d.idCAUHOI " +
                "WHERE ch.IDDETHI = @idDethi";



                    using (MySqlCommand command = new MySqlCommand(query, con.connection))
                    {
                        command.Parameters.AddWithValue("@idDethi", idDethi);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            // Xóa dữ liệu cũ trong ListView trước khi load dữ liệu mới
                            bangcauhoi.Items.Clear();

                            // Biến đếm số thứ tự
                            int count = 1;

                            // Đọc dữ liệu từ cơ sở dữ liệu và thêm vào ListView
                            while (reader.Read())
                            {
                                CauHoiItem item = new CauHoiItem();

                                item.Number = count++;
                                item.TenCauHoi = reader.GetString(reader.GetOrdinal("tenCAUHOI"));

                                // Nếu có idFilecauhoi, hiển thị tên file trong cột nội dung
                                if (!reader.IsDBNull(reader.GetOrdinal("idFilecauhoi")))
                                {
                                    item.NoiDung = reader.GetString(reader.GetOrdinal("nameFile"));
                                }
                                // Kiểm tra xem cột idTxtcauhoi có giá trị null hay không
                                else if (!reader.IsDBNull(reader.GetOrdinal("idTxtcauhoi")))
                                {
                                    item.NoiDung = reader.GetString(reader.GetOrdinal("noiDung"));
                                }

                                // Kiểm tra xem cột dapandungDAPAN có giá trị null hay không
                                if (!reader.IsDBNull(reader.GetOrdinal("dapandungDAPAN")))
                                {
                                    item.DapAnDung = reader.GetString(reader.GetOrdinal("dapandungDAPAN"));
                                }
                                item.ID = reader.GetInt32(reader.GetOrdinal("idCAUHOI"));
                                // Thêm ListViewItem vào ListView
                                bangcauhoi.Items.Add(item);
                            }
                        }
                    }

                    // Đóng kết nối
                    con.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi khi tải dữ liệu từ cơ sở dữ liệu: {ex.Message}");
                Console.WriteLine("looic: " + ex.Message);
            }
        }

        public class CauHoiItem
        {
            public int Number { get; set; }
            public string TenCauHoi { get; set; }
            public string NoiDung { get; set; }
            public string DapAnDung { get; set; }
            public int ID { get; set; }
        }





        private void xemDe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.connection.Open();

                    string selectQuery = "SELECT filedethi FROM dethi WHERE IDDETHI = @id";
                    MySqlCommand command = new MySqlCommand(selectQuery, conn.connection);
                    command.Parameters.AddWithValue("@id", MADE);
                    object fileData = command.ExecuteScalar();

                    if (fileData != null)
                    {
                        // Lưu file dự thi vào một tệp tạm thời
                        string tempFilePath = System.IO.Path.GetTempFileName();
                        System.IO.File.WriteAllBytes(tempFilePath, (byte[])fileData);

                        // Mở tệp đề thi bằng chương trình mặc định
                        System.Diagnostics.Process.Start(tempFilePath);
                    }
                    else
                    {
                        MessageBox.Show("Không có tệp đề thi.");
                    }

                    conn.connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void setdapan_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem đã chọn một mục trong ListView hay chưa
            if (bangcauhoi.SelectedItem != null)
            {
                // Ép kiểu SelectedItem thành kiểu dữ liệu của mục trong ListView (trong trường hợp này là CauHoiItem)
                CauHoiItem selectedCauHoi = (CauHoiItem)bangcauhoi.SelectedItem;

                try
                {
                    using (Connection con = new Connection())
                    {
                        con.OpenConnection();

                        // Chuẩn bị câu lệnh SQL để cập nhật đáp án trong bảng dapan cho một câu hỏi cụ thể
                        string updateQuery = "UPDATE dapan SET dapandungDAPAN = @newDapAn WHERE idCAUHOI = @idCauHoi";

                        using (MySqlCommand command = new MySqlCommand(updateQuery, con.connection))
                        {
                            // Lấy ID của câu hỏi đã chọn
                            int idCauHoi = selectedCauHoi.ID;

                            // Lấy giá trị mới của đáp án từ ComboBox
                            string newDapAn = dapandung.SelectedValue as string;

                            // Gán giá trị cho các tham số trong câu lệnh SQL
                            command.Parameters.AddWithValue("@newDapAn", newDapAn);
                            command.Parameters.AddWithValue("@idCauHoi", idCauHoi);

                            // Thực thi truy vấn cập nhật
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Nếu cập nhật thành công, hiển thị thông báo
                                MessageBox.Show("Đã cập nhật đáp án thành công!");
                            }
                            else
                            {
                                // Xử lý khi không có bản ghi nào được cập nhật
                                MessageBox.Show("Không thể cập nhật đáp án!");
                            }
                        }

                        // Đóng kết nối
                        con.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show($"Đã xảy ra lỗi khi cập nhật đáp án: {ex.Message}");
                }
            }
            else
            {
                // Hiển thị thông báo khi chưa chọn một mục trong ListView
                MessageBox.Show("Vui lòng chọn một câu hỏi để cập nhật đáp án!");
            }
            LoadData(MADE);
        }



        public static int id;
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] dapAnList = { "A", "B", "C", "D" };

            // Gán mảng vào ItemsSource của ComboBox
            dapandung.ItemsSource = dapAnList;
            // Kiểm tra xem có mục nào được chọn không
            if (bangcauhoi.SelectedItem != null)
            {
                try
                {
                    // Ép kiểu SelectedItem thành kiểu dữ liệu của mục trong ListView (trong trường hợp này là CauHoiItem)
                    CauHoiItem selectedCauHoi = (CauHoiItem)bangcauhoi.SelectedItem;

                    // Sử dụng dữ liệu của mục đã chọn ở đây
                    id = selectedCauHoi.ID;
                    Console.WriteLine( "ID :: :: :: "+ id);

                    using (Connection con = new Connection())
                    {
                        con.OpenConnection();

                        // Chuẩn bị câu lệnh SQL để lấy dữ liệu từ bảng câu hỏi cho một câu hỏi cụ thể
                        string query = "SELECT ch.idFilecauhoi, ch.idTxtcauhoi, d.dapandungDAPAN " +
                                       "FROM cauhoi ch " +
                                       "LEFT JOIN dapan d ON ch.idCAUHOI = d.idCAUHOI " +
                                       "WHERE ch.idCAUHOI = @idCauHoi";

                        using (MySqlCommand command = new MySqlCommand(query, con.connection))
                        {
                            command.Parameters.AddWithValue("@idCauHoi", id);

                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Kiểm tra xem có idFilecauhoi tồn tại hay không
                                    if (!reader.IsDBNull(reader.GetOrdinal("idFilecauhoi")))
                                    {
                                        // Hiển thị log nếu idFilecauhoi tồn tại
                                        FILE.Visibility = Visibility.Visible;
                                        TXT.Visibility = Visibility.Hidden;
                                    }
                                    else if (!reader.IsDBNull(reader.GetOrdinal("idTxtcauhoi")))
                                    {
                                        // Hiển thị log nếu idTxtcauhoi tồn tại
                                        FILE.Visibility = Visibility.Hidden;
                                        TXT.Visibility = Visibility.Visible;
                                    }

                                    // Nếu có dapan đúng, lưu vào biến dapandungDAPAN
                                    if (!reader.IsDBNull(reader.GetOrdinal("dapandungDAPAN")))
                                    {
                                        string dapa = reader.GetString(reader.GetOrdinal("dapandungDAPAN"));
                                        // Giả sử danh sách dữ liệu là listData
                                        
                                        dapandung.DisplayMemberPath = dapa; // PropertyName là tên của thuộc tính trong đối tượng dữ liệu bạn muốn hiển thị
                                        dapandung.SelectedValuePath = dapa;
                                        
                                    }
                                }
                                else
                                {
                                    // Hiển thị log nếu không tìm thấy câu hỏi với idCauHoi tương ứng
                                    Console.WriteLine("Không tìm thấy câu hỏi có id tương ứng.");
                                }
                            }
                        }

                        // Đóng kết nối
                        con.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show($"Đã xảy ra lỗi khi tải dữ liệu từ cơ sở dữ liệu: {ex.Message}");
                }
            }
           
        }

        public static int GetIDCH()
        {
            return id;
        }
        
        private void chinhdethi(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection con = new Connection())
                {
                    con.OpenConnection();

                    // Chuẩn bị câu lệnh SQL để cập nhật tên đề thi trong cơ sở dữ liệu
                    string updateQuery = "UPDATE dethi SET nameDETHI = @newTenDeThi WHERE IDDETHI = @idDethi";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, con.connection))
                    {
                        // Gán giá trị cho các tham số trong câu lệnh SQL
                        command.Parameters.AddWithValue("@newTenDeThi", txtTenDeThi.Text);
                        command.Parameters.AddWithValue("@idDethi", MADE);

                        // Thực thi truy vấn cập nhật
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Nếu cập nhật thành công, hiển thị thông báo
                            MessageBox.Show("Đã cập nhật tên đề thi thành công!");
                        }
                        else
                        {
                            // Xử lý khi không có bản ghi nào được cập nhật
                            MessageBox.Show("Không thể cập nhật tên đề thi!");
                        }
                    }

                    // Đóng kết nối
                    con.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi khi cập nhật tên đề thi: {ex.Message}");
            }
        }

        private void chinhtime(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection con = new Connection())
                {
                    con.OpenConnection();

                    // Chuẩn bị câu lệnh SQL để cập nhật tên đề thi trong cơ sở dữ liệu
                    string updateQuery = "UPDATE dethi SET timeDETHI = @newTenDeThi WHERE IDDETHI = @idDethi";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, con.connection))
                    {
                        // Gán giá trị cho các tham số trong câu lệnh SQL
                        command.Parameters.AddWithValue("@newTenDeThi", txtThoiGianThi.Text);
                        command.Parameters.AddWithValue("@idDethi", MADE);

                        // Thực thi truy vấn cập nhật
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Nếu cập nhật thành công, hiển thị thông báo
                            MessageBox.Show("Đã cập nhật tên đề thi thành công!");
                        }
                        else
                        {
                            // Xử lý khi không có bản ghi nào được cập nhật
                            MessageBox.Show("Không thể cập nhật tên đề thi!");
                        }
                    }

                    // Đóng kết nối
                    con.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi khi cập nhật tên đề thi: {ex.Message}");
            }
        }

        private void chinhmon(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection con = new Connection())
                {
                    con.OpenConnection();

                    // Chuẩn bị câu lệnh SQL để cập nhật tên đề thi trong cơ sở dữ liệu
                    string updateQuery = "UPDATE dethi SET monhocDETHI = @newTenDeThi WHERE IDDETHI = @idDethi";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, con.connection))
                    {
                        // Gán giá trị cho các tham số trong câu lệnh SQL
                        command.Parameters.AddWithValue("@newTenDeThi", txtMonHoc);
                        command.Parameters.AddWithValue("@idDethi", MADE);

                        // Thực thi truy vấn cập nhật
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Nếu cập nhật thành công, hiển thị thông báo
                            MessageBox.Show("Đã cập nhật tên đề thi thành công!");
                        }
                        else
                        {
                            // Xử lý khi không có bản ghi nào được cập nhật
                            MessageBox.Show("Không thể cập nhật tên đề thi!");
                        }
                    }

                    // Đóng kết nối
                    con.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi khi cập nhật tên đề thi: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData(MADE);
        }
    }
}
