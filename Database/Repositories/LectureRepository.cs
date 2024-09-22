﻿using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Database.Entities;
using StudentInfoSys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Database.Repositories
{
    public class LectureRepository : ILectureRepository
    {
        private readonly StudentContext _context;

        public LectureRepository(StudentContext context)
        {
            _context = context;
        }
        public int AddLecture(Lecture lecture)
        {
            _context.Lectures.Add(lecture);
            _context.SaveChanges();
            return lecture.LectureId; // Return autogenerated LectureId
        }
        public List<Lecture> GetAllLectures()
        {
            return _context.Lectures
                .Include(l => l.Departments)
                .Include(l => l.Students)
                .ToList();
        }
        public Lecture? GetLectureById(int lectureId)
        {
            return _context.Lectures
                .Include(l => l.Departments)
                .Include(l => l.Students)
                .FirstOrDefault(l => l.LectureId == lectureId);
        }
        public Lecture? GetLectureByName(string lectureName)
        {
            return _context.Lectures
                .Include(l => l.Departments)
                .Include(l => l.Students)
                .FirstOrDefault(l => l.LectureName == lectureName);
        }
        public void ChangeLectureDepartment(int lectureId, string departmentCode)
        {
            var lecture = GetLectureById(lectureId);
            var department = _context.Departments.FirstOrDefault(d => d.DepartmentCode == departmentCode);
            if (lecture != null && department != null)
            {
                department.Lectures.Add(lecture);
                _context.SaveChanges();
            }
        }

        // these crud operations are not used currently

        //public void UpdateLecture(Lecture lecture)
        //{
        //    _context.Lectures.Update(lecture);
        //    _context.SaveChanges();
        //}

        //public void DeleteLecture(int lectureId)
        //{
        //    var lecture = GetLectureById(lectureId);
        //    if (lecture != null)
        //    {
        //        _context.Lectures.Remove(lecture);
        //        _context.SaveChanges();
        //    }
        //}

    }
}
