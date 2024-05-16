using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_sharp
{
    public class NonDB
    {
        public NonDB() { }
        public List<Student> Students = [];
        public List<Course> Courses = [];
       
        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }

        public bool EnrollStudentInCourse(Student student, Course course)
        {
            return student.AddCourse(course);
        }

        public Student? FindByStudentId(int id)
        {
            return Students.Find(x => x.StudentId == id);
        }

        public Student? FindStudentByName(string studentName)
        {
            return Students.Find(x => x.Name == studentName);
        }

        public Course? FindCourseByName(string courseName)
        {
            return Courses.Find(x => x.CourseName == courseName);
        }

        public Course? FindCourseById(int courseId)
        {
            return Courses.Find(x => x.CourseId == courseId);
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            foreach (Student student in Students)
            {
                sb.Append($"({student.StudentId}) {student.Name} enrolled in:\n");
                foreach (Course course in student.Courses)
                {
                    sb.Append($"\t({course.CourseId}) {course.CourseName}\n");
                }
            }

            return sb.ToString();
        }
    }
}
