using System;

public class Student
{
    public string StudentId { get; set; }
    public string Name { get; set; }
    public double Score { get; set; }

    // Constructor
    public Student(string id, string name, double score)
    {
        // Validation: ID không được rỗng
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Lỗi: ID sinh viên không được để trống.");
        }

        // Validation: Name không được rỗng
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Lỗi: Tên sinh viên không được để trống.");
        }

        // Validation: Score phải từ 0 đến 10
        if (score < 0 || score > 10)
        {
            throw new ArgumentException("Lỗi: Điểm phải nằm trong khoảng từ 0 đến 10.");
        }

        this.StudentId = id;
        this.Name = name;
        this.Score = score;
    }

    // Phương thức in thông tin
    public void Display()
    {
        Console.WriteLine($"ID: {StudentId.PadRight(10)} | Tên: {Name.PadRight(20)} | Điểm: {Score}");
    }
}