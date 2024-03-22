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

namespace ManageExam.admin.view.addexam
{
    /// <summary>
    /// Interaction logic for nameaddexam.xaml
    /// </summary>
    public partial class nameaddexam : Window
    {
        public string nameExam { get; private set; }
        public nameaddexam()
        {
            InitializeComponent();
        }
        private void Next(object sender, RoutedEventArgs e)
        {   // nếu đề thi không null thì mới chạy
            if(!string.IsNullOrEmpty(tendethi.Text))
            {
                nameExam = tendethi.Text;



                DialogResult = true;// đóng cả 2 tab home và này

                addexammain addexammain = new addexammain(nameExam);    
                addexammain.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên đề thi");
            }



        }
    }
}
