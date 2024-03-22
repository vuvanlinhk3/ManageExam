using ManageExam.admin.view.addexam;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
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
    }
}
