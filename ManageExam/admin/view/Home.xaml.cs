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
using System.Collections.ObjectModel;

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
                        tintuc.Items.Clear();
                        string query = "SELECT * FROM tintuc ORDER BY idtintuc DESC"; // Sắp xếp theo ID giảm dần
                        MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                        MySqlDataReader reader = cmd.ExecuteReader();

                        List<ListViewItem> items = new List<ListViewItem>(); // Tạo một danh sách tạm thời để lưu các mục

                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem();
                            item.Content = new { Action = reader["hanhdong"].ToString() };
                            items.Add(item); // Thêm vào danh sách tạm thời
                        }

                        // Đảo ngược danh sách tạm thời trước khi thêm vào ListView
                        for (int i =0; i < items.Count;i++)
                        {
                            tintuc.Items.Add(items[i]);
                        }

                        reader.Close();

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



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy button đã được nhấp
                Button button = sender as Button;

                // Lấy ListViewItem chứa button đã nhấp
                ListViewItem item = FindAncestor<ListViewItem>(button);

                // Kiểm tra xem item có tồn tại không
                if (item != null)
                {
                    // Lấy dữ liệu của ListViewItem (trong trường hợp này, dữ liệu tin tức)
                    dynamic data = item.Content;
                    string action = data.Action;

                    // Hiển thị hộp thoại xác nhận
                    MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa mục này?", "Xác nhận xóa", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                    if (result == MessageBoxResult.OK)
                    {
                        // Xóa mục từ cơ sở dữ liệu
                        using (Connection conn = new Connection())
                        {
                            if (conn.OpenConnection())
                            {
                                // Thực hiện truy vấn xóa trong cơ sở dữ liệu
                                string query = "DELETE FROM tintuc WHERE hanhdong = @action";
                                MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                                cmd.Parameters.AddWithValue("@action", action);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                // Kiểm tra xem mục đã được xóa thành công hay không
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Đã xóa mục thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                    // Xóa mục từ ListView
                                    tintuc.Items.Remove(item);
                                }
                                else
                                {
                                    MessageBox.Show("Không thể xóa mục từ cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        // Phương thức hỗ trợ để tìm ListViewItem chứa một Control trong cấu trúc Visual Tree
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            } while (current != null);
            return null;
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
        private void TTHS_Click(object sender, RoutedEventArgs e)
        {
            usercontrol usercontrol = new usercontrol();
            usercontrol.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            help help   = new help();
            help.ShowDialog();
        }
    }
}
