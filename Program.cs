using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
//using Newtonsoft.Json;

namespace StudentApiClient
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7207/api/StudentsAPI"); // Set this to the correct URI for your API

            await DeleteStudent(18);
            //await AddNewStudent(ReadStudentInfo());
            await GetAllStudents();
            //Console.WriteLine("\n\n Passed students : ");
            //await GetPassedStudents();
            //await GetAverageGrades();
            //await GetStudentByID();
        }


        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching passed students...\n");
                var students = await httpClient.GetFromJsonAsync<List<Student>>("StudentsAPI/Passed");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task GetAllStudents()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching all students...\n");
                var students = await httpClient.GetFromJsonAsync<List<Student>>("StudentsAPI/All");
                if (students != null)
                {
                    foreach (var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task GetAverageGrades()
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine("\nFetching average grade...\n");
                var averageGrade = await httpClient.GetFromJsonAsync<float>("StudentsAPI/AverageGrades");
                Console.WriteLine($"Average Grade: {averageGrade}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }


        }
        
        static async Task GetStudentByID()
        {
            Console.WriteLine("Student ID : ");
            int id;
            id = Convert.ToInt32(Console.ReadLine());

            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nFetching student with ID {id}...\n");

                var response = await httpClient.GetAsync($"StudentsAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<Student>();
                    if (student != null)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Grade: {student.Grade}");
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static Student ReadStudentInfo()
        {
            Student s = new Student();
            Console.WriteLine("ID :");
            s.Id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Name :");
            s.Name = Console.ReadLine();
            Console.WriteLine("Age :");
            s.Age = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Grade :");
            s.Grade = Convert.ToInt32(Console.ReadLine());

            return s;
        }
        static async Task AddNewStudent(Student NewStudent)
        {
            try
            {
                Console.WriteLine("_________________________________");
                Console.WriteLine("Add New Student ...");
                var response = await httpClient.PostAsJsonAsync("" , NewStudent);
                if (response.IsSuccessStatusCode)
                {
                    var addedStudent = await response.Content.ReadFromJsonAsync<Student>();
                    Console.WriteLine($"Added Student - ID: {addedStudent.Id}, Name: {addedStudent.Name}, Age: {addedStudent.Age}, Grade: {addedStudent.Grade}");

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) 
                {
                    Console.WriteLine("Bad request , invalid inputs");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task DeleteStudent(int id)
        {
            try
            {
                Console.WriteLine("\n_____________________________");
                Console.WriteLine($"\nDeleting student with ID {id}...\n");
                var response = await httpClient.DeleteAsync($"StudentsAPI/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Student with ID {id} has been deleted.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    Console.WriteLine($"Bad Request: Not accepted ID {id}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Not Found: Student with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }

    

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }


    }
}
