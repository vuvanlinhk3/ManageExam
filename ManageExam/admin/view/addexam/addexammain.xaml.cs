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
        private ObservableCollection<DeThi> exams; // Danh sách chứa các đề thi
        private int stt = 1;
        private ObservableCollection<CauHoi> bangdapan;

        public addexammain(string nameExam)
        {
            InitializeComponent();
            Loaded += AddExamMain_Loaded;
            exams = new ObservableCollection<DeThi>();
            dataGrid.ItemsSource = exams;
            receivedNameExam = nameExam;
            bangdapan = new ObservableCollection<CauHoi>();
            bangdapans.ItemsSource = bangdapan;

        }
        private void AddExamMain_Loaded(object sender, RoutedEventArgs e)
        {
            tendethichinh.Content = receivedNameExam.ToString();
        }
        public class DeThi
        {
            public string NameDETHI { get; set; }
            public string MonHocDETHI { get; set; }
            public string TimeDETHI { get; set; }
            public DateTime NgayTaoDETHI { get; set; }
            public int NguoiTaoDETHI { get; set; }
            public List<AnhDeThi> AnhDeThis { get; set; }
            public List<CauHoi> DanhSachCauHoi { get; set; }
        }

        public class AnhDeThi
        {
            public int IdHA { get; set; }
            public byte[] HinhAnh { get; set; }
            public int IDDETHI { get; set; }
        }

        public class CauHoi
        {
            //public int IdCAUHOI { get; set; }
            public string TenCAUHOI { get; set; }
            //public int LoaiCAUHOI { get; set; }
            public int IDDETHI { get; set; }
            public List<DapAn> DanhSachDapAn { get; set; }
        }

        public class DapAn
        {
            public int IdDAPAN { get; set; }
            public int IdCAUHOI { get; set; }
            public int DapAnDungDAPAN { get; set; }
            public int IDDETHI { get; set; }
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
                DeThi newExam = new DeThi();
                newExam.NameDETHI = "" + stt;
                newExam.AnhDeThis = new List<AnhDeThi>()
        {
            new AnhDeThi() { HinhAnh = ConvertImageToByteArray(bitmap) }
        };
                exams.Add(newExam);

                // Tăng biến đếm lên để sử dụng cho tên đề thi tiếp theo
                stt++;
            }
        }

        private byte[] ConvertImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }




        public void ThayDoi_Click(object sender, RoutedEventArgs e)
        {

        }
        public void Xoa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void thietlapdapan_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(soluongcauhoi.Text) && int.TryParse(soluongcauhoi.Text, out int soLuongCauHoi) && soLuongCauHoi > 0)
            {
                bangdapan.Clear(); // Xóa dữ liệu cũ trong danh sách trước khi thêm mới

                // Lặp qua số lượng câu hỏi được chỉ định và thêm các đáp án vào danh sách
                for (int i = 1; i <= soLuongCauHoi; i++)
                {
                    CauHoi cauHoi = new CauHoi();

                    // Thiết lập tên câu hỏi
                    cauHoi.TenCAUHOI = "Câu hỏi số " + i;

                    // Thiết lập loại câu hỏi (giả sử mặc định là 1)

                    // Tạo danh sách đáp án cho câu hỏi
                    cauHoi.DanhSachDapAn = new List<DapAn>();

                    // Thiết lập số lượng đáp án mặc định là 4 và đáp án đúng mặc định là "A"
                    for (int j = 1; j <= 4; j++)
                    {
                        DapAn dapAn = new DapAn();
                        dapAn.IdCAUHOI = i;
                        dapAn.DapAnDungDAPAN = (j == 1) ? 1 : 0; // Giả sử đáp án đầu tiên là đáp án đúng
                        cauHoi.DanhSachDapAn.Add(dapAn);
                    }

                    bangdapan.Add(cauHoi);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập số lượng câu hỏi là một số nguyên dương hợp lệ.");
            }
        }







        public string LoaiCAUHOI ="a";
        public int NguoiTao = 1;
        public string MonHoc = "Tiếng Anh";
        public int selectedHour;
        public string NgayTao = DateTime.Now.ToString();
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        foreach (var exam in exams)
                        {
                            // Lưu thông tin đề thi vào bảng dethi
                            string insertExamQuery = "INSERT INTO dethi (nameDETHI, monhocDETHI, timeDETHI, ngaytaoDETHI, nguoitaoDETHI) VALUES (@TenDeThi, @MonHoc, @Time, @NgayTao, @NguoiTao)";
                            MySqlCommand insertExamCmd = new MySqlCommand(insertExamQuery, conn.connection);
                            exam.NameDETHI = tendethichinh.Content.ToString();
                            exam.MonHocDETHI = "Tiếng Anh";
                            string hour;
                            string minute;

                            // Lấy giá trị từ TextBox của giờ và phút
                            hour = hourTextBox.Text;
                            minute = minuteTextBox.Text;

                            // Kết hợp giá trị giờ và phút thành một chuỗi
                            string timeString = hour + ":" + minute;
                            exam.TimeDETHI = timeString;

                            exam.NgayTaoDETHI = DateTime.Now;

                            exam.NguoiTaoDETHI = 1;

                            insertExamCmd.Parameters.AddWithValue("@TenDeThi", exam.NameDETHI);
                            insertExamCmd.Parameters.AddWithValue("@MonHoc", exam.MonHocDETHI);
                            insertExamCmd.Parameters.AddWithValue("@Time", exam.TimeDETHI);
                            insertExamCmd.Parameters.AddWithValue("@NgayTao", exam.NgayTaoDETHI);
                            insertExamCmd.Parameters.AddWithValue("@NguoiTao", exam.NguoiTaoDETHI);
                            insertExamCmd.ExecuteNonQuery();

                            // Lấy IDDETHI của đề thi vừa được tạo
                            int idExam = (int)insertExamCmd.LastInsertedId;

                            // Lưu hình ảnh vào bảng anhdethi
                            foreach (var anhDeThi in exam.AnhDeThis)
                            {
                                string insertImageQuery = "INSERT INTO anhdethi (HinhAnh, IDDETHI) VALUES (@HinhAnh, @IDDETHI)";
                                MySqlCommand insertImageCmd = new MySqlCommand(insertImageQuery, conn.connection);
                                insertImageCmd.Parameters.AddWithValue("@HinhAnh", anhDeThi.HinhAnh);
                                insertImageCmd.Parameters.AddWithValue("@IDDETHI", idExam);
                                insertImageCmd.ExecuteNonQuery();
                            }

                            foreach (var cauHoi in exam.DanhSachCauHoi)
                            {
                                // Lưu câu hỏi vào bảng cauhoi
                                string insertCauHoiQuery = "INSERT INTO cauhoi (tenCAUHOI, loaiCAUHOI, IDDETHI) VALUES (@TenCauHoi, @LoaiCauHoi, @IDDeThi)";
                                MySqlCommand insertCauHoiCmd = new MySqlCommand(insertCauHoiQuery, conn.connection);
                                insertCauHoiCmd.Parameters.AddWithValue("@TenCauHoi", cauHoi.TenCAUHOI);
                                insertCauHoiCmd.Parameters.AddWithValue("@LoaiCauHoi", LoaiCAUHOI);
                                insertCauHoiCmd.Parameters.AddWithValue("@IDDeThi", idExam);
                                insertCauHoiCmd.ExecuteNonQuery();

                                // Lấy IDCAUHOI của câu hỏi vừa được tạo
                                int idCauHoi = (int)insertCauHoiCmd.LastInsertedId;

                                foreach (var dapAn in cauHoi.DanhSachDapAn)
                                {
                                    // Lưu đáp án vào bảng dapan
                                    string insertDapAnQuery = "INSERT INTO dapan (IdCAUHOI, DapAnDungDAPAN, IDDETHI) VALUES (@IdCAUHOI, @DapAnDung, @IDDETHI)";
                                    MySqlCommand insertDapAnCmd = new MySqlCommand(insertDapAnQuery, conn.connection);
                                    insertDapAnCmd.Parameters.AddWithValue("@IdCAUHOI", idCauHoi);
                                    insertDapAnCmd.Parameters.AddWithValue("@DapAnDung", dapAn.DapAnDungDAPAN);
                                    insertDapAnCmd.Parameters.AddWithValue("@IDDETHI", idExam);
                                    insertDapAnCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        MessageBox.Show("Dữ liệu đã được lưu thành công vào cơ sở dữ liệu.");
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu dữ liệu vào cơ sở dữ liệu: " + ex.Message);
            }
        }

        private void SetTime_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
