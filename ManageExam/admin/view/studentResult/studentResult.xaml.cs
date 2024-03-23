﻿using System;
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
using System.Data;
using ManageExam.database;
using MySql.Data.MySqlClient;


namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for studentResult.xaml
    /// </summary>
    public partial class studentResult : Window
    {
        //string connectstring = @"";
        //SqlConnection connection;
        MySqlCommand cmd;
        MySqlDataAdapter adt;
        Connection conn;
        DataTable dt = new DataTable();
        public studentResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new Connection();
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (conn.OpenConnection())
                {
                    string query = "SELECT * FROM ketqua";
                    MySqlCommand cmd = new MySqlCommand(query, conn.connection);

                    MySqlDataAdapter adt = new MySqlDataAdapter(cmd);
                    adt.Fill(dt);
                    datagrid.ItemsSource = dt.DefaultView;
                    conn.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
