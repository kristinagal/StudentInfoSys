using Microsoft.EntityFrameworkCore;
using StudentInfoSys.Database.Entities;
using StudentInfoSys.Interfaces;
using StudentInfoSys.Presentation;

namespace StudentInfoSys.UserInterface
{
    public class BusinessLogicService : IBusinessLogicService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ILectureRepository _lectureRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IValidationsService _validationsService;

        public BusinessLogicService(IStudentRepository studentRepo, ILectureRepository lectureRepo, IDepartmentRepository departmentRepo, IValidationsService validationsService)
        {
            _studentRepo = studentRepo;
            _lectureRepo = lectureRepo;
            _departmentRepo = departmentRepo;
            _validationsService = validationsService;
        }

        // This method handles all user inputs
        // Func<string, Result> validationMethod - method from ValidationsService with structure "public Result MethodName(string userInput)"
        // Result - model class with properties Success and Message.
        public string? GetValidUserInput(Func<string, Result> validationMethod, string prompt) 
        // prints out prompt and asks for input until valid answer is received
        {
            while (true)
            {
                Console.Write(prompt);
                var userInput = Console.ReadLine().Trim();
                var result = validationMethod(userInput);
                if (result.Success)
                {
                    return userInput;
                }
                Console.WriteLine(result.Message);
            }
        }

        // Method to create a student
        public void CreateStudent()
        {
            Console.Clear();
            Console.WriteLine("---> Create student");
            Console.WriteLine();
            Console.WriteLine("To create new student please enter");

            // getting all needed values
            var firstName = GetValidUserInput(_validationsService.GetValidName, "First name: ");
            var lastName = GetValidUserInput(_validationsService.GetValidName, "Last name: ");
            var studentNumber = GetValidUserInput(_validationsService.GetValidStudentNumber, "Student number: ");
            var email = GetValidUserInput(_validationsService.GetValidEmail, "Email: ");
            var departmentCode = GetValidUserInput(_validationsService.GetExistingDepartmentCode, "Department code: ");

            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                StudentNumber = int.Parse(studentNumber),
                Email = email,
                DepartmentCode = departmentCode
            };

            _studentRepo.AddStudent(student);
            Console.WriteLine("Student created successfully.");
            Console.ReadKey();
        }

        // Method to change student's department
        public void ChangeStudentDepartment()
        {
            Console.Clear();
            Console.WriteLine("---> Change department");
            Console.WriteLine();
            Console.WriteLine("To change student's department please enter");
            var studentNumber = GetValidUserInput(_validationsService.GetValidStudentNumber, "Student number: ");
            var student = _studentRepo.GetStudentByNumber(int.Parse(studentNumber));

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            var newDepartmentCode = GetValidUserInput(_validationsService.GetExistingDepartmentCode, "New department code: ");
            student.DepartmentCode = newDepartmentCode;
            _studentRepo.UpdateStudent(student);

            // Reassign lectures based on the new department
            _studentRepo.ReassignLecturesByDepartment(student);

            Console.WriteLine("Student's department and lectures changed successfully.");
            Console.ReadKey();
        }

        // Method to add a lecture to a student
        public void AddLectureToStudent()
        {
            Console.Clear();
            Console.WriteLine("---> Add lecture");
            Console.WriteLine();
            Console.WriteLine("To add lecture to student please enter");

            var studentNumber = GetValidUserInput(_validationsService.GetValidStudentNumber, "Student number: ");
            var student = _studentRepo.GetStudentByNumber(int.Parse(studentNumber));

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            var lectureId = GetValidUserInput(_validationsService.GetExistingLectureId, "Lecture Id: ");
            var lecture = _lectureRepo.GetLectureById(int.Parse(lectureId));

            if (lecture == null)
            {
                Console.WriteLine("Lecture not found.");
                return;
            }

            // Call the department check method before assigning the lecture
            var departmentResult = _validationsService.IsStudentInLectureDepartment(student, lecture);
            if (!departmentResult.Success)
            {
                Console.WriteLine(departmentResult.Message);
                return;
            }

            // Call the conflict check before adding the lecture
            var conflictResult = _validationsService.CheckStudentLectureConflicts(student.StudentNumber, lecture.LectureStartTime, lecture.LectureEndTime, lecture.Weekday);
            if (!conflictResult.Success)
            {
                Console.WriteLine(conflictResult.Message);
                return;
            }

            // If both validations pass, add the lecture to the student and save the changes
            _studentRepo.AddLectureToStudent(student.StudentNumber, lecture);

            Console.WriteLine("Lecture added to student successfully.");
            Console.ReadKey();
        }


        // Method to remove a lecture from a student
        public void RemoveLectureFromStudent()
        {
            Console.Clear();
            Console.WriteLine("---> Remove lecture");
            Console.WriteLine();
            Console.WriteLine("To remove lecture from student please enter");
            var studentNumber = GetValidUserInput(_validationsService.GetExistingStudentNumber, "Student number: ");
            var student = _studentRepo.GetStudentByNumber(int.Parse(studentNumber));

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            var lectureId = GetValidUserInput(_validationsService.GetExistingLectureId, "Lecture Id: ");
            var lecture = student.Lectures.FirstOrDefault(l => l.LectureId == int.Parse(lectureId));

            if (lecture == null)
            {
                Console.WriteLine("Lecture not found for this student.");
                return;
            }

            student.Lectures.Remove(lecture);
            _studentRepo.UpdateStudent(student);
            Console.WriteLine("Lecture removed from student successfully.");
            Console.ReadKey();
        }

        // Method to display all lectures for a student, including time and weekday
        public void ShowStudentLectures()
        {
            Console.Clear();
            Console.WriteLine("---> Show student's lectures");
            Console.WriteLine();

            var studentNumber = GetValidUserInput(_validationsService.GetExistingStudentNumber, "Student number: ");
            var student = _studentRepo.GetStudentByNumber(int.Parse(studentNumber));

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            if (student.Lectures == null || student.Lectures.Count == 0)
            {
                Console.WriteLine("This student has no assigned lectures.");
                return;
            }

            Console.WriteLine($"Lectures for {student.FirstName} {student.LastName}:");
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Lecture Name | Start Time - End Time | Weekday");
            Console.WriteLine("-----------------------------------------------------");

            foreach (var lecture in student.Lectures)
            {
                var lectureTime = $"{lecture.LectureStartTime:hh\\:mm} - {lecture.LectureEndTime:hh\\:mm}";
                var lectureDay = string.IsNullOrEmpty(lecture.Weekday) ? "Every weekday" : lecture.Weekday;
                Console.WriteLine($"{lecture.LectureName,-15} | {lectureTime,-17} | {lectureDay}");
            }

            Console.ReadKey();
        }


        // Method to create a lecture
        public void CreateLecture()
        {
            Console.Clear();
            Console.WriteLine("---> Create lecture");
            Console.WriteLine();
            Console.WriteLine("To create new lecture please enter");

            var lectureName = GetValidUserInput(_validationsService.GetValidLectureName, "Lecture name: ");
            var startTime = GetValidUserInput(_validationsService.GetValidLectureStartTime, "Start time (hh:mm): ");
            string? endTime = null;
            Result result;
            do
            {
                Console.Write("End time (hh:mm): ");
                var userInput = Console.ReadLine().Trim();
                result = _validationsService.GetValidLectureEndTime(userInput, TimeSpan.Parse(startTime));
                if (result.Success)
                    endTime = userInput;
                if (!result.Success)
                    Console.WriteLine(result.Message);
            }
            while (!result.Success);

            var weekday = GetValidUserInput(_validationsService.GetValidLectureWeekday, "Weekday (Monday to Friday or blank): ");

            var departmentCode = GetValidUserInput(_validationsService.GetExistingDepartmentCode, "Department code: ");
            var department = _departmentRepo.GetDepartmentByCode(departmentCode);

            Lecture lecture = new Lecture
            {
                LectureName = lectureName,
                LectureStartTime = TimeSpan.Parse(startTime),
                LectureEndTime = TimeSpan.Parse(endTime),
                Weekday = char.ToUpper(weekday[0]) + weekday.Substring(1).ToLower(),
                Departments = new List<Department> { department }
            };

            _lectureRepo.AddLecture(lecture);

            // Automatically reassign lectures for all students in the department
            _studentRepo.ReassignLecturesForAllStudents();

            Console.WriteLine($"Lecture '{lectureName}' created successfully and added to department '{department.DepartmentName}'.");
            Console.ReadKey();
        }

        // Method to change a lecture's department
        public void ChangeLectureDepartment()
        {
            Console.Clear();
            Console.WriteLine("---> Change department");
            Console.WriteLine();
            Console.WriteLine("To change lecture's department please enter");
            var lectureId = GetValidUserInput(_validationsService.GetExistingLectureId, "Lecture Id: ");
            var lecture = _lectureRepo.GetLectureById(int.Parse(lectureId));

            if (lecture == null)
            {
                Console.WriteLine("Lecture not found.");
                return;
            }

            var departmentCode = GetValidUserInput(_validationsService.GetExistingDepartmentCode, "New department code: ");
            var department = _departmentRepo.GetDepartmentByCode(departmentCode);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            lecture.Departments.Clear();
            _lectureRepo.ChangeLectureDepartment(int.Parse(lectureId), departmentCode);

            Console.WriteLine("Lecture department updated successfully.");
            Console.ReadKey();
        }

        // Method to create a department
        public void CreateDepartment()
        {
            Console.Clear();
            Console.WriteLine("---> Create department");
            Console.WriteLine();
            Console.WriteLine("To create new department please enter");
            var departmentCode = GetValidUserInput(_validationsService.GetValidDepartmentCode, "Department code: ");
            var departmentName = GetValidUserInput(_validationsService.GetValidDepartmentName, "Department name: ");

            var department = new Department
            {
                DepartmentCode = departmentCode,
                DepartmentName = departmentName
            };

            _departmentRepo.AddDepartment(department);
            Console.WriteLine("Department created successfully.");
            Console.ReadKey();
        }

        // Method to show all students in a department
        public void ShowDepartmentStudents()
        {
            Console.Clear();
            Console.WriteLine("---> Show department’s students");
            Console.WriteLine();
            var departmentCode = GetValidUserInput(_validationsService.GetExistingDepartmentCode, "Department code: ");
            var department = _departmentRepo.GetDepartmentByCode(departmentCode);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            Console.WriteLine($"Students for {department.DepartmentName}:");
            foreach (var student in department.Students)
            {
                Console.WriteLine($"- {student.FirstName} {student.LastName}");
            }
            Console.ReadKey();
        }

        // Method to display all lectures for a department
        public void ShowDepartmentLectures()
        {
            Console.Clear();
            Console.WriteLine("---> Show department’s lectures");
            Console.WriteLine();
            var departmentCode = GetValidUserInput(_validationsService.GetExistingDepartmentCode, "Department code: ");
            var department = _departmentRepo.GetDepartmentByCode(departmentCode);

            if (department == null)
            {
                Console.WriteLine("Department not found.");
                return;
            }

            Console.WriteLine($"Lectures for {department.DepartmentName}:");
            foreach (var lecture in department.Lectures)
            {
                Console.WriteLine($"- {lecture.LectureName} ({lecture.LectureStartTime} - {lecture.LectureEndTime})");
            }
            Console.ReadKey();
        }

        

    }
}