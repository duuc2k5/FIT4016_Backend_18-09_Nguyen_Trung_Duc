using System;

public class StudentManager
{
    private Student[] students = new Student[50];
    private int count = 0; // Số lượng sinh viên hiện tại

    // Tìm sinh viên theo ID (Helper method)
    public Student FindStudentById(string id)
    {
        for (int i = 0; i < count; i++)
        {
            if (students[i].StudentId.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                return students[i];
            }
        }
        return null;
    }

    // Thêm sinh viên mới
    public void AddStudent(string id, string name, double score)
    {
        if (count >= 50)
        {
            throw new InvalidOperationException("Danh sách đã đầy (tối đa 50 sinh viên).");
        }

        if (FindStudentById(id) != null)
        {
            throw new ArgumentException($"Sinh viên với ID {id} đã tồn tại.");
        }

        // Tạo đối tượng Student (Constructor sẽ tự kiểm tra validation)
        Student newStudent = new Student(id, name, score);
        students[count] = newStudent;
        count++;
        Console.WriteLine("--> Thêm sinh viên thành công!");
    }

    // Xóa sinh viên theo ID
    public void RemoveStudent(string id)
    {
        int index = -1;
        // Tìm vị trí sinh viên
        for (int i = 0; i < count; i++)
        {
            if (students[i].StudentId.Equals(id, StringComparison.OrdinalIgnoreCase))
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            Console.WriteLine("--> Không tìm thấy sinh viên để xóa.");
            return;
        }

        // Dời các phần tử phía sau lên trước để lấp chỗ trống
        for (int i = index; i < count - 1; i++)
        {
            students[i] = students[i + 1];
        }

        students[count - 1] = null; // Xóa phần tử cuối thừa
        count--;
        Console.WriteLine("--> Xóa sinh viên thành công!");
    }

    // Cập nhật điểm
    public void UpdateScore(string id, double newScore)
    {
        Student sv = FindStudentById(id);
        if (sv == null)
        {
            Console.WriteLine("--> Không tìm thấy sinh viên.");
            return;
        }

        if (newScore < 0 || newScore > 10)
        {
            throw new ArgumentException("Điểm mới không hợp lệ (0-10).");
        }

        sv.Score = newScore;
        Console.WriteLine("--> Cập nhật điểm thành công!");
    }

    // Tính điểm trung bình
    public double GetAverageScore()
    {
        if (count == 0) return 0;
        double sum = 0;
        for (int i = 0; i < count; i++)
        {
            sum += students[i].Score;
        }
        return sum / count;
    }

    // Tìm điểm cao nhất
    public double GetMaxScore()
    {
        if (count == 0) return 0;
        double max = students[0].Score;
        for (int i = 1; i < count; i++)
        {
            if (students[i].Score > max)
            {
                max = students[i].Score;
            }
        }
        return max;
    }

    // In danh sách tất cả sinh viên
    public void DisplayAllStudents()
    {
        if (count == 0)
        {
            Console.WriteLine("--> Danh sách trống.");
            return;
        }

        Console.WriteLine("\n--- DANH SÁCH SINH VIÊN ---");
        for (int i = 0; i < count; i++)
        {
            students[i].Display();
        }
        Console.WriteLine($"Tổng số: {count} sinh viên.");
    }
}