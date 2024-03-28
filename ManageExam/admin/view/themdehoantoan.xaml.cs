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

namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for themdehoantoan.xaml
    /// </summary>
    public partial class themdehoantoan : Window
    {
        public themdehoantoan()
        {
            InitializeComponent();
            Load();
        }
        public void Load()
        {
            string[] subjects = { "Toán", "Ngữ văn", "Tiếng Anh",  "Hóa học"};

            // Xóa tất cả các mục hiện có trong ComboBox (nếu có)
            monhoc.Items.Clear();

            // Thêm các giá trị vào ComboBox từ mảng
            foreach (string subject in subjects)
            {
                monhoc.Items.Add(subject);
            }

            // Nếu bạn muốn chọn một giá trị mặc định, bạn có thể thiết lập SelectedIndex hoặc SelectedItem
            monhoc.SelectedIndex = 2;
        }
        private void thoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public string tende;
        public string mon;
        public string thoigian;
        public int iddethi;
        public DateTime thoigiantaode;
        public string filedethi = null;
        
        private void Next(object sender, RoutedEventArgs e)
        {
            int nguoitao = 1;
            tende = tendethi.Text;
            mon = monhoc.Text;
            thoigian = $"{hourTextBox.Text} : {minuteTextBox.Text}";
            thoigiantaode = DateTime.Now;
            try
            {
                using (Connection con = new Connection())
                {
                    // Mở kết nối tới cơ sở dữ liệu
                    con.OpenConnection();

                    // Chuẩn bị câu lệnh SQL để thêm dữ liệu vào bảng dethi
                    // Chuẩn bị câu lệnh SQL để thêm dữ liệu vào bảng dethi
                    string query = "INSERT INTO dethi (nameDETHI, monhocDETHI, timeDETHI, ngaytaoDETHI, nguoitaoDETHI) " +
                                   "VALUES (@tende, @mon, @thoigian, @thoigiantaode, @nguoitao)";

                    using (MySqlCommand command = new MySqlCommand(query, con.connection))
                    {
                        // Thêm các tham số vào câu lệnh SQL
                        command.Parameters.AddWithValue("@tende", tende);
                        command.Parameters.AddWithValue("@mon", mon);
                        command.Parameters.AddWithValue("@thoigian", thoigian);
                        command.Parameters.AddWithValue("@thoigiantaode", thoigiantaode);
                        command.Parameters.AddWithValue("@nguoitao", nguoitao);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();
                        iddethi = (int)command.LastInsertedId;
                        // Đóng kết nối
                        con.CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý nếu có lỗi xảy ra
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }




            themdekieukhac themdekieukhac = new themdekieukhac(tende , mon , thoigian,iddethi );
            themdekieukhac.Show();
            //ressuitclose
            DialogResult = true;
            this.Close();

        }
    }
}
