using StudentInfoSys.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentInfoSys.Database.Entities
{
    public class Lecture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LectureId { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "LectureName must be at least 5 letters or digits.")]
        public required string LectureName { get; set; }

        public required TimeSpan LectureStartTime { get; set; }

        public required TimeSpan LectureEndTime { get; set; } = new TimeSpan(1, 30, 0); // Fiksuota paskaitos trukmė - 1:30

        public string? Weekday { get; set; } // jei NULL, tada kasdien

        public required ICollection<Department> Departments { get; set; }
        public ICollection<Student>? Students { get; set; }
    }
}
