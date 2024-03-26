using ManageExam.database;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace ManageExam.admin.view.viewExam
{
    public partial class SingerExam : System.Windows.Window // Chỉ rõ namespace cho Window
    {
        public int MADE;

        public SingerExam(int madethi)
        {
            InitializeComponent();
            MADE = madethi;
            Load();
        }

        public void Load()
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.connection.Open();

                    string selectQuery = "SELECT * FROM dethi WHERE IDDETHI = @id";
                    MySqlCommand command = new MySqlCommand(selectQuery, conn.connection);
                    command.Parameters.AddWithValue("@id", MADE);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        txtTenDeThi.Text = reader["nameDETHI"].ToString();
                        txtMonHoc.Text = reader["monhocDETHI"].ToString();
                        txtThoiGianThi.Text = reader["timeDETHI"].ToString();
                        tenfile.Content = reader["filedethi"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu đề thi.");
                        return; // Thoát khỏi phương thức nếu không có dữ liệu đề thi
                    }

                    // Đóng reader sau khi sử dụng
                    reader.Close();

                    // Mở reader mới để truy vấn bảng "cauhoi" và "dapan"
                    selectQuery = @"
                SELECT cauhoi.tenCAUHOI, dapan.tenDAPAN 
                FROM cauhoi 
                LEFT JOIN dapan ON cauhoi.idCAUHOI = dapan.idCAUHOI 
                WHERE cauhoi.IDDETHI = @id";
                    command = new MySqlCommand(selectQuery, conn.connection);
                    command.Parameters.AddWithValue("@id", MADE);
                    MySqlDataReader renders = command.ExecuteReader();

                    // Dictionary để lưu trữ câu hỏi và danh sách các đáp án đúng
                    Dictionary<string, List<string>> questions = new Dictionary<string, List<string>>();

                    while (renders.Read())
                    {
                        string question = renders["tenCAUHOI"].ToString();
                        string answer = renders["tenDAPAN"].ToString();

                        if (!questions.ContainsKey(question))
                        {
                            questions[question] = new List<string>();
                        }

                        questions[question].Add(answer);
                    }
                    renders.Close();

                    // Hiển thị dữ liệu trong ListView
                    listViewQuestions.Items.Clear();

                    foreach (var item in questions)
                    {
                        string question = item.Key;
                        List<string> answers = item.Value;
                        string correctAnswersString = string.Join(", ", answers);

                        ListViewItem questionItem = new ListViewItem();
                        questionItem.Content = question + " - Đáp án đúng: " + correctAnswersString;
                        listViewQuestions.Items.Add(questionItem);
                    }

                    // Đóng kết nối
                    conn.connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                Console.WriteLine("\n \n \n \n Lỗi :::" + ex.Message);
            }
        }



        private void xemDe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Connection conn = new Connection())
                {
                    conn.connection.Open();

                    string selectQuery = "SELECT filedethi FROM dethi WHERE IDDETHI = @id";
                    MySqlCommand command = new MySqlCommand(selectQuery, conn.connection);
                    command.Parameters.AddWithValue("@id", MADE);
                    object fileData = command.ExecuteScalar();

                    if (fileData != null)
                    {
                        // Lưu file dự thi vào một tệp tạm thời
                        string tempFilePath = System.IO.Path.GetTempFileName();
                        System.IO.File.WriteAllBytes(tempFilePath, (byte[])fileData);

                        // Mở tệp đề thi bằng chương trình mặc định
                        System.Diagnostics.Process.Start(tempFilePath);
                    }
                    else
                    {
                        MessageBox.Show("Không có tệp đề thi.");
                    }

                    conn.connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

    }
}
