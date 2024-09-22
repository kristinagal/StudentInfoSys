using StudentInfoSys.Database.Entities;
using StudentInfoSys.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Interfaces
{
    public interface IStudentRepository
    {
        void AddLectureToStudent(int studentNumber, Lecture lecture);
        void AddStudent(Student student);     
        List<Student> GetAllStudents();
        Student? GetStudentByNumber(int studentNumber);
        Student? GetStudentByEmail(string email);     
        void UpdateStudent(Student student);
        void ReassignLecturesByDepartment(Student student);
        void ReassignLecturesForAllStudents();

        //void DeleteStudent(int studentNumber);
        //void RemoveLectureFromStudent(int studentNumber, int lectureId);

    }
}
