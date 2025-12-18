using System;
namespace GradeClassification
{
    static void Main(string[] args)
    {
        int score = 75;
        if (score >= 90)
        {
            Console.WriteLine("ban da dat loai xuat sac")
        }
        else if (score >=80)
        {
            Console.WriteLine("ban da dat loai gioi")
        }
        else if (score >=70)
        {
            Console.WriteLine("ban da dat loai kha")
        }
        else if (score >= 60)
        {
            Console.WriteLine("ban da dat loai trung binh")
        }
        else if (score < 60)
        {
            Console.WriteLine("ban da dat loai yeu")
        }
        else {
            Console.WriteLine("ban khong dat")
        }
    }
}