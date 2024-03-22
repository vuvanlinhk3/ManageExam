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
    /// Interaction logic for dapancauhoi.xaml
    /// </summary>
    public partial class dapancauhoi : Window
    {
        public int soddapans = 4;
        public string dapandung;

        public dapancauhoi()
        {

            InitializeComponent();

        }

        private void chonslda_Click(object sender, RoutedEventArgs e)
        {
            soddapans = int.Parse(soddapan.Text);

        }

        public void nextcau_Click(object sender, RoutedEventArgs e)
        {
            //if (a.IsChecked == true)
            //{
            //    dapandung = "A";
            //    Console.WriteLine("A");
            //}
            //if (b.IsChecked == true)
            //{
            //    dapandung = "B";
            //    Console.WriteLine("B");

            //}
            //if (c.IsChecked == true)
            //{
            //    dapandung = "C";
            //    Console.WriteLine("C");

            //}
            //if (d.IsChecked == true)
            //{
            //    dapandung = "D";
            //    Console.WriteLine("D");

            //}

        }
    }
}
