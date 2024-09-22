using StudentInfoSys.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Interfaces
{
    public interface IBusinessLogicService
    {
        void AddLectureToStudent();
        void ChangeLectureDepartment();
        void ChangeStudentDepartment();
        void CreateDepartment();
        void CreateLecture();
        void CreateStudent();
        string GetValidUserInput(Func<string, Result> validationMethod, string prompt);
        void RemoveLectureFromStudent();
        void ShowDepartmentLectures();
        void ShowDepartmentStudents();
        void ShowStudentLectures();
    }
}
