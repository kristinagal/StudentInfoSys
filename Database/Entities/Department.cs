using StudentInfoSys.Database.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentInfoSys.Database.Entities
{
    public class Department
    {
        [Key]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "DepartmentCode must be 6 letters or digits.")]
        public required string DepartmentCode { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "DepartmentName must be between 3 and 100 letters or digits.")]
        public required string DepartmentName { get; set; }

        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}