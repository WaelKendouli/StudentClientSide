using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;

namespace StudentApiClient
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("https://localhost:7207/api/StudentsAPI"); // Set this to the correct URI for your API

            //await GetAllStudents();
            //Console.WriteLine("\n\n Passed students : ");
            //await GetPassedStudents();
            //await GetAverageGrades();
            await GetStudentByID();
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

    }

    

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }
    }
}
