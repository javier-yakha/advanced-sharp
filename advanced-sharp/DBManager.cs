using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_sharp
{
    public static class DBManager
    {
        public static void InitializeDB(NonDB DB)
        {
            InitializeStudents(DB);
            InitializeCourses(DB);
            EnrollStudentsInRandomCourses(DB);
        }
        public static void InitializeStudents(NonDB DB)
        {
            List<Student> students =
            [
                new Student("Pascal"),
                new Student("Sandra"),
                new Student("Kevin"),
                new Student("Lisa"),
                new Student("Bastian"),
                new Student("Kristina"),
                new Student("Javier"),
            ];
            foreach (Student student in students)
            {
                DB.AddStudent(student);
            }
            
            return;
        }

        public static void InitializeCourses(NonDB DB)
        {
            List<Course> courses =
            [
                new Course("Databases"),
                new Course("LINQ"),
                new Course("WPF"),
                new Course("Docker"),
                new Course("Data Structures"),
                new Course("Algorithms"),
            ];
            foreach (Course course in courses)
            {
                DB.AddCourse(course);
            }

            return;
        }

        public static void EnrollNewStudentInCourses(NonDB DB, Student student)
        {
            Random random = new();
            int amountOfCourses = DB.Courses.Count;
            int coursesToAddRemaining = random.Next(2, amountOfCourses);

            while (coursesToAddRemaining > 0)
            {
                int randomCourseIndex = random.Next(2, amountOfCourses);
                Course randomCourse = DB.Courses[randomCourseIndex];

                bool enrolled = DB.EnrollStudentInCourse(student, randomCourse);
                if (enrolled)
                {
                    coursesToAddRemaining--;
                }
            }
        }

        public static void EnrollStudentsInRandomCourses(NonDB DB)
        {
            Random random = new();
            int amountOfCourses = DB.Courses.Count;

            foreach (Student student in DB.Students)
            {
                int coursesToAddRemaining = random.Next(2, amountOfCourses);
                while (coursesToAddRemaining > 0)
                {
                    int randomCourseIndex = random.Next(amountOfCourses);
                    Course randomCourse = DB.Courses[randomCourseIndex];

                    bool enrolled = DB.EnrollStudentInCourse(student, randomCourse);
                    if (enrolled)
                    {
                        coursesToAddRemaining--;
                    }
                }
            }
        }
    }
}
