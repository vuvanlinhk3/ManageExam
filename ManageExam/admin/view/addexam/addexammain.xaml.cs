using System;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using ManageExam.database;
using MySql.Data.MySqlClient;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using System.Linq;
using System.Data;
using DataSqlClient;

namespace ManageExam.admin.view.addexam
{
    public partial class addexammain : Window
    {
        private ObservableCollection<FileItem> filesList = new ObservableCollection<FileItem>();
        private ObservableCollection<DeThiItem> listdapan = new ObservableCollection<DeThiItem>();
        public string namedethi;
        public addexammain(string nameExam)
        {
            InitializeComponent();
            file.ItemsSource = filesList;
            bangdapans.ItemsSource = listdapan;
            namedethi = nameExam;




            Load();
        }
        private void Load()
        {
            // Gán tên đề thi chính từ biến namedethi cho tendethichinh
            tendethichinh.Content = namedethi;

            // Tạo danh sách các môn học
            List<string> subjects = new List<string> { "Tiếng Anh" };

            // Xóa danh sách lựa chọn hiện tại của combobox monhoc
            monhoc.Items.Clear();

            // Thêm các môn học vào danh sách lựa chọn của combobox monhoc
            foreach (string subject in subjects)
            {
                monhoc.Items.Add(subject);
            }

            // Chọn mặc định môn học là "Tiếng Anh"
            monhoc.SelectedIndex = monhoc.Items.IndexOf("Tiếng Anh");

        }
        private void AddExamMain_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Sự kiện click nút để thêm hình ảnh




        public int sum;
        private void thietlapdapan_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem giá trị nhập vào có phải là số hay không
            if (!int.TryParse(soluongcauhoi.Text, out int soLuongCau))
            {
                // Nếu không phải là số, hiển thị MessageBox thông báo lỗi
                MessageBox.Show("Vui lòng nhập số nguyên dương.");
                return;
            }

            // Xóa tất cả các mục hiện có trong DataGrid
            //bangdapans.Items.Clear();

            // Tạo và thêm các mục mới vào danh sách
            for (int i = 1; i <= soLuongCau; i++)
            {
                string cau = i.ToString(); // Tạo câu với số thứ tự tăng dần
                int soLuongDapAn = 4; // Giả sử số lượng đáp án mặc định là 4
                string dapAnDung = ""; // Không có đáp án đúng ban đầu

                // Tạo một mục mới và thêm vào danh sách dữ liệu
                DeThiItem newItem = new DeThiItem
                {
                    Cau = cau,
                    SoLuongDapAn = soLuongDapAn,
                    DapAnDung = dapAnDung
                };

                // Thêm mục mới vào DataGrid
                listdapan.Add(newItem);
            }
        }

        public class DeThiItem
        {
            public string Cau { get; set; }
            public int SoLuongDapAn { get; set; }
            public string DapAnDung { get; set; }
        }
        public class FileItem
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }

        private void themFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // Chỉ cho phép chọn một tệp duy nhất
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName; // Lấy tên của tệp duy nhất được chọn
                FileInfo fileInfo = new FileInfo(filename);

                // Kiểm tra xem danh sách đã chứa tệp nào hay không
                FileItem existingFile = filesList.FirstOrDefault();
                if (existingFile != null)
                {
                    // Nếu danh sách đã chứa tệp, thực hiện thay đổi tệp đã có
                    existingFile.FileName = fileInfo.Name;
                    existingFile.FilePath = fileInfo.FullName;
                }
                else
                {
                    // Nếu danh sách không chứa tệp nào, thêm tệp mới
                    FileItem fileItem = new FileItem()
                    {
                        FileName = fileInfo.Name,
                        FilePath = fileInfo.FullName
                    };
                    filesList.Add(fileItem);
                }
            }
        }


        private void Xem_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FileItem fileItem = button.DataContext as FileItem;
            if (fileItem != null)
            {
                // Mở tệp tin để xem
                Process.Start(fileItem.FilePath);
            }
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy hàng chứa nút được nhấn
            var button = sender as Button;
            if (button == null)
                return;

            var item = button.DataContext as FileItem;
            if (item == null)
                return;

            // Mở hộp thoại OpenFileDialog để chọn tệp mới
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false; // Chỉ cho phép chọn một tệp duy nhất
            if (openFileDialog.ShowDialog() == true)
            {
                string newFilename = openFileDialog.FileName;
                FileInfo newFileInfo = new FileInfo(newFilename);

                // Cập nhật thông tin của tệp trong danh sách
                item.FileName = newFileInfo.Name;
                item.FilePath = newFileInfo.FullName;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy hàng chứa nút được nhấn
            var button = sender as Button;
            if (button == null)
                return;

            var item = button.DataContext as FileItem;
            if (item == null)
                return;

            // Xác nhận xóa từ người dùng (tùy chọn)
            MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn xóa tệp này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Xóa mục khỏi danh sách
                filesList.Remove(item);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem giá trị nhập vào có phải là số hay không
            if (!int.TryParse(soluongcauhoi.Text, out int soLuongCau))
            {
                // Nếu không phải là số, hiển thị MessageBox thông báo lỗi
                MessageBox.Show("Vui lòng nhập số nguyên dương.");
                return;
            }

            MySqlCommand insertDeThiCommand = null; // Khai báo biến insertDeThiCommand ở ngoài vòng lặp

            using (Connection conn = new Connection())
            {
                if (conn.OpenConnection())
                {
                    foreach (var item in filesList)
                    {
                        byte[] fileData;
                        using (FileStream fs = new FileStream(item.FilePath, FileMode.Open, FileAccess.Read))
                        {
                            fileData = new byte[fs.Length];
                            fs.Read(fileData, 0, (int)fs.Length);
                        }
                        string times = $"{hourTextBox.Text} : {minuteTextBox.Text}";
                        // Lưu dữ liệu vào bảng dethi
                        string insertDeThiQuery = "INSERT INTO dethi (nameDETHI, monhocDETHI, timeDETHI, ngaytaoDETHI, nguoitaoDETHI, filedethi) VALUES (@Name, @MonHoc, @Time, @NgayTao, @NguoiTao, @FileData)";
                        insertDeThiCommand = new MySqlCommand(insertDeThiQuery, conn.connection);
                        insertDeThiCommand.Parameters.AddWithValue("@Name", tendethichinh.Content);
                        insertDeThiCommand.Parameters.AddWithValue("@MonHoc", monhoc.Text);
                        insertDeThiCommand.Parameters.AddWithValue("@Time", times);
                        insertDeThiCommand.Parameters.AddWithValue("@NgayTao", DateTime.Now);
                        insertDeThiCommand.Parameters.AddWithValue("@NguoiTao", 1); // Thay thế 1 bằng ID người tạo
                        insertDeThiCommand.Parameters.AddWithValue("@FileData", fileData);

                        insertDeThiCommand.ExecuteNonQuery();
                    }

                    // Lấy ID của đề thi vừa thêm vào
                    int idDeThi;
                    using (MySqlCommand getIdDeThiCommand = new MySqlCommand("SELECT LAST_INSERT_ID()", conn.connection))
                    {
                        idDeThi = Convert.ToInt32(getIdDeThiCommand.ExecuteScalar());
                    }

                    // Lưu dữ liệu vào bảng cauhoi
                    string insertCauHoiQuery = "INSERT INTO cauhoi (tenCAUHOI, loaiCAUHOI, IDDETHI) VALUES (@TenCauHoi, @LoaiCauHoi, @IdDeThi)";
                    MySqlCommand insertCauHoiCommand = new MySqlCommand(insertCauHoiQuery, conn.connection);
                    // Thay thế 1 bằng loại câu hỏi tương ứng

                    for (int i = 1; i <= soLuongCau; i++)
                    {
                        string cau = "Câu " + i.ToString(); // Tạo câu với số thứ tự tăng dần

                        insertCauHoiCommand.Parameters.Clear();
                        insertCauHoiCommand.Parameters.AddWithValue("@TenCauHoi", cau);
                        insertCauHoiCommand.Parameters.AddWithValue("@IdDeThi", idDeThi);
                        insertCauHoiCommand.Parameters.AddWithValue("@LoaiCauHoi", "Bình Thường");
                        insertCauHoiCommand.ExecuteNonQuery();

                        // Lấy ID của câu hỏi vừa thêm vào
                        int idCauHoi;
                        using (MySqlCommand getIdCauHoiCommand = new MySqlCommand("SELECT LAST_INSERT_ID()", conn.connection))
                        {
                            idCauHoi = Convert.ToInt32(getIdCauHoiCommand.ExecuteScalar());
                        }
                        foreach (var item in listdapan)
                        {
                            string dapAnDung = ""; // Không có đáp án đúng ban đầu
                            string insertDapAnQuery = "INSERT INTO dapan (idCAUHOI, dapandungDAPAN, IDDETHI) VALUES (@IdCauHoi, @DapAnDung, @IdDeThi)";
                            MySqlCommand insertDapAnCommand = new MySqlCommand(insertDapAnQuery, conn.connection);

                            insertDapAnCommand.Parameters.AddWithValue("@IdCauHoi", idCauHoi);
                            insertDapAnCommand.Parameters.AddWithValue("@DapAnDung", item.DapAnDung

                                );
                            insertDapAnCommand.Parameters.AddWithValue("@IdDeThi", idDeThi);
                            insertDapAnCommand.ExecuteNonQuery();



                        }
                        // Lưu đáp án đúng vào bảng dapan


                    }

                    DateTime timestamp = DateTime.Now;
                    string action = $"Đã thêm đề thi {tendethichinh.Content} vào ngày : {timestamp}";

                    string insertActionQuery = "INSERT INTO tintuc (hanhdong, time) VALUES (@Action, @Timestamp)";
                    MySqlCommand insertActionCommand = new MySqlCommand(insertActionQuery, conn.connection);
                    insertActionCommand.Parameters.AddWithValue("@Action", action);
                    insertActionCommand.Parameters.AddWithValue("@Timestamp", timestamp);
                    insertActionCommand.ExecuteNonQuery();
                    MessageBox.Show("Lưu Thành Công", "Thành Công");
                    conn.CloseConnection();

                    Home home = new Home();
                    home.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
    }

}
