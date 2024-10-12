using BLL;
using DAL.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace Lab06
{
    public partial class Form1 : Form
    {
        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvStudent);
                var lisFacultys = facultyService.GeAll();
                var lisStudents = studentService.GetAll();
                FillFacultyCombobox(lisFacultys);
                BindGrid(lisStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillFacultyCombobox(List<Faculty> facultyList)
        {
            facultyList.Insert(0, new Faculty());
            this.cmbKhoa.DataSource = facultyList;
            this.cmbKhoa.DisplayMember = "FacultyName";
            this.cmbKhoa.ValueMember = "FacultyID";
        }
        private void BindGrid(List<Student> studentList)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in studentList)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                if(item.Faculty != null)
                    dgvStudent.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells[3].Value = item.AverageScore + "";
                if(item.MajorID != null)
                    dgvStudent.Rows[index].Cells[4].Value = item.Major.Name+"";
                //ShowAvatar(item.Avatar); 
            }    
        }
        private void ShowAvatar (string ImageName)
        {
            if(string.IsNullOrEmpty(ImageName))
            {
                picAvata.Image = null;
            }  
            else
            {
                string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
                string imagePath = Path.Combine(parentDirectory,"images", ImageName);
                picAvata.Image = Image.FromFile(imagePath);
                picAvata.Refresh();
            }    
        }
        private void setGridViewStyle(DataGridView dgView)
        {
            dgView.BorderStyle = BorderStyle.None;
            dgView.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgView.BackgroundColor = Color.White;
            dgView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void chkChuaDK_CheckedChanged(object sender, EventArgs e)
        {
            var listStudent = new List<Student>();
            if(chkChuaDK.Checked)
            {
                listStudent = studentService.GetAllHasNoMajor();
            }
            else
            {
                listStudent = studentService.GetAll();
                
            }
            BindGrid(listStudent);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtMa.Text.Trim(), out int studentIdInt))
                {
                    MessageBox.Show("Vui lòng nhập ID sinh viên hợp lệ.");
                    return;
                }
                string fullName = txtHoTen.Text.Trim();
                if (string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Tên sinh viên không được để trống.");
                    return;
                }

                if (!float.TryParse(txtDiem.Text.Trim(), out float averageScore))
                {
                    MessageBox.Show("Vui lòng nhập điểm trung bình hợp lệ.");
                    return;
                }
                Faculty selectedFaculty = cmbKhoa.SelectedItem as Faculty;
                if (selectedFaculty == null)
                {
                    MessageBox.Show("Vui lòng chọn khoa cho sinh viên.");
                    return;
                }
                Student newStudent = new Student
                {
                    StudentID = studentIdInt,
                    FullName = fullName,
                    AverageScore = averageScore,
                    FacultyID = selectedFaculty.FacultyID
                };
                studentService.AddStudent(newStudent);
                MessageBox.Show("Thêm sinh viên thành công.");
                BindGrid(studentService.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudent.Rows[e.RowIndex];
                txtMa.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                cmbKhoa.Text = row.Cells[2].Value.ToString();
                txtDiem.Text = row.Cells[3].Value.ToString();
            }    
        }
    }
}
