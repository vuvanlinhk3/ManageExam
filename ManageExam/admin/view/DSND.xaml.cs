using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using ManageExam.database;
using System.Collections.ObjectModel;
using ManageExam.admin.view.diaLog;

namespace ManageExam.admin.view
{
    public partial class DSND : UserControl
    {
        public ObservableCollection<User> Users { get; set; }

        public DSND()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
            LoadUsers();
            DataContext = this;
        }

        public class User
        {
            public int IDUSER { get; set; }
            public string nameUSER { get; set; }
            public string matkhauUSER { get; set; }
            public string sdtUSER { get; set; }
            public string emailUSER { get; set; }
            //  public DateTime ngaysinhUSER { get; set; }
            public string gioitinhUSER { get; set; }
            public string quoctich { get; set; }
            public string tinhTp { get; set; }
            public string quanhuyen { get; set; }
        }

        private void LoadUsers()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    if (conn.OpenConnection())
                    {
                        string query = "SELECT * FROM user";
                        MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Users.Add(new User
                                {
                                    IDUSER = reader.GetInt32("IDUSER"),
                                    nameUSER = reader.GetString("nameUSER"),
                                    matkhauUSER = reader.GetString("matkhauUSER"),
                                    sdtUSER = reader.GetString(("sdtUSER")),
                                    //       ngaysinhUSER = reader.GetDateTime("ngaysinhUSER"),
                                    emailUSER = reader.GetString("emailUSER"),
                                    gioitinhUSER = reader.GetString("gioitinhUSER"),
                                    quoctich = reader.GetString("quoctich"),
                                    tinhTp = reader.GetString("tinhTp"),
                                    quanhuyen = reader.GetString("quanhuyen")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu người dùng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button.DataContext as User;
            if (user != null)
            {
                Users.Remove(user);
                try
                {
                    using (Connection conn = new Connection())
                    {
                        if (conn.OpenConnection())
                        {
                            string query = "DELETE FROM user WHERE IDUSER = @id";
                            MySqlCommand cmd = new MySqlCommand(query, conn.connection);
                            cmd.Parameters.AddWithValue("@id", user.IDUSER);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Không thể xóa người dùng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa người dùng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SuaButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as User; // Lấy đối tượng người dùng từ DataContext của nút

            if (user != null)
            {
                Chinhsuangdung chinhsuangdung = new Chinhsuangdung(user.IDUSER);
                chinhsuangdung.ShowDialog();
            }
        }

    }
}
