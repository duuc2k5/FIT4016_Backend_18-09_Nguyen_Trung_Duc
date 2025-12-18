using System;
namespace BasicClass
{
    class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // Constructor
        public Product(int productID, string productName, double price, int quantity)
        {
            ProductID = productID;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }
    }

    class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int GPA { get; set; }
        public Student(int studentID, string name, int gpa)
        {
            StudentID = studentID;
            Name = name;
            GPA = gpa;
        }
        static void Main(string[] args)
        {
            Student student = new Student(1, "Nguyen Van A", 9);
            Console.WriteLine("Student ID: " + student.StudentID);
            Console.WriteLine("Name: " + student.Name);
            Console.WriteLine("GPA: " + student.GPA);
        }
    }
}