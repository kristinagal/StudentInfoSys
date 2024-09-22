using StudentInfoSys.Interfaces;

namespace StudentInfoSys.UserInterface
{
    public class MenuService : IMenuService
    {
        private readonly IBusinessLogicService _businessLogicService;

        public MenuService(IBusinessLogicService businessLogicService)
        {
            _businessLogicService = businessLogicService;
        }


        /*
        This is how menu looks:

        1. Student			1.1. Create student(= add student to department)
					        1.2. Change department
					        1.3. Add lecture
					        1.4. Remove lecture
					        1.5. Show student's lectures
		
        2. Lecture			2.1. Create lecture(= add lecture to department)
					        2.2. Change department
					        2.3. Show student's lectures
					        2.4. Show department's lectures
				
        3. Department       3.1. Create department
					        3.2. Show department's students
					        3.3. Show department's lectures
        4. Exit	
        
        */

        public void MainMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Welcome to StudentInfoSys!");
                Console.WriteLine();
                Console.WriteLine("1. Student");
                Console.WriteLine("2. Lecture");
                Console.WriteLine("3. Department");
                Console.WriteLine("4. Exit");
                Console.WriteLine();
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowStudentMenu();
                        break;
                    case "2":
                        ShowLectureMenu();
                        break;
                    case "3":
                        ShowDepartmentMenu();
                        break;
                    case "4":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }

        // Student Menu
        private void ShowStudentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---> Student");
                Console.WriteLine();
                Console.WriteLine("1. Create student");
                Console.WriteLine("2. Change department");
                Console.WriteLine("3. Add lecture");
                Console.WriteLine("4. Remove lecture");
                Console.WriteLine("5. Show student's lectures");
                Console.WriteLine();
                Console.WriteLine("Enter 0 to go back to the main menu.");
                Console.WriteLine();
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        _businessLogicService.CreateStudent();
                        break;
                    case "2":
                        _businessLogicService.ChangeStudentDepartment();
                        break;
                    case "3":
                        _businessLogicService.AddLectureToStudent();
                        break;
                    case "4":
                        _businessLogicService.RemoveLectureFromStudent();
                        break;
                    case "5":
                        _businessLogicService.ShowStudentLectures();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }

            }
        }

        // Lecture Menu
        private void ShowLectureMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---> Lecture");
                Console.WriteLine();
                Console.WriteLine("1. Create lecture");
                Console.WriteLine("2. Change department for lecture");
                Console.WriteLine("3. Show student’s lectures");
                Console.WriteLine("4. Show department’s lectures");
                Console.WriteLine();
                Console.WriteLine("Enter 0 to go back to the main menu.");
                Console.WriteLine();
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        _businessLogicService.CreateLecture();
                        break;
                    case "2":
                        _businessLogicService.ChangeLectureDepartment();
                        break;
                    case "3":
                        _businessLogicService.ShowStudentLectures();
                        break;
                    case "4":
                        _businessLogicService.ShowDepartmentLectures();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }

        // Department Menu
        private void ShowDepartmentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---> Department");
                Console.WriteLine();
                Console.WriteLine("1. Create department");
                Console.WriteLine("2. Show department’s students");
                Console.WriteLine("3. Show department’s lectures");
                Console.WriteLine();
                Console.WriteLine("Enter 0 to go back to the main menu.");
                Console.WriteLine();
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        _businessLogicService.CreateDepartment();
                        break;
                    case "2":
                        _businessLogicService.ShowDepartmentStudents();
                        break;
                    case "3":
                        _businessLogicService.ShowDepartmentLectures();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }

            }
        }
    }
}
