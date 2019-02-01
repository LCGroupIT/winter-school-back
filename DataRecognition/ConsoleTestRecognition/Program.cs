using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using RecognitionService;
using Domain.Model;

namespace ConsoleTestRecognition
{
    class Program
    {
        static void Main(string[] args)
        {
            var imagePassport = new Bitmap("pass11.jpg");
            /*Image img = Image.FromFile("C:/Users/WinterSchool/Desktop/Новая папка/DataRecognition/ConsoleTestRecognition/img/Passport.jpg");
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] input = ms.ToArray();*/

            //RecognitionService.RecognitionService recognition = new RecognitionService.RecognitionService(input);
            var areaSerial = new Rectangle
            {
                X = (int)((imagePassport.Width / 100.0) * 2.0),
                Y = (int)((imagePassport.Height / 100.0) * 2.0),
                Width = (int)((imagePassport.Width / 100.0) * 40.0),
                Height = (int)((imagePassport.Height / 100.0) * 7.0)
            };

            var passport = Recognition.PassportRecognition(imagePassport);
            /*imagePassport.RotateFlip(RotateFlipType.Rotate270FlipNone);
            var img = imagePassport.Clone(areaSerial, imagePassport.PixelFormat);
            img.Save("Serial.jpg");*/

            /*Console.WriteLine(recognition.ReadIssued);
            //Console.WriteLine(recognition.ReadDateIssued);
            Console.WriteLine(recognition.ReadCode);
            Console.WriteLine(recognition.ReadSurname);
            Console.WriteLine(recognition.ReadName);
            Console.WriteLine(recognition.ReadPatronymic);
            Console.WriteLine(recognition.ReadGender);
            Console.WriteLine(recognition.ReadBirthday);
            Console.WriteLine(recognition.ReadAddress);
            Console.WriteLine(recognition.ReadSerial);*/
            Console.WriteLine(passport.IssuedBy);
            Console.WriteLine(passport.IssuedOn);
            Console.WriteLine(passport.IssuedDepartment);
            Console.WriteLine(passport.Lastname);
            Console.WriteLine(passport.Firstname);
            Console.WriteLine(passport.Secondname);
            Console.WriteLine(passport.Sex);
            Console.WriteLine(passport.Birthday);
            Console.WriteLine(passport.Address);
            Console.WriteLine(passport.Series);
            Console.WriteLine(passport.Number);
            Console.ReadKey();
        }
    }
}
