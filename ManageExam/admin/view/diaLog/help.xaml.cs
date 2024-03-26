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
using System.IO;

namespace ManageExam.admin.view.diaLog
{
    /// <summary>
    /// Interaction logic for help.xaml
    /// </summary>
    public partial class help : Window
    {
        public help()
        {
            InitializeComponent();
            Load();
        }
        public void Load()
        {
            try
            {
                // Đường dẫn đến tệp tin help.txt
                string filePath = "F:\\PROJECT_CSHAP\\ManageExam\\ManageExam\\lib\\help.txt";

                // Kiểm tra xem tệp tin tồn tại
                if (File.Exists(filePath))
                {
                    // Đọc nội dung từ tệp tin
                    string helpContent = File.ReadAllText(filePath);

                    // Gán nội dung vừa đọc vào TextBox
                    help1.Text = helpContent;
                }
                else
                {
                    // Hiển thị thông báo nếu tệp tin không tồn tại
                    MessageBox.Show("Tệp tin help.txt không tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra
                MessageBox.Show("Lỗi khi tải nội dung từ tệp tin: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
