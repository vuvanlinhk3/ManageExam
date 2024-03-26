using ManageExam.admin.view;
using System.Windows.Controls;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using ManageExam.database;
using System.Collections.ObjectModel;

namespace ManageExam.admin.view
{
    public partial class usercontrol : Window
    {


        public usercontrol()
        {
            InitializeComponent();
        }
        private void danhsach(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DSND, 3);
            Panel.SetZIndex(AddU, 1);
        }

        private void chinhsua(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DSND, 1);
            Panel.SetZIndex(AddU, 2);
        }

        private void them(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(DSND, 1);
            Panel.SetZIndex(AddU, 3);
        }


        private void DSND_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void thoat_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }
    }
}
