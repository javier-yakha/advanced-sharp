using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace advanced_sharp
{
    public class Student
    {
        public Student(string name)
        {
            StudentId = StaticStudentId;
            Name = name;
            StaticStudentId++;
        }
        private static int StaticStudentId = 1;
        public int StudentId { get; set; }
        public string Name { get; set; }
        public List<Course> Courses { get; set; } = [];

        public bool AddCourse(Course course)
        {
            if (!Courses.Exists(x => x.CourseId == course.CourseId))
            {
                Courses.Add(course);
                course.AddStudent(this);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"({StudentId}) {Name}:\n");

            foreach (Course course in Courses)
            {
                sb.Append($"\t({course.CourseId}) {course.CourseName}\n");
            }

            return sb.ToString();
        }
    }
}
