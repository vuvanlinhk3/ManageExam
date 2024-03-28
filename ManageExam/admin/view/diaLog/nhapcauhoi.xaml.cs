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

namespace ManageExam.admin.view.diaLog
{
    /// <summary>
    /// Interaction logic for nhapcauhoi.xaml
    /// </summary>
    public partial class nhapcauhoi : Window
    {
       
        public nhapcauhoi(int iddethi, int demcau)
        {
            InitializeComponent();
            this.iddethi = iddethi;
            this.demcau = demcau;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // hàm thoát
        }
        public int iddethi;
        public int demcau;
        public static int idCauHoi;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (text.Text !="")
            {
                try
                {
                    if (!string.IsNullOrEmpty(text.Text))
                    {
                        using (Connection con = new Connection())
                        {
                            con.OpenConnection(); // Mở kết nối

                            // Chuẩn bị câu lệnh SQL để thêm nội dung câu hỏi vào bảng txtcauhoi
                            string txtQuery = "INSERT INTO txtcauhoi (noidung) VALUES (@noidung)";
                            using (MySqlCommand txtCommand = new MySqlCommand(txtQuery, con.connection))
                            {
                                txtCommand.Parameters.AddWithValue("@noidung", text.Text);

                                // Thực thi câu lệnh SQL để thêm nội dung câu hỏi vào bảng txtcauhoi
                                txtCommand.ExecuteNonQuery();

                                // Lấy ID của câu hỏi vừa được thêm
                                int idTxtCauHoi = (int)txtCommand.LastInsertedId;

                                // Chuẩn bị câu lệnh SQL để thêm bản ghi mới vào bảng cauhoi
                                string cauHoiQuery = "INSERT INTO cauhoi (tenCAUHOI,idTxtcauhoi, IDDETHI) VALUES (@tenCAUHOI,@idTxtCauHoi, @IDDethi)";
                                using (MySqlCommand cauHoiCommand = new MySqlCommand(cauHoiQuery, con.connection))
                                {
                                    string tenCH = $"Câu: {demcau}";
                                    // Thêm các tham số vào câu lệnh SQL
                                    cauHoiCommand.Parameters.AddWithValue("@tenCAUHOI", tenCH);
                                    cauHoiCommand.Parameters.AddWithValue("@idTxtCauHoi", idTxtCauHoi);
                                    cauHoiCommand.Parameters.AddWithValue("@IDDethi", iddethi);

                                    // Thực thi câu lệnh SQL để thêm bản ghi mới vào bảng cauhoi
                                    cauHoiCommand.ExecuteNonQuery();
                                    idCauHoi = (int)cauHoiCommand.LastInsertedId;

                                }
                            }

                            // Đóng kết nối
                            con.CloseConnection();

                            MessageBox.Show("Thêm câu hỏi thành công!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng không để trống nội dung câu hỏi!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
                }


                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Vui Lòng Không để trống !");
            }
        }
        public static int getIdCauHoi()
        {
            return idCauHoi;
        }
    }
}
