using System;

namespace StudentManagementSystem
{
    // 1. CLASS STUDENT
    public class Student
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public double Score { get; set; }

        // Constructor
        public Student(string id, string name, double score)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Lỗi: ID sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Lỗi: Tên sinh viên không được để trống.");

            if (score < 0 || score > 10)
                throw new ArgumentException("Lỗi: Điểm phải nằm trong khoảng từ 0 đến 10.");

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

    // 2. CLASS STUDENT MANAGER
    public class StudentManager
    {
        private Student[] students = new Student[50];
        private int count = 0;

        public Student FindStudentById(string id)
        {
            for (int i = 0; i < count; i++)
            {
                if (students[i].StudentId.Equals(id, StringComparison.OrdinalIgnoreCase))
                    return students[i];
            }
            return null;
        }

        public void AddStudent(string id, string name, double score)
        {
            if (count >= 50)
                throw new InvalidOperationException("Danh sách đã đầy (tối đa 50 sinh viên).");

            if (FindStudentById(id) != null)
                throw new ArgumentException($"Sinh viên với ID {id} đã tồn tại.");

            Student newStudent = new Student(id, name, score);
            students[count] = newStudent;
            count++;
            Console.WriteLine("--> Thêm sinh viên thành công!");
        }

        public void RemoveStudent(string id)
        {
            int index = -1;
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

            for (int i = index; i < count - 1; i++)
            {
                students[i] = students[i + 1];
            }

            students[count - 1] = null;
            count--;
            Console.WriteLine("--> Xóa sinh viên thành công!");
        }

        public void UpdateScore(string id, double newScore)
        {
            Student sv = FindStudentById(id);
            if (sv == null)
            {
                Console.WriteLine("--> Không tìm thấy sinh viên.");
                return;
            }

            if (newScore < 0 || newScore > 10)
                throw new ArgumentException("Điểm mới không hợp lệ (0-10).");

            sv.Score = newScore;
            Console.WriteLine("--> Cập nhật điểm thành công!");
        }

        public double GetAverageScore()
        {
            if (count == 0) return 0;
            double sum = 0;
            for (int i = 0; i < count; i++)
                sum += students[i].Score;
            return sum / count;
        }

        public double GetMaxScore()
        {
            if (count == 0) return 0;
            double max = students[0].Score;
            for (int i = 1; i < count; i++)
            {
                if (students[i].Score > max)
                    max = students[i].Score;
            }
            return max;
        }

        public void DisplayAllStudents()
        {
            if (count == 0)
            {
                Console.WriteLine("--> Danh sách trống.");
                return;
            }

            Console.WriteLine("\n--- DANH SÁCH SINH VIÊN ---");
            for (int i = 0; i < count; i++)
                students[i].Display();
            Console.WriteLine($"Tổng số: {count} sinh viên.");
        }
    }

    // 3. CLASS PROGRAM (MAIN)
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            StudentManager manager = new StudentManager();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n========== QUẢN LÝ SINH VIÊN ==========");
                Console.WriteLine("1. Thêm sinh viên");
                Console.WriteLine("2. Xóa sinh viên");
                Console.WriteLine("3. Cập nhật điểm");
                Console.WriteLine("4. In danh sách");
                Console.WriteLine("5. Tính điểm trung bình");
                Console.WriteLine("6. Tìm điểm cao nhất");
                Console.WriteLine("7. Tìm sinh viên theo ID");
                Console.WriteLine("0. Thoát");
                Console.WriteLine("=======================================");
                Console.Write("Chọn chức năng: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Nhập ID: ");
                            string id = Console.ReadLine();
                            Console.Write("Nhập Tên: ");
                            string name = Console.ReadLine();
                            Console.Write("Nhập Điểm: ");
                            double score = double.Parse(Console.ReadLine());
                            manager.AddStudent(id, name, score);
                            break;

                        case "2":
                            Console.Write("Nhập ID cần xóa: ");
                            string removeId = Console.ReadLine();
                            manager.RemoveStudent(removeId);
                            break;

                        case "3":
                            Console.Write("Nhập ID cần sửa điểm: ");
                            string updateId = Console.ReadLine();
                            Console.Write("Nhập điểm mới: ");
                            double newScore = double.Parse(Console.ReadLine());
                            manager.UpdateScore(updateId, newScore);
                            break;

                        case "4":
                            manager.DisplayAllStudents();
                            break;

                        case "5":
                            Console.WriteLine($"--> Điểm trung bình cả lớp: {manager.GetAverageScore():F2}");
                            break;

                        case "6":
                            Console.WriteLine($"--> Điểm cao nhất: {manager.GetMaxScore()}");
                            break;

                        case "7":
                            Console.Write("Nhập ID cần tìm: ");
                            string searchId = Console.ReadLine();
                            Student sv = manager.FindStudentById(searchId);
                            if (sv != null)
                            {
                                Console.WriteLine("--> Tìm thấy:");
                                sv.Display();
                            }
                            else
                            {
                                Console.WriteLine("--> Không tìm thấy sinh viên này.");
                            }
                            break;

                        case "0":
                            running = false;
                            Console.WriteLine("Đã thoát chương trình.");
                            break;

                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("LỖI: Định dạng số không hợp lệ.");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"LỖI DỮ LIỆU: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"LỖI KHÔNG XÁC ĐỊNH: {ex.Message}");
                }
            }
        }
    }
}