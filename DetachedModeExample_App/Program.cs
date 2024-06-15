using DetachedModeExample_App.Model;
using DetachedModeExample_App.Rdb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetachedModeExample_App
{
    internal class Program
    {
        // сценарий получения всех записей
        static void Test_IStudentRepository_GetAll(IStudentsRepository repository)
        {
            Console.WriteLine("All students: ");
            foreach (Student s in repository.GetAll())
            {
                Console.WriteLine(s);
            }
        }

        // сценарий получения записи по id
        static void Test_IStudentRepository_Get(IStudentsRepository repository)
        {
            Console.Write("Enter id for get: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Student student = repository.Get(id);
            Console.WriteLine(student);
        }

        // сценарий добавления записи
        static void Test_IStudentRepository_Add(IStudentsRepository repository)
        {
            Console.Write("Enter lastname: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter firstname: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter rate: ");
            int rate = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter grants (press enter to set null): ");
            string grantsStr = Console.ReadLine();
            decimal? grants = null;
            if (grantsStr != null && grantsStr != "")
            {
                grants = Convert.ToDecimal(grantsStr);
            }
            Student newStudent = new Student(lastName, firstName, rate, grants);
            repository.Add(newStudent);
            Console.WriteLine("New student added");
        }

        // сценарий удаления записи
        static void Test_IStudentRepository_Delete(IStudentsRepository repository)
        {
            Console.Write("Enter id for delete: ");
            int id = Convert.ToInt32(Console.ReadLine());
            repository.Delete(id);
            Console.WriteLine("Student deleted");
        }

        // сценарий обновления записи
        static void Test_IStudentRepository_Update(IStudentsRepository repository)
        {
            Console.Write("Enter id for update: ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter lastname: ");
            string lastName = Console.ReadLine();
            Console.Write("Enter firstname: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter rate: ");
            int rate = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter grants (press enter to set null): ");
            string grantsStr = Console.ReadLine();
            decimal? grants = null;
            if (grantsStr != null && grantsStr != "")
            {
                grants = Convert.ToDecimal(grantsStr);
            }
            Student newStudent = new Student(id, lastName, firstName, rate, grants);
            repository.Update(newStudent);
            Console.WriteLine("Student updated");
        }

        static void Main(string[] args)
        {
            // using (IStudentsRepository repository = new StudentsRepositoryStub())
            using (IDetachedStudentsRepository repository = new RdbDetachedStudentsRepository())
            {
                while (true)
                {
                    Console.WriteLine("1. Pull");
                    Console.WriteLine("2. Push");
                    Console.WriteLine("3. GetAll");
                    Console.WriteLine("4. Get");
                    Console.WriteLine("5. Add");
                    Console.WriteLine("6. Delete");
                    Console.WriteLine("7. Update");
                    Console.Write("Enter choice: ");
                    string choice = Console.ReadLine();
                    // выполним операцию
                    switch (choice)
                    {
                        case "1":
                            repository.Pull();
                            Console.WriteLine("Pull OK");
                            break;
                        case "2":
                            repository.Push();
                            Console.WriteLine("Push OK");
                            break;
                        case "3":
                            Test_IStudentRepository_GetAll(repository);
                            break;
                        case "4":
                            Test_IStudentRepository_Get(repository);
                            break;
                        case "5":
                            Test_IStudentRepository_Add(repository);
                            break;
                        case "6":
                            Test_IStudentRepository_Delete(repository);
                            break;
                        case "7":
                            Test_IStudentRepository_Update(repository);
                            break;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
            }
        }
    }
}
