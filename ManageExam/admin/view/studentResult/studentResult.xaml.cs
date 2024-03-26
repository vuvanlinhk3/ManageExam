using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using ManageExam.admin.view.viewExam;
using ManageExam.database;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;

namespace ManageExam.admin.view
{
    public partial class studentResult : Window
    {
       
        public studentResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datagrid.ItemsSource = null;
        }
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.OpenConnection();

                    string selectQuery = @"
                SELECT kq.idKETQUA, kq.idDETHI, u.nameUSER AS TenNguoiDung, dt.nameDETHI AS TenDeThi, dt.monhocDETHI AS MonThi, kq.diemKETQUA AS Diem
                FROM ketqua kq
                INNER JOIN user u ON kq.IDUSER = u.IDUSER
                INNER JOIN dethi dt ON kq.idDETHI = dt.IDDETHI";

                    MySqlCommand command = new MySqlCommand(selectQuery, conn.connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    List<KetQuaItem> ketQuaItems = new List<KetQuaItem>();

                    while (reader.Read())
                    {
                        KetQuaItem item = new KetQuaItem()
                        {
                            IdKetQua = reader["idKETQUA"].ToString(),
                            IdDeThis = reader["idDETHI"].ToString(), // Lấy ID đề thi
                            TenNguoiDung = reader["TenNguoiDung"].ToString(),
                            TenDeThi = reader["TenDeThi"].ToString(),
                            MonThi = reader["MonThi"].ToString(),
                            Diem = Convert.ToDouble(reader["Diem"]) // Chuyển đổi dữ liệu kiểu double
                        };

                        ketQuaItems.Add(item);
                    }

                    reader.Close();

                    // Gán danh sách các đối tượng vào ItemsSource của DataGrid
                    datagrid.ItemsSource = ketQuaItems;

                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }



        public class KetQuaItem
        {
            public string IdKetQua { get; set; }
            public string TenNguoiDung { get; set; }
            public string TenDeThi { get; set; }
            public string MonThi { get; set; }

            public double Diem { get; set; }
            public string IdDeThis { get; set; }

        }

        private void XemBaiThi_Click(object sender, RoutedEventArgs e)
        {
            if (datagrid.SelectedItem != null)
            {
                KetQuaItem selectedItem = (KetQuaItem)datagrid.SelectedItem;
                int idDeThi = Convert.ToInt32(selectedItem.IdDeThis);

                SingerExam singerExam = new SingerExam(idDeThi);
                singerExam.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một kết quả để xem bài thi.");
            }
        }


        private void screenMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void screenClose_Click(object sender, RoutedEventArgs e)
        {
            Home homeWindow = new Home();
            homeWindow.Show();
            this.Close();
        }
    }
}
