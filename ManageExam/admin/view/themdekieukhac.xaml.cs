using ManageExam.admin.view.contro;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Microsoft.Win32;
using System.IO;


namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for themdekieukhac.xaml
    /// </summary>
    public partial class themdekieukhac : Window
    {
        public string tende;
        public string mon;
        public string thoigian;

        // id đề thi này
        public int iddethi;

        public themdekieukhac(string td, string mo , string tg , int iddt)
        {
            InitializeComponent();
            tende = td;
            mon = mo;
            thoigian = tg;
            iddethi = iddt;
            Load();
            Lad(iddethi);
        }
        public static void Lad(int ID)
        {
            
        }
        public void Load()
        {
            tendethichinh.Content = tende;
            monhoc.Content = mon;
            thoigianthi.Content = thoigian;

            // load 
            string[] dapAnList = { "A", "B", "C", "D" };

            // Gán mảng vào ItemsSource của ComboBox
            dapandung.ItemsSource = dapAnList;

        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            // thêm lựa chọn chắc chắn thoát


            Home home = new Home();
            home.Show();
            this.Close();
        }

        public string dapand;
        //public static int idcau = nhapcauhoi.getIdCauHoi();

        private void setdapan_Click(object sender, RoutedEventArgs e)
        {
            string[] dapAnList = { "A", "B", "C", "D" };

            // Gán mảng vào ItemsSource của ComboBox
            dapandung.ItemsSource = dapAnList;
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
                        LoadData(iddethi);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show($"Đã xảy ra lỗi khi cập nhật đáp án: {ex.Message}");
                }
            }

            int idcau = nhapcauhoi.getIdCauHoi();
            dapand = dapandung.Text;
            if (TX ==true && AN == false)
            {
                Console.WriteLine(idcau);
                try
                {
                    using (Connection con = new Connection())
                    {
                        con.OpenConnection();

                        // Kiểm tra xem idCauHoi đã tồn tại trong bảng dapan hay không
                        string checkQuery = "SELECT COUNT(*) FROM dapan WHERE idCAUHOI = @idCauHoi";
                        using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, con.connection))
                        {
                            checkCommand.Parameters.AddWithValue("@idCauHoi", idcau);
                            int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (existingCount == 0)
                            {
                                // Chuẩn bị câu lệnh SQL để chèn một bản ghi mới vào bảng dapan
                                string query = "INSERT INTO dapan (idCAUHOI, dapandungDAPAN) VALUES (@idCauHoi, @dapand)";

                                using (MySqlCommand command = new MySqlCommand(query, con.connection))
                                {
                                    // Thêm các tham số vào câu lệnh SQL
                                    command.Parameters.AddWithValue("@idCauHoi", idcau); // ID của câu hỏi
                                    command.Parameters.AddWithValue("@dapand", dapand); // Đáp án

                                    // Thực thi câu lệnh SQL
                                    command.ExecuteNonQuery();
                                }

                                // Thông báo thành công
                                MessageBox.Show("Success");
                            }
                            else
                            {
                                string updateQuery = "UPDATE dapan SET dapandungDAPAN = @dapand WHERE idCAUHOI = @idCauHoi";

                                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, con.connection))
                                {
                                    // Thêm các tham số vào câu lệnh SQL
                                    updateCommand.Parameters.AddWithValue("@idCauHoi", idcau); // ID của câu hỏi
                                    updateCommand.Parameters.AddWithValue("@dapand", dapand); // Đáp án

                                    // Thực thi câu lệnh SQL
                                    updateCommand.ExecuteNonQuery();
                                }

                                // Thông báo thành công
                                MessageBox.Show("Cập nhật đáp án đúng thành công");

                            }
                        }

                        // Đóng kết nối
                        con.CloseConnection();
                        LoadData(iddethi);
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show($"Đã xảy ra lỗi khi thêm đáp án vào cơ sở dữ liệu: {ex.Message}");
                    Console.WriteLine( "set   "+ ex.Message);
                }
            }





            else
            {
                try
                {
                    using (Connection con = new Connection())
                    {
                        con.OpenConnection();

                        // Kiểm tra xem idCauHoi đã tồn tại trong bảng dapan hay không
                        string checkQuery = "SELECT COUNT(*) FROM dapan WHERE idCAUHOI = @idCauHoi";
                        using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, con.connection))
                        {
                            checkCommand.Parameters.AddWithValue("@idCauHoi", idCauHoi);
                            int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (existingCount == 0)
                            {
                                // Chuẩn bị câu lệnh SQL để chèn một bản ghi mới vào bảng dapan
                                string query = "INSERT INTO dapan (idCAUHOI, dapandungDAPAN) VALUES (@idCauHoi, @dapand)";

                                using (MySqlCommand command = new MySqlCommand(query, con.connection))
                                {
                                    // Thêm các tham số vào câu lệnh SQL
                                    command.Parameters.AddWithValue("@idCauHoi", idCauHoi); // ID của câu hỏi
                                    command.Parameters.AddWithValue("@dapand", dapand); // Đáp án

                                    // Thực thi câu lệnh SQL
                                    command.ExecuteNonQuery();
                                }

                                // Thông báo thành công
                                MessageBox.Show("Success");
                            }
                            else
                            {
                                string updateQuery = "UPDATE dapan SET dapandungDAPAN = @dapand WHERE idCAUHOI = @idCauHoi";

                                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, con.connection))
                                {
                                    // Thêm các tham số vào câu lệnh SQL
                                    updateCommand.Parameters.AddWithValue("@idCauHoi", idcau); // ID của câu hỏi
                                    updateCommand.Parameters.AddWithValue("@dapand", dapand); // Đáp án

                                    // Thực thi câu lệnh SQL
                                    updateCommand.ExecuteNonQuery();
                                }

                                // Thông báo thành công
                                MessageBox.Show("Cập nhật đáp án đúng thành công");
                            }
                        }

                        // Đóng kết nối
                        con.CloseConnection();
                    }
                }
                catch (Exception ex)
                {
                    // Xử lý nếu có lỗi xảy ra
                    MessageBox.Show($"Đã xảy ra lỗi khi thêm đáp án vào cơ sở dữ liệu: {ex.Message}");
                }
            }
            LoadData(iddethi);

        }
        public int demcau = 1;
        public static int idCauHoi;
        public bool AN;
        public bool TX;
        private void them_Click(object sender, RoutedEventArgs e)
        {
            

            if (themanh.IsChecked == true)
            {
                
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";

                // Mở hộp thoại mở tệp và kiểm tra nếu người dùng đã chọn một tệp
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // Đọc nội dung của tệp ảnh
                        byte[] imageData = System.IO.File.ReadAllBytes(openFileDialog.FileName);

                        // Lấy tên tệp ảnh
                        string imageName = System.IO.Path.GetFileName(openFileDialog.FileName);

                        // Thực hiện thêm dữ liệu vào cơ sở dữ liệu
                        using (Connection con = new Connection())
                        {
                            con.OpenConnection();

                            // Chuẩn bị câu lệnh SQL để chèn một bản ghi mới vào bảng filecauhoi
                            string query = "INSERT INTO filecauhoi (nameFile, file) VALUES (@nameFile, @file)";

                            using (MySqlCommand command = new MySqlCommand(query, con.connection))
                            {
                                // Thêm các tham số vào câu lệnh SQL
                                command.Parameters.AddWithValue("@nameFile", imageName); // Tên tệp ảnh
                                command.Parameters.AddWithValue("@file", imageData); // Nội dung của tệp ảnh

                                // Thực thi câu lệnh SQL
                                command.ExecuteNonQuery();

                                int idfileCauHoi = (int)command.LastInsertedId;

                                // Chuẩn bị câu lệnh SQL để thêm bản ghi mới vào bảng cauhoi
                                string cauHoiQuery = "INSERT INTO cauhoi (tenCAUHOI,idFilecauhoi, IDDETHI) VALUES (@tenCAUHOI,@idFilecauhoi, @IDDethi)";
                                using (MySqlCommand cauHoiCommand = new MySqlCommand(cauHoiQuery, con.connection))
                                {
                                    string tenCH = $"Câu: {demcau}";
                                    // Thêm các tham số vào câu lệnh SQL
                                    cauHoiCommand.Parameters.AddWithValue("@tenCAUHOI", tenCH);
                                    cauHoiCommand.Parameters.AddWithValue("@idFilecauhoi", idfileCauHoi);
                                    cauHoiCommand.Parameters.AddWithValue("@IDDethi", iddethi);

                                    // Thực thi câu lệnh SQL để thêm bản ghi mới vào bảng cauhoi
                                    cauHoiCommand.ExecuteNonQuery();
                                    idCauHoi = (int)cauHoiCommand.LastInsertedId;

                                }



                            }

                            // Đóng kết nối
                            con.CloseConnection();
                            demcau++;
                            // Thông báo thành công
                            MessageBox.Show("Thêm ảnh vào cơ sở dữ liệu thành công!");
                            FILE.Visibility = Visibility.Visible;
                            TXT.Visibility = Visibility.Hidden;
                            AN = true;
                            TX = false;
                            LoadData(iddethi);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Xử lý nếu có lỗi xảy ra
                        MessageBox.Show($"Đã xảy ra lỗi khi thêm ảnh vào cơ sở dữ liệu: {ex.Message}");
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            else if (themtext.IsChecked == true)
            {
                
                // Thêm Text
                nhapcauhoi nhapcauhoi = new nhapcauhoi(iddethi, demcau);

                bool? result = nhapcauhoi.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    // Đã click vào nút trong dialog, đóng cửa sổ
                    demcau++;
                    FILE.Visibility = Visibility.Hidden;
                    TXT.Visibility = Visibility.Visible;
                    AN = false;
                    TX = true;
                    LoadData(iddethi);
                }
            }
        }
        public static int getIdCauHoi()
        {
            return idCauHoi;
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
                Console.WriteLine("looic: "+ex.Message );
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


        public int id;
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
                    Console.WriteLine("ID :: :: :: " + id);

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
        public static int GetId()
        {
            return idCauHoi;
        }



        private void save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Lưu Thành Công");
            Home home = new Home();
            home.Show();

            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData(iddethi);
        }
    }
}
