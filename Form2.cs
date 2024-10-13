using BUS4;
using DAL4;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DAL4.entities;


namespace MoHinh3Lop
{
    public partial class frmDangKiChuyenNganh : Form
    {
        private StudentBUS studentBLL;

        public frmDangKiChuyenNganh(StudentBUS studentBus)
        {
            InitializeComponent();
            studentBLL =  studentBus;
            LoadComboBoxData();
            InitializeDataGridView();  // Gọi hàm khởi tạo DataGridView

        }
        private void InitializeDataGridView()
        {
            dgvDKCN.Columns.Clear();
            dgvDKCN.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Chon", HeaderText = "Chọn" });
            dgvDKCN.Columns.Add("MSSV", "Mã SV");
            dgvDKCN.Columns.Add("HoTen", "Họ Tên");
            dgvDKCN.Columns.Add("Khoa", "Khoa");
            dgvDKCN.Columns.Add("DiemTB", "Điểm TB");
        }

        private void LoadStudentData()
        {
            if (cmbKhoa.SelectedValue != null)
            {
                int facultyID;
                if (!int.TryParse(cmbKhoa.SelectedValue.ToString(), out facultyID))
                {
                    MessageBox.Show("Giá trị khoa không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var studentList = studentBLL.GetAllStudentsWithNoMajor().Where(s => s.FacultyID == facultyID).ToList();
                BindGrid(studentList);
            }
        }

        private void LoadComboBoxData()
        {
            var faculties = studentBLL.GetAllFaculties();
            cmbKhoa.DataSource = faculties;
            cmbKhoa.DisplayMember = "FacultyName";
            cmbKhoa.ValueMember = "FacultyID";

            var majors = studentBLL.GetAllMajors();
            cmbNganh.DataSource = majors;
            cmbNganh.DisplayMember = "Name";
            cmbNganh.ValueMember = "MajorID";
        }
        private void dgvDKCN_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmDangKiChuyenNganh_Load(object sender, EventArgs e)
        {

        }

        private void cmbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadStudentData();


        }
        private void BindGrid(List<Student> listStudent)
        {
            dgvDKCN.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvDKCN.Rows.Add();
                dgvDKCN.Rows[index].Cells["MSSV"].Value = item.StudentID;
                dgvDKCN.Rows[index].Cells["HoTen"].Value = item.FullName;
                dgvDKCN.Rows[index].Cells["Khoa"].Value = item.Faculty != null ? item.Faculty.FacultyName : "";
                dgvDKCN.Rows[index].Cells["DiemTB"].Value = item.AverageScore != null ? item.AverageScore.ToString() : "";
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                int? majorID = string.IsNullOrEmpty(cmbNganh.Text) ? (int?)null : int.Parse(cmbNganh.SelectedValue.ToString());

                if (majorID == null)
                {
                    MessageBox.Show("Vui lòng chọn chuyên ngành!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (DataGridViewRow row in dgvDKCN.Rows)
                {
                    var cellValue = row.Cells["Chon"].Value;
                    if (cellValue != null && Convert.ToBoolean(cellValue) == true)
                    {
                        string studentID = row.Cells["MSSV"].Value.ToString();
                        Student student = studentBLL.GetStudentById(studentID);
                        if (student != null)
                        {
                            student.MajorID = majorID;
                            studentBLL.AddOrUpdateStudent(student);
                        }
                    }
                }

                MessageBox.Show("Đăng ký chuyên ngành thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudentData();
                this.DialogResult = DialogResult.OK;
                this.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
