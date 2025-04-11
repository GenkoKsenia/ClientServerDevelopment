using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoursesClient
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Courses endpoints
        public async Task<List<Course>> GetCoursesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Courses");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Course>>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Courses/{id}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Course>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(course),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/Courses", content);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Course>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateCourseAsync(int id, Course course)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(course),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Courses/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCourseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Courses/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Students endpoints
        public async Task<List<Student>> GetStudentsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Students");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<Student>>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Students/{id}");
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Student>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(student),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/Students", content);
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Student>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task UpdateStudentAsync(int id, Student student)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(student),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}/api/Students/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteStudentAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Students/{id}");
            response.EnsureSuccessStatusCode();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            const string apiUrl = "https://localhost:7045"; 

            // Для обхода SSL ошибок в development
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (message, cert, chain, errors) => true
            };

            var client = new ApiClient(apiUrl);

            try
            {
                // Пример работы с курсами
                var newCourse = new Course
                {
                    Title = "Advanced .NET",
                    Description = "Курс повышения квалификации",
                    Credits = 4
                };

                // Создание курса
                var createdCourse = await client.CreateCourseAsync(newCourse);
                Console.WriteLine($"Создан курс ID: {createdCourse.Id}");

                // Обновление курса
                createdCourse.Description = "Обновлено описание";
                await client.UpdateCourseAsync(createdCourse.Id, createdCourse);

                // Получение всех курсов
                var courses = await client.GetCoursesAsync();
                Console.WriteLine("\nВсе курсы:");
                foreach (var course in courses)
                {
                    Console.WriteLine($"{course.Id}: {course.Title} - {course.Description}");
                }

                // Пример работы со студентами
                var newStudent = new Student
                {
                    Name = "Генько Ксения"
                };

                // Создание студента
                var createdStudent = await client.CreateStudentAsync(newStudent);
                Console.WriteLine($"\nСоздан студент с ID: {createdStudent.Id}");

                // Получение всех студентов
                var students = await client.GetStudentsAsync();
                Console.WriteLine("\nВсе студенты:");
                foreach (var student in students)
                {
                    Console.WriteLine($"{student.Id}: {student.Name}");
                }

                // Удаление тестовых данных
                await client.DeleteCourseAsync(createdCourse.Id);
                await client.DeleteStudentAsync(createdStudent.Id);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                //if (ex.StatusCode.HasValue)
                //{
                    Console.WriteLine($"Status Code: Ошибка");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}