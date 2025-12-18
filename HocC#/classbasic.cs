using System;
namespace basicclass
{
    class Student
    {
        private string name;
        private int GPA;
        private int studentID;
        
        // Constructor
        public Student(string name, int gpa, int studentID)
        {
            this.name = name;
            this.GPA = gpa;
            this.studentID = studentID;
        }
        
        // Phương thức in ra thông tin
        public void Display()
        {
            Console.WriteLine("Name: " + name);
            Console.WriteLine("GPA: " + GPA);
            Console.WriteLine("Student ID: " + studentID);
        }
    }
}