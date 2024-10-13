using DAL4.entities;
using DAL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS4
{
    public class StudentBUS
    {
        private StudentDAL studentDAL;

        public StudentBUS()
        {
            studentDAL = new StudentDAL();
        }

        public List<Student> GetAllStudents()
        {
            return studentDAL.GetAllStudents();
        }
        public List<Student> GetAllStudentsWithNoMajor()
        {
            return studentDAL.GetAllStudentsWithNoMajor();
        }
        public List<Faculty> GetAllFaculties()
        {
            return studentDAL.GetAllFaculties();
        }

        public List<Major> GetAllMajors()
        {
            return studentDAL.GetAllMajors();
        }
        public Student GetStudentById(string studentID)
        {
            return studentDAL.GetStudentById(studentID);
        }

        public void AddOrUpdateStudent(Student student)
        {
            var existingStudent = studentDAL.GetStudentById(student.StudentID);
            if (existingStudent == null)
            {
                studentDAL.AddStudent(student);
            }
            else
            {
                studentDAL.UpdateStudent(student);
            }
        }
        public void DeleteStudent(string studentID)
        {
            studentDAL.DeleteStudent(studentID);
        }
    }
}
