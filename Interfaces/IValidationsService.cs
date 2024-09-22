using StudentInfoSys.Database.Entities;
using StudentInfoSys.Presentation;

namespace StudentInfoSys.Interfaces
{
    public interface IValidationsService
    {
        Result GetValidDepartmentCode(string userInput);
        Result GetValidDepartmentName(string userInput);
        Result GetValidEmail(string userInput);
        Result GetExistingDepartmentCode(string userInput);
        Result GetExistingLectureId(string userInput);
        Result GetExistingStudentNumber(string userInput);
        Result GetValidLectureName(string userInput);
        Result GetValidLectureStartTime(string userInput);
        Result GetValidLectureEndTime(string userInput, TimeSpan startTime);
        Result GetValidLectureWeekday(string? userInput);
        Result GetValidName(string userInput);
        Result GetValidStudentNumber(string userInput);
        Result IsStudentInLectureDepartment(Student student, Lecture lecture);
        Result CheckStudentLectureConflicts(int studentNumber, TimeSpan startTime, TimeSpan endTime, string? weekday);
    }

}


