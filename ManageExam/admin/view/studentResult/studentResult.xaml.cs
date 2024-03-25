using System;
using System.Data;
using System.Windows;
using ManageExam.database;
using MySql.Data.MySqlClient;

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
            using (Connection conn = new Connection())
            {
                if (conn.OpenConnection())
                {
                    string sqlQuery = @"SELECT ketqua.idKETQUA, user.nameUSER, dethi.nameDETHI, dethi.monhocDETHI, ketqua.diemKETQUA
                            FROM ketqua
                            INNER JOIN user ON ketqua.IDUSER = user.IDUSER
                            INNER JOIN dethi ON ketqua.idDETHI = dethi.IDDETHI";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, conn.connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            datagrid.ItemsSource = dataTable.DefaultView;
                        }
                    }

                    conn.CloseConnection();
                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.");
                }
            }
        }
        private void XemBaiThi_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Sua_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
