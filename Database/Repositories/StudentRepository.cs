using StudentInfoSys.Database.Entities;
using StudentInfoSys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudentInfoSys.Database.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;
        private readonly ILectureRepository _lectureRepo;
        private readonly IDepartmentRepository _departmentRepo;

        public StudentRepository(StudentContext context, ILectureRepository lectureRepo, IDepartmentRepository departmentRepo)
        {
            _context = context;
            _lectureRepo = lectureRepo;
            _departmentRepo = departmentRepo;
        }

        public void AddStudent(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public List<Student> GetAllStudents()
        {
            return _context.Students
                .Include(s => s.Department)
                .Include(s => s.Lectures)
                .ToList();
        }

        public Student? GetStudentByNumber(int studentNumber)
        {
            return _context.Students
                .Include(s => s.Department)
                .Include(s => s.Lectures)
                .FirstOrDefault(s => s.StudentNumber == studentNumber);
        }

        public Student? GetStudentByEmail(string email)
        {
            return _context.Students
                .Include(s => s.Department)
                .Include(s => s.Lectures)
                .FirstOrDefault(s => s.Email == email);
        }

        public void UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void AddLectureToStudent(int studentNumber, Lecture lecture)
        {
            var student = GetStudentByNumber(studentNumber);
            if (student != null)
            {
                student.Lectures.Add(lecture);
                _context.SaveChanges();
            }
        }

        public void ReassignLecturesByDepartment(Student student)
        {
            // Get the department
            var studentDepartment = _departmentRepo.GetDepartmentByCode(student.DepartmentCode);

            if (studentDepartment == null)
            {
                Console.WriteLine("Student's department not found.");
                return;
            }

            // Get all lectures in the student's department
            var lecturesInDepartment = _lectureRepo.GetAllLectures()
                .Where(l => l.Departments.Any(d => d.DepartmentCode == studentDepartment.DepartmentCode)).ToList();

            // Clear the student's current lectures and assign new ones
            student.Lectures.Clear();
            foreach (var lecture in lecturesInDepartment)
            {
                student.Lectures.Add(lecture);
            }

            UpdateStudent(student); // Save changes to the database
        }

        public void ReassignLecturesForAllStudents()
        {
            var students = GetAllStudents();

            foreach (var student in students)
            {
                var studentDepartment = _departmentRepo.GetDepartmentByCode(student.DepartmentCode);

                if (studentDepartment == null)
                {
                    Console.WriteLine($"Department for student {student.FirstName} {student.LastName} not found.");
                    continue;
                }

                // Get all lectures in the student's department
                var lecturesInDepartment = _lectureRepo.GetAllLectures()
                    .Where(l => l.Departments.Any(d => d.DepartmentCode == studentDepartment.DepartmentCode)).ToList();

                // Clear the student's current lectures and assign new ones
                student.Lectures.Clear();
                foreach (var lecture in lecturesInDepartment)
                {
                    student.Lectures.Add(lecture);
                }

                // Save changes after updating each student's lectures
                UpdateStudent(student);
            }
        }


        // these crud operations are not used currently

        //public void DeleteStudent(int studentNumber)
        //{
        //    var student = GetStudentByNumber(studentNumber);
        //    if (student != null)
        //    {
        //        _context.Students.Remove(student);
        //        _context.SaveChanges();
        //    }
        //}

        //public void RemoveLectureFromStudent(int studentNumber, int lectureId)
        //{
        //    var student = GetStudentByNumber(studentNumber);
        //    if (student != null)
        //    {
        //        var lecture = student.Lectures.FirstOrDefault(l => l.LectureId == lectureId);
        //        if (lecture != null)
        //        {
        //            student.Lectures.Remove(lecture);
        //            _context.SaveChanges();
        //        }
        //    }
        //}
    }
}
