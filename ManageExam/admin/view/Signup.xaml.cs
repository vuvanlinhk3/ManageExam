﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using ManageExam.database;
using MySql.Data.MySqlClient;

namespace ManageExam.admin.view
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void screenMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void screenClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void toLoginTab_Click(object sender, RoutedEventArgs e)
        {
            //LoginWindow loginWindowInstance = new LoginWindow();

            //loginWindowInstance.Owner = this;

            //loginWindowInstance.ShowDialog();
        }

        private void resgisterBtn_Click(object sender, RoutedEventArgs e)
        {
            using (Connection conn = new Connection())
            {
                try
                {
                    if (password.Text != passwordAuth.Text)
                    {
                        MessageBox.Show("Mật khẩu nhập lại không khớp. Vui lòng nhập lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    conn.OpenConnection();
                    string dataInsert = "INSERT INTO user (nameUSER, matkhauUSER) VALUES (@name, @password)";
                    MySqlCommand cmd = new MySqlCommand(dataInsert, conn.connection);
                    cmd.Parameters.AddWithValue("@name", "Tên người dùng"); // Thay "Tên người dùng" bằng giá trị thực tế bạn muốn thêm
                    cmd.Parameters.AddWithValue("@password", "Mật khẩu"); // Thay "Mật khẩu" bằng giá trị thực tế bạn muốn thêm
                    cmd.ExecuteNonQuery();
                    conn.CloseConnection();
                    username.Text = "";
                    password.Text = "";     
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
