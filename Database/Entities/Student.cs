using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInfoSys.Database.Entities
{
    public class Student
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "FirstName must be between 2 and 50 letters.")]
        public required string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "LastName must be between 2 and 50 letters.")]
        public required string LastName { get; set; }

        [Key]
        [Range(00000000, 99999999, ErrorMessage = "StudentNumber must be exactly 8 digits.")]
        public required int StudentNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        public required string Email { get; set; }
        
        public Department? Department { get; set; } // Navigation property

        public required string DepartmentCode { get; set; } 

        public ICollection<Lecture> Lectures { get; set; } = new List<Lecture>(); // Initialize the collection
    }

}
