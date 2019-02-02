using System;
using System.IO;
using IronOcrWrapper;

namespace Recognise.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bytes = File.ReadAllBytes(args[0]);
            var p = OcrHelper.Recognise(bytes);

            Console.WriteLine(p.Firstname);
            Console.WriteLine(p.Secondname);
            Console.WriteLine(p.Lastname);
            Console.WriteLine(p.Sex);
            Console.WriteLine(p.Address);
            Console.WriteLine(p.Number);
            Console.WriteLine(p.Series);
            Console.WriteLine(p.Birthday);
            Console.WriteLine(p.IssuedBy);
            Console.WriteLine(p.IssuedOn);
            Console.WriteLine(p.IssuedDepartment);
        }
    }
}
