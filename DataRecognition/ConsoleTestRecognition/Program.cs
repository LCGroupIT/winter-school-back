using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using RecognitionService;

namespace ConsoleTestRecognition
{
    class Program
    {
        static void Main(string[] args)
        {
            Image img = Image.FromFile("C:/Users/WinterSchool/Desktop/Новая папка/DataRecognition/ConsoleTestRecognition/img/Passport.jpg");
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] input = ms.ToArray();

            RecognitionService.RecognitionService recognition = new RecognitionService.RecognitionService(input);
            Console.WriteLine(recognition.ReadIssued);
            Console.WriteLine(recognition.ReadDateIssued);
            Console.WriteLine(recognition.ReadCode);
            Console.WriteLine(recognition.ReadSurname);
            Console.WriteLine(recognition.ReadName);
            Console.WriteLine(recognition.ReadPatronymic);
            Console.WriteLine(recognition.ReadGender);
            Console.WriteLine(recognition.ReadBirthday);
            Console.WriteLine(recognition.ReadAddress);
            Console.WriteLine(recognition.ReadSerial);
            Console.ReadKey();
        }
    }
}
