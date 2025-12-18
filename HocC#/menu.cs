using System;

namespace StudentManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Thiết lập hỗ trợ tiếng Việt có dấu (nếu cần)
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
                        case "1": // Thêm
                            Console.Write("Nhập ID: ");
                            string id = Console.ReadLine();
                            Console.Write("Nhập Tên: ");
                            string name = Console.ReadLine();
                            Console.Write("Nhập Điểm: ");
                            double score = double.Parse(Console.ReadLine());
                            
                            manager.AddStudent(id, name, score);
                            break;

                        case "2": // Xóa
                            Console.Write("Nhập ID cần xóa: ");
                            string removeId = Console.ReadLine();
                            manager.RemoveStudent(removeId);
                            break;

                        case "3": // Cập nhật
                            Console.Write("Nhập ID cần sửa điểm: ");
                            string updateId = Console.ReadLine();
                            Console.Write("Nhập điểm mới: ");
                            double newScore = double.Parse(Console.ReadLine());
                            manager.UpdateScore(updateId, newScore);
                            break;

                        case "4": // In danh sách
                            manager.DisplayAllStudents();
                            break;

                        case "5": // Điểm trung bình
                            double avg = manager.GetAverageScore();
                            Console.WriteLine($"--> Điểm trung bình cả lớp: {avg:F2}");
                            break;

                        case "6": // Điểm cao nhất
                            double max = manager.GetMaxScore();
                            Console.WriteLine($"--> Điểm cao nhất: {max}");
                            break;

                        case "7": // Tìm kiếm
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
                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("LỖI: Định dạng số không hợp lệ (Vui lòng nhập điểm là số).");
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