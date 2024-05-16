using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_sharp
{
    public class Course
    {
        public Course(string courseName) 
        {
            CourseId = StaticId;
            CourseName = courseName;
            StaticId++;
        }
        public static int StaticId = 1;
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<Student> Students { get; set; } = [];

        public void AddStudent(Student student)
        {
            if (!Students.Exists(x => x.StudentId == student.StudentId))
            {
                Students.Add(student);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new();

            sb.Append($"({CourseId}) {CourseName}:\n");

            foreach (Student student in Students)
            {
                sb.Append($"\t({student.StudentId}) {student.Name}\n");
            }

            return sb.ToString();
        }
    }
}
