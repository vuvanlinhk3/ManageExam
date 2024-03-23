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

namespace ManageExam.admin.view.addexam
{
    public partial class addexammain : Window
    {
        private string receivedNameExam;
        // Danh sách chứa các đề thi
        private ObservableCollection<DeThi> danhSachDeThi;
        private ObservableCollection<dapan> bangdapans;

        // Biến đếm để tạo tên đề thi theo thứ tự
        private int stt = 1;

        public addexammain(string nameExam)
        {
            InitializeComponent();
            Loaded += Addexammain_Loaded;
            danhSachDeThi = new ObservableCollection<DeThi>();
            bangdapans = new ObservableCollection<dapan>();
            dataGrid.ItemsSource = danhSachDeThi; // Thiết lập nguồn dữ liệu cho DataGrid
            bangdapan.ItemsSource = bangdapans; // Thiết lập nguồn dữ liệu cho DataGrid
            receivedNameExam = nameExam;

        }
        private void Addexammain_Loaded(object sender, RoutedEventArgs e)
        {
            // Thực hiện các thao tác load dữ liệu ở đây
            LoadData();
        }

        private void LoadData()
        {
            tendethichinh.Content = receivedNameExam.ToString();
        }
        // Sự kiện click nút để thêm hình ảnh
        private void themanh(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif)|*.jpg; *.jpeg; *.png; *.gif|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));

                // Tạo một đối tượng DeThi mới và thêm vào danh sách
                DeThi newDeThi = new DeThi();
                newDeThi.TenDeThi = "" + stt;
                newDeThi.Image = bitmap;
                danhSachDeThi.Add(newDeThi);

                // Tăng biến đếm lên để sử dụng cho tên đề thi tiếp theo
                stt++;
            }
        }

        public void ThayDoi_Click(object sender, RoutedEventArgs e)
        {

        }
        public void Xoa_Click(object sender, RoutedEventArgs e)
        {

        }
        public string soluongcauhois { get; private set; }

        private void thietlapdapan_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(soluongcauhoi.Text) && int.TryParse(soluongcauhoi.Text, out int soLuongCauHoi))
            {
                bangdapans.Clear(); // Xóa dữ liệu cũ trong danh sách trước khi thêm mới
                int s = 1;
                // Thêm các câu hỏi vào danh sách với số thứ tự từ 1 đến soLuongCauHoi
                for (int i = 1; i <= soLuongCauHoi; i++)
                {
                    dapan dapan = new dapan();
                    dapan.cau = "" + s; // Số thứ tự câu hỏi
                    bangdapans.Add(dapan);
                    s++;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số lượng câu hỏi là một số nguyên hợp lệ.");
            }
        }

        public class dapan
        {
            public string cau { get; set; }
            public string SoLuongDapAn { get; set; }
            public string DapAnDung { get; set; }
        }

        // Lớp DeThi để lưu trữ thông tin về đề thi
        public class DeThi
        {
            public string TenDeThi { get; set; }
            public BitmapImage Image { get; set; }
        }


        private void save_Click(object sender, RoutedEventArgs e)
        {
            using (Connection conn = new Connection())
            {
                if (conn.OpenConnection())
                {
                    try
                    {
                        foreach (var cauHoi in bangdapans)
                        {
                            string query = "INSERT INTO CauHoi (Cau, SoLuongDapAn, DapAnDung) VALUES (@Cau, @SoLuongDapAn, @DapAnDung)";
                            MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                            cmd.Parameters.AddWithValue("@Cau", cauHoi.cau);
                            cmd.Parameters.AddWithValue("@SoLuongDapAn", cauHoi.SoLuongDapAn);
                            cmd.Parameters.AddWithValue("@DapAnDung", cauHoi.DapAnDung);
                            cmd.ExecuteNonQuery();
                        }

                        foreach (var deThi in danhSachDeThi)
                        {
                            // Chuyển đổi hình ảnh thành mảng byte để lưu vào cơ sở dữ liệu
                            byte[] imageBytes;
                            using (var stream = new MemoryStream())
                            {
                                PngBitmapEncoder encoder = new PngBitmapEncoder();
                                encoder.Frames.Add(BitmapFrame.Create(deThi.Image));
                                encoder.Save(stream);
                                imageBytes = stream.ToArray();
                            }

                            string query = "INSERT INTO DeThi (TenDeThi, HinhAnh) VALUES (@TenDeThi, @HinhAnh)";
                            MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                            cmd.Parameters.AddWithValue("@TenDeThi", deThi.TenDeThi);
                            cmd.Parameters.AddWithValue("@HinhAnh", imageBytes);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Dữ liệu đã được lưu thành công vào cơ sở dữ liệu.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi lưu dữ liệu vào cơ sở dữ liệu: " + ex.Message);
                    }
                    finally
                    {
                        conn.CloseConnection();
                    }
                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                }
            }
        }


    }
}
