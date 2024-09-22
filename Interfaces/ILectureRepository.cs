using StudentInfoSys.Database.Entities;
using StudentInfoSys.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Interfaces
{
    public interface ILectureRepository
    {
        int AddLecture(Lecture lecture);
        void ChangeLectureDepartment(int lectureId, string departmentCode);     
        List<Lecture> GetAllLectures();
        Lecture? GetLectureById(int lectureId);
        Lecture? GetLectureByName(string lectureName);

        //void DeleteLecture(int lectureId);
        //void UpdateLecture(Lecture lecture);
    }
}
