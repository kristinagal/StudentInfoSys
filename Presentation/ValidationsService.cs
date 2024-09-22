using StudentInfoSys.Database.Entities;
using StudentInfoSys.Interfaces;

namespace StudentInfoSys.Presentation
{
   
    public class ValidationsService : IValidationsService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ILectureRepository _lectureRepo;
        private readonly IDepartmentRepository _departmentRepo;

        public ValidationsService(IStudentRepository studentRepo, ILectureRepository lectureRepo, IDepartmentRepository departmentRepo)
        {
            _studentRepo = studentRepo;
            _lectureRepo = lectureRepo;
            _departmentRepo = departmentRepo;
        }

        // Papildomos validacijos:

        // paskaitos laikas ne daugiau nei 2 val
        // paskaitos nevyksta nakti
        // paskaitos nevyksta pietu pertraukos metu

        #region Student validations
        public Result GetExistingStudentNumber(string userInput) // does it exist in the database
        {
            if (int.TryParse(userInput, out int studentNumber))
            {
                var student = _studentRepo.GetStudentByNumber(studentNumber);
                if (student != null)
                {
                    return new Result(true);
                }
                return new Result(false, "Student not found. Please enter a valid student number.");
            }
            return new Result(false, "Invalid input! Please enter a valid student number.");
        }

        public Result GetValidName(string userInput) // 2-50 letters
        {
            if (userInput.Length >= 2 && userInput.Length <= 50 && userInput.All(char.IsLetter))
            {
                return new Result(true);
            }
            return new Result(false, "Name must be between 2 and 50 letters.");
        }

        public Result GetValidEmail(string? userInput) // text @ text . text
        {
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                int atIndex = userInput.IndexOf('@');
                int dotIndex = userInput.LastIndexOf('.');

                if (atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < userInput.Length - 1)
                {
                    if (_studentRepo.GetStudentByEmail(userInput) == null)
                    {
                        return new Result(true);
                    }
                    return new Result(false, "This email already exists. Please enter a unique email.");

                }
            }          
            return new Result(false, "Email format is invalid.");
        }

        public Result GetValidStudentNumber(string userInput) // 8 digits, unique
        {
            if (int.TryParse(userInput, out int studentNumber) && userInput.Length == 8)
            {
                if (_studentRepo.GetStudentByNumber(studentNumber) == null)
                {
                    return new Result(true);
                }
                return new Result(false, "This student number already exists. Please enter a unique number.");
            }
            return new Result(false, "Student number must be exactly 8 digits.");
        }
        #endregion

        #region Department validations
        public Result GetValidDepartmentName(string userInput) // 3-100 characters, letters and numbers only
        {
            if (userInput.Length >= 3 && userInput.Length <= 100 && userInput.All(char.IsLetterOrDigit))
            {
                return new Result(true);
            }
            return new Result(false, "Department name must be between 3 and 100 characters (letters and numbers only).");
        }

        public Result GetExistingDepartmentCode(string userInput) // does it exist in the database
        {
            var department = _departmentRepo.GetDepartmentByCode(userInput);
            if (department != null)
            {
                return new Result(true);
            }
            return new Result(false, "Department not found. Please enter a valid department code.");
        }

        public Result GetValidDepartmentCode(string userInput) // for getting new code - exactly 6 digits, unique
        {
            if (userInput.Length == 6 && userInput.All(char.IsLetterOrDigit))
            {
                if (_departmentRepo.GetDepartmentByCode(userInput) == null)
                {
                    return new Result(true);
                }
                return new Result(false, "This department code already exists. Please enter a unique code.");
            }
            return new Result(false, "Department code must be exactly 6 digits and contain only letters and numbers.");
        }
        #endregion

        #region Lecture validations
        public Result GetExistingLectureId(string userInput) // does it exist in the database
        {
            if (int.TryParse(userInput, out int lectureId))
            {
                var lecture = _lectureRepo.GetLectureById(lectureId);
                if (lecture != null)
                {
                    return new Result(true);
                }
                return new Result(false, "Lecture not found. Please enter a valid lecture ID.");
            }
            return new Result(false, "Invalid input! Please enter a valid lecture ID.");
        }

        public Result GetValidLectureName(string userInput)  // min 5 characters
        {
            if (userInput.Length >= 5)
            {
                var lecture = _lectureRepo.GetLectureByName(userInput);
                if (lecture == null)
                {
                    return new Result(true);
                }
            }

            return new Result(false, "Lecture name must be unique and at least 5 characters long.");
        }

        public Result GetValidLectureStartTime(string userInput) // no night lectures, no lectures during lunch
        {
            TimeSpan lunchStart = new TimeSpan(11, 30, 0);
            TimeSpan lunchEnd = new TimeSpan(12, 0, 0);
            TimeSpan earliestStartTime = new TimeSpan(8, 0, 0);
            TimeSpan latestStartTime = new TimeSpan(17, 59, 0);

            if (TimeSpan.TryParse(userInput, out TimeSpan lectureStartTime))
            {
                if (lectureStartTime >= earliestStartTime
                    && lectureStartTime <= latestStartTime
                    && (lectureStartTime < lunchStart || lectureStartTime >= lunchEnd))
                {
                    return new Result(true);
                }
                return new Result(false, "Invalid time! Lecture must be between 08:00-18:00, and should not overlap with lunch (11:30-12:00).");
            }
            return new Result(false, "Invalid time format! Please enter a valid time (hh:mm).");
        }

        public Result GetValidLectureEndTime(string userInput, TimeSpan startTime) // lecture duration <= 2:00, no night lectures, no lectures during lunch
        {
            TimeSpan lunchStart = new TimeSpan(11, 30, 0);
            TimeSpan lunchEnd = new TimeSpan(12, 0, 0);
            TimeSpan earliestEndTime = new TimeSpan(8, 1, 0);
            TimeSpan latestEndTime = new TimeSpan(18, 0, 0);
            TimeSpan maxLectureDuration = new TimeSpan(2, 0, 0); // 2 hours

            if (TimeSpan.TryParse(userInput, out TimeSpan lectureEndTime))
            {
                TimeSpan lectureDuration = lectureEndTime - startTime;      
                bool validTimeInterval = startTime < lectureEndTime;
                bool lunchOverlap = startTime < lunchEnd && lectureEndTime > lunchStart;

                if (lectureEndTime >= earliestEndTime
                    && lectureEndTime <= latestEndTime
                    && !lunchOverlap
                    && validTimeInterval
                    && lectureDuration <= maxLectureDuration)
                {
                    return new Result(true);
                }
                return new Result(false, "Invalid time! Lecture must be between 08:00-18:00, duration less than 2 hours and should not overlap with lunch (11:30-12:00).");
            }
            return new Result(false, "Invalid time format! Please enter a valid time (hh:mm).");
        }

        public Result GetValidLectureWeekday(string? userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return new Result(true, "Lecture will be scheduled every weekday (Monday to Friday).");
            }

            var validWeekdays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            var enteredWeekday = userInput.ToLower();

            if (validWeekdays.Any(day => day.ToLower() == enteredWeekday))
            {
                return new Result(true);
            }

            return new Result(false, "Invalid weekday. Lecture can only be scheduled from Monday to Friday, or leave it blank to schedule every workday.");
        }

        #endregion

        #region Additional helper methods

        public Result IsStudentInLectureDepartment(Student student, Lecture lecture)
        {
            if (!lecture.Departments.Any(d => d.DepartmentCode == student.DepartmentCode))
            {
                return new Result(false, "Student cannot be assigned to this lecture as it doesn't belong to their department.");
            }
            return new Result(true, "Student can be assigned to this lecture.");
        }

        public Result CheckStudentLectureConflicts(int studentNumber, TimeSpan startTime, TimeSpan endTime, string? weekday)
        {
            var student = _studentRepo.GetStudentByNumber(studentNumber);

            if (student != null)
            {
                var conflictingLecture = student.Lectures
                    .FirstOrDefault(l =>
                        // Check if the lecture times overlap
                        ((startTime >= l.LectureStartTime && startTime < l.LectureEndTime) ||
                         (endTime > l.LectureStartTime && endTime <= l.LectureEndTime)) &&
                        // Check if the weekdays are the same, or if one of them is null (meaning it occurs every weekday)
                        (string.IsNullOrEmpty(weekday) || string.IsNullOrEmpty(l.Weekday) || l.Weekday == weekday)
                    );

                if (conflictingLecture != null)
                {
                    return new Result(false, $"Warning: Student already has a lecture at this time.");
                }
            }

            return new Result(true, "No conflicting lecture found.");
        }

        #endregion

    }
}
