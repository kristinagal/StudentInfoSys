using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Database.Entities;
using StudentInfoSys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Database.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly StudentContext _context;

        public DepartmentRepository(StudentContext context)
        {
            _context = context;
        }

        public void AddDepartment(Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }


        public Department? GetDepartmentByCode(string departmentCode)
        {
            return _context.Departments
                .Include(d => d.Lectures)
                .Include(d => d.Students)
                .FirstOrDefault(d => d.DepartmentCode == departmentCode);
        }

        // these crud operations are not used currently

        //public List<Department> GetAllDepartments()
        //{
        //    return _context.Departments
        //        .Include(d => d.Lectures)
        //        .Include(d => d.Students)
        //        .ToList();
        //}

        //public void UpdateDepartment(Department department)
        //{
        //    _context.Departments.Update(department);
        //    _context.SaveChanges();
        //}

        //public void DeleteDepartment(string departmentCode)
        //{
        //    var department = GetDepartmentByCode(departmentCode);
        //    if (department != null)
        //    {
        //        _context.Departments.Remove(department);
        //        _context.SaveChanges();
        //    }
        //}

        //public void AddLectureToDepartment(string departmentCode, Lecture lecture)
        //{
        //    var department = GetDepartmentByCode(departmentCode);
        //    if (department != null)
        //    {
        //        department.Lectures.Add(lecture);
        //        _context.SaveChanges();
        //    }
        //}
    }
}

