using StudentInfoSys.Database.Entities;
using StudentInfoSys.Database.Repositories;
using StudentInfoSys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Interfaces
{
    public interface IDepartmentRepository
    {
        void AddDepartment(Department department);
        Department? GetDepartmentByCode(string departmentCode);

        //void AddLectureToDepartment(string departmentCode, Lecture lecture);
        //void DeleteDepartment(string departmentCode);
        //List<Department> GetAllDepartments();       
        //void UpdateDepartment(Department department);
    }
}
