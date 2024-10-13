using DAL4.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL4
{

    public class StudentDAL
    {
        private QuanLySinhVienModel db;
        public StudentDAL()
        {
            db = new QuanLySinhVienModel();
        }

        public List<Student> GetAllStudents()
        {
            return db.Student.Include("Faculty").Include("Major").ToList();
        }
        public List<Student> GetAllStudentsWithNoMajor()
        {
            return db.Student.Include("Faculty").Include("Major").Where(s => s.MajorID == null).ToList();
        }
        public List<Faculty> GetAllFaculties()
        {
            return db.Faculty.ToList();
        }

        public List<Major> GetAllMajors()
        {
            return db.Major.ToList();
        }
        public Student GetStudentById(string studentID)
        {
            return db.Student.SingleOrDefault(s => s.StudentID == studentID);
        }

        public void AddStudent(Student student)
        {
            db.Student.Add(student);
            db.SaveChanges();
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = db.Student.SingleOrDefault(s => s.StudentID == student.StudentID);
            if (existingStudent != null)
            {
                existingStudent.FullName = student.FullName;
                existingStudent.AverageScore = student.AverageScore;
                existingStudent.FacultyID = student.FacultyID;
                existingStudent.MajorID = student.MajorID;
                existingStudent.Avatar = student.Avatar;
                db.SaveChanges();
            }
        }
        public void DeleteStudent(string studentID)
        {
            var student = db.Student.SingleOrDefault(s => s.StudentID == studentID);
            if (student != null)
            {
                db.Student.Remove(student);
                db.SaveChanges();
            }
        }
    }
}
