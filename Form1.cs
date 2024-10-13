using BUS1;
using DAL1.Model;
using System;
using System.Collections.Generic;
using System.Collections.Generic;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{


    public partial class Form1 : Form
    {
        private readonly studentService StudentService = new studentService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            List<Student> students = StudentService.GetAll();

            // Tạo DataTable để hiển thị thông tin
            DataTable table = new DataTable();
            table.Columns.Add("MSSV", typeof(int));
            table.Columns.Add("Họ Tên", typeof(string));
            table.Columns.Add("Khoa", typeof(string));
            table.Columns.Add("DTB", typeof(double));
            table.Columns.Add("Chuyên Ngành", typeof(string));

            foreach (var student in students)
            {
                table.Rows.Add(student.StudentID, student.FullName, student.FacultyName, student.AverageScore, student.Major);
            }

            dataGridView1.DataSource = table;

            // Tùy chỉnh độ rộng cột, header text
            dataGridView1.Columns["MSSV"].HeaderText = "Mã SV";
            dataGridView1.Columns["Họ Tên"].HeaderText = "Họ Tên";
            dataGridView1.Columns["Khoa"].HeaderText = "Khoa";
            dataGridView1.Columns["DTB"].HeaderText = "Điểm Trung Bình";
            dataGridView1.Columns["Chuyên Ngành"].HeaderText = "Chuyên Ngành";


        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy dữ liệu từ các textbox
                int studentID = Convert.ToInt32(txtMaSV.Text);
                string fullName = txtTenSV.Text;
                double averageScore = Convert.ToDouble(txtDTB.Text);
                int facultyID = Convert.ToInt32(txtMaKhoa.Text);

                // Tạo sinh viên mới từ dữ liệu nhập vào
                Student newStudent = new Student
                {
                    StudentID = studentID, // Thêm ID sinh viên nếu cần thiết
                    FullName = fullName,
                    AverageScore = averageScore,
                    FacultyID = facultyID
                };

                StudentService.AddStudent(newStudent);
                LoadData(); // Cập nhật lại DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi thêm sinh viên: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                try
                {
                    int studentId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StudentID"].Value);
                    StudentService.DeleteStudent(studentId);
                    LoadData(); // Cập nhật lại DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi xóa sinh viên: " + ex.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                try
                {
                    // Lấy dữ liệu từ các textbox
                    int studentID = Convert.ToInt32(txtMaSV.Text);
                    string fullName = txtTenSV.Text;
                    double averageScore = Convert.ToDouble(txtDTB.Text);
                    int facultyID = Convert.ToInt32(txtMaKhoa.Text);

                    // Lấy sinh viên từ DataGridView
                    int selectedStudentID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StudentID"].Value);
                    Student student = StudentService.GetStudentById(selectedStudentID);

                    // Cập nhật thông tin sinh viên
                    student.FullName = fullName;
                    student.AverageScore = averageScore;
                    student.FacultyID = facultyID;

                    StudentService.UpdateStudent(student);
                    LoadData(); // Cập nhật lại DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi sửa sinh viên: " + ex.Message);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Đảm bảo không click vào tiêu đề
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Chuyển dữ liệu từ hàng được chọn sang TextBox
                txtMaSV.Text = row.Cells["MSSV"].Value.ToString();
                txtTenSV.Text = row.Cells["Họ Tên"].Value.ToString();
                txtDTB.Text = row.Cells["DTB"].Value.ToString();
                txtMaKhoa.Text = row.Cells["Khoa"].Value.ToString();

                // Hiển thị ảnh đại diện
                string avatarPath = row.Cells["Avatar"].Value.ToString();
                if (!string.IsNullOrEmpty(avatarPath) && System.IO.File.Exists(avatarPath))
                {
                    pictureBox1.Image = Image.FromFile(avatarPath);
                }
                else
                {
                    pictureBox1.Image = null; // Nếu không có ảnh thì xóa ảnh cũ
                }
            }
        }
    }
}
