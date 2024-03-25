using System;
using System.Windows;
using System.Data;
using MySql.Data.MySqlClient;
using ManageExam.database;

namespace ManageExam.admin.view.viewExam
{
    public partial class viewImgExam : Window
    {
        public viewImgExam()
        {
            InitializeComponent();
            LoadData();
        }
        public class ExamItem
        {
            public int MaDeThi { get; set; }
            public string TenDeThi { get; set; }
            public string MonThi { get; set; }
            public int SoLuongCauHoi { get; set; }
        }

        public void LoadData()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        string query = "SELECT dethi.IDDETHI AS MaDeThi, dethi.nameDETHI AS TenDeThi, dethi.monhocDETHI AS MonThi, COUNT(cauhoi.idCAUHOI) AS SoLuongCauHoi FROM dethi LEFT JOIN cauhoi ON dethi.IDDETHI = cauhoi.IDDETHI GROUP BY dethi.IDDETHI, dethi.nameDETHI, dethi.monhocDETHI";
                        MySqlCommand command = new MySqlCommand(query, conn.connection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGrid.ItemsSource = dt.DefaultView;

                        conn.connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void XemDeThi_Click(object sender, RoutedEventArgs e)
        {
            // Thêm xử lý khi nhấn nút Xem đề thi
        }

        private void XoaDeThi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn xóa đề này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (Connection conn = new Connection())
                    {
                        if (conn.OpenConnection())
                        {
                            // Lấy hàng đã chọn trong DataGrid
                            DataRowView row = (DataRowView)dataGrid.SelectedItem;
                            if (row != null)
                            {
                                // Lấy ID của đề thi từ hàng đã chọn
                                int maDeThi = Convert.ToInt32(row["MaDeThi"]);

                                // Xóa từ bảng ketqua
                                string deleteKetQuaQuery = "DELETE FROM ketqua WHERE idDETHI = @MaDeThi";
                                MySqlCommand deleteKetQuaCommand = new MySqlCommand(deleteKetQuaQuery, conn.connection);
                                deleteKetQuaCommand.Parameters.AddWithValue("@MaDeThi", maDeThi);
                                deleteKetQuaCommand.ExecuteNonQuery();

                                // Xóa từ bảng dapan
                                string deleteDapAnQuery = "DELETE FROM dapan WHERE IDDETHI = @MaDeThi";
                                MySqlCommand deleteDapAnCommand = new MySqlCommand(deleteDapAnQuery, conn.connection);
                                deleteDapAnCommand.Parameters.AddWithValue("@MaDeThi", maDeThi);
                                deleteDapAnCommand.ExecuteNonQuery();

                                // Xóa từ bảng cauhoi
                                string deleteCauHoiQuery = "DELETE FROM cauhoi WHERE IDDETHI = @MaDeThi";
                                MySqlCommand deleteCauHoiCommand = new MySqlCommand(deleteCauHoiQuery, conn.connection);
                                deleteCauHoiCommand.Parameters.AddWithValue("@MaDeThi", maDeThi);
                                deleteCauHoiCommand.ExecuteNonQuery();

                                // Xóa từ bảng anhdethi
                                string deleteAnhDeThiQuery = "DELETE FROM anhdethi WHERE IDDETHI = @MaDeThi";
                                MySqlCommand deleteAnhDeThiCommand = new MySqlCommand(deleteAnhDeThiQuery, conn.connection);
                                deleteAnhDeThiCommand.Parameters.AddWithValue("@MaDeThi", maDeThi);
                                deleteAnhDeThiCommand.ExecuteNonQuery();

                                // Xóa từ bảng dethi
                                string deleteDeThiQuery = "DELETE FROM dethi WHERE IDDETHI = @MaDeThi";
                                MySqlCommand deleteDeThiCommand = new MySqlCommand(deleteDeThiQuery, conn.connection);
                                deleteDeThiCommand.Parameters.AddWithValue("@MaDeThi", maDeThi);
                                deleteDeThiCommand.ExecuteNonQuery();

                                // Load lại dữ liệu sau khi xóa
                                LoadData();

                                MessageBox.Show("Đã xóa đề thi và các thông tin liên quan.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Vui lòng chọn đề thi cần xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi xóa đề thi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void timkiem_Click(object sender, RoutedEventArgs e)
        {
            string searchText = inputtinkem.Text.Trim();

            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        string query = "SELECT dethi.IDDETHI AS MaDeThi, dethi.nameDETHI AS TenDeThi, dethi.monhocDETHI AS MonThi, COUNT(cauhoi.idCAUHOI) AS SoLuongCauHoi FROM dethi LEFT JOIN cauhoi ON dethi.IDDETHI = cauhoi.IDDETHI WHERE dethi.IDDETHI LIKE @SearchText OR dethi.nameDETHI LIKE @SearchText GROUP BY dethi.IDDETHI, dethi.nameDETHI, dethi.monhocDETHI";
                        MySqlCommand command = new MySqlCommand(query, conn.connection);
                        command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Không tìm thấy kết quả nào phù hợp.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        dataGrid.ItemsSource = dt.DefaultView;

                        conn.connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
