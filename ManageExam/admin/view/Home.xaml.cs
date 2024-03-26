using ManageExam.admin.view.addexam;
using ManageExam.admin.view.diaLog;
using ManageExam.admin.view.viewExam;
using ManageExam.database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            Load();
            
        }
       
        public void Load()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        // Load lịch sử từ cơ sở dữ liệu và hiển thị trên ListView
                        string query = "SELECT * FROM tintuc ORDER BY idtintuc DESC"; // Sắp xếp theo id giảm dần để lấy các mục mới nhất đầu tiên
                        MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        List<ListViewItem> items = new List<ListViewItem>(); // Mảng động để lưu các mục từ cơ sở dữ liệu

                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem();
                            item.Content = reader["hanhdong"].ToString();
                            items.Add(item); // Thêm mục vào mảng
                        }

                        reader.Close();

                        // Thêm các mục từ mảng vào ListView theo thứ tự ngược lại (từ mới nhất đến cũ nhất)
                        for (int i = 0; i <items.Count;i++)
                        {
                            tintuc.Items.Add(items[i]);
                        }

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

        private void addExam(object sender, RoutedEventArgs e)
        {
            nameaddexam dialog = new nameaddexam();
            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                // Đã click vào nút trong dialog, đóng cả hai cửa sổ
                Close();
            }
        }
        private void themlichthi_Click(object sender, RoutedEventArgs e)
        {


            this.Close();
        }
        private void DSde_Click(object sender, RoutedEventArgs e)
        {
            viewImgExam viewImgExam = new viewImgExam();
            viewImgExam.Show();

            this.Close();
        }
        private void KQ_click(object sender, RoutedEventArgs e)
        {
            studentResult st = new studentResult();
            st.Show();

            this.Close();
        }
        private void themtintuc_Click(object sender, RoutedEventArgs e)
        {
            selectAddExam selectAddExam = new selectAddExam();

            bool? result = selectAddExam.ShowDialog();
            if (result.HasValue && result.Value)
            {
                
                Load();
            }

        }

    }
}
