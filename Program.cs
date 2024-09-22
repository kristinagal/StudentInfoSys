using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using StudentInfoSys.Database;
using StudentInfoSys.Database.Repositories;
using StudentInfoSys.Interfaces;
using StudentInfoSys.Presentation;
using StudentInfoSys.UserInterface;


namespace StudentInfoSys
{
    public class Program
    {
        static void Main(string[] args)
        {
            /*
            Add-Migration InitialCreate
            Update-Database
            */

            using var context = new StudentContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //// Execute this part once after DB creation:
            //// Manually seed lectures here and associate relationships after Lecture IDs are generated
            //var lectures = CsvHelperService.GetLecturesFromInitialData();
            //context.Lectures.AddRange(lectures);
            //context.SaveChanges(); // Save to get generated IDs
            //SeedRelationships(context);
            //Console.WriteLine("Database created, initial data loaded");


            // Initialize repositories
            ILectureRepository lectureRepo = new LectureRepository(context);
            IDepartmentRepository departmentRepo = new DepartmentRepository(context);
            IStudentRepository studentRepo = new StudentRepository(context, lectureRepo, departmentRepo);

            // Assign lectures to students according to department
            studentRepo.ReassignLecturesForAllStudents();

            // Initialize validations and business logic services
            IValidationsService validationsService = new ValidationsService(studentRepo, lectureRepo, departmentRepo);
            IBusinessLogicService businessLogicService = new BusinessLogicService(studentRepo, lectureRepo, departmentRepo, validationsService);

            // Create MenuService and start the menu loop
            var menuService = new MenuService(businessLogicService);
            menuService.MainMenu();

        }

        public static void SeedRelationships(StudentContext context)
        {
            // Associate students with lectures
            CsvHelperService.GetStudentLecturesFromInitialData(context);

            // Associate departments with lectures
            CsvHelperService.GetDepartmentLecturesFromInitialData(context);

            // Save changes
            context.SaveChanges();
        }
    }
}
