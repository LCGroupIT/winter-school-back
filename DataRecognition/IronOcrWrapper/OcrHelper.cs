using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Domain.Model;
using Tesseract;

namespace IronOcrWrapper
{
    public class OcrHelper
    {
        public static Passport Recognise(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var passportImage = new Bitmap(ms);
                if (!ImagePassport.HaveFace(passportImage))
                    return new Passport();

                var p = PassportRecognition(passportImage);
                return p;
            }
        }

        private static TesseractEngine OCR;
        private static Rectangle areaIssuedFirst;
        private static Rectangle areaIssuedSecond;
        private static Rectangle areaDateIssued;
        private static Rectangle areaCode;
        private static Rectangle areaSurname;
        private static Rectangle areaName;
        private static Rectangle areaPatronymic;
        private static Rectangle areaGender;
        private static Rectangle areaBirthday;
        private static Rectangle areaAddressFirst;
        private static Rectangle areaAddressSecond;
        private static Rectangle areaSerial;
        private static Rectangle areaNumber;

        public static Passport PassportRecognition(Bitmap imagePassport)
        {
            OCR = new TesseractEngine(@"C:\tessdata\", "rus", EngineMode.Default);

            AreasInit(imagePassport);

            var passport = new Passport
            {
                IssuedOn = PostprocessingDate(Read(CropImage(imagePassport, areaDateIssued))),
                Birthday = PostprocessingDate(Read(CropImage(imagePassport, areaBirthday))),
                IssuedDepartment = PostProcessingCode(Read(CropImage(imagePassport, areaCode))),
                IssuedBy = PostProcessingString(Read(CropImage(imagePassport, areaIssuedFirst)))
                           + " " + PostProcessingString(Regex.Replace(Read(CropImage(imagePassport, areaIssuedSecond)),
                               "\n", " ")),
                Lastname = PostProcessingString(Read(CropImage(imagePassport, areaSurname))),
                Firstname = PostProcessingString(Read(CropImage(imagePassport, areaName))),
                Secondname = PostProcessingString(Read(CropImage(imagePassport, areaPatronymic))),
                Sex = PostSex(Read(CropImage(imagePassport, areaGender))),
                Address = PostProcessingString(Read(CropImage(imagePassport, areaAddressFirst)))
                          + " " + PostProcessingString(Regex.Replace(Read(CropImage(imagePassport, areaAddressSecond)),
                              "\n", " "))
            };

            imagePassport.RotateFlip(RotateFlipType.Rotate270FlipNone);

            passport.Series = PostProcessingNumber(Read(CropImage(imagePassport, areaSerial)));
            passport.Number = PostProcessingNumber(Read(CropImage(imagePassport, areaNumber)));

            return passport;
        }

        private static void AreasInit(Bitmap img)
        {
            areaIssuedFirst = new Rectangle
            {
                X = (int)(((double)img.Width / 100.0) * 20),
                Y = (int)(((double)img.Height / 100.0) * 6.0),
                Width = (int)(((double)img.Width / 100.0) * 78.0),
                Height = (int)(((double)img.Height / 100.0) * 6.0)
            }; 

            areaIssuedSecond = new Rectangle
            {
                X = (int)(((double)img.Width / 100.0) * 4.0),
                Y = (int)(((double)img.Height / 100.0) * 12.0),
                Width = (int)(((double)img.Width / 100.0) * 88.0),
                Height = (int)(((double)img.Height / 100.0) * 7.0)
            };

            areaDateIssued = new Rectangle      
            {
                X = (int)(((double)img.Width / 100.0) * 17.6),
                Y = (int)(((double)img.Height / 100.0) * 19.3),
                Width = (int)(((double)img.Width / 100.0) * 23.0),
                Height = (int)(((double)img.Height / 100.0) * 5.0)
            };

            areaCode = new Rectangle
            {
                X = (int)(((double)img.Width / 100.0) * 55.0),
                Y = (int)(((double)img.Height / 100.0) * 18.5),
                Width = (int)(((double)img.Width / 100.0) * 37.0),
                Height = (int)(((double)img.Height / 100.0) * 7.0)
            };

            areaSurname = new Rectangle
            {
                X = (int)(((double)img.Width / 100.0) * 41.0),
                Y = (int)(((double)img.Height / 100.0) * 55.0),
                Width = (int)(((double)img.Width / 100.0) * 44.0),
                Height = (int)(((double)img.Height / 100.0) * 9.0)
            };

            areaName = new Rectangle
            {
                X = (int)((img.Width / 100.0) * 47.0),
                Y = (int)((img.Height / 100.0) * 63.8),
                Width = (int)((img.Width / 100.0) * 46.0),
                Height = (int)((img.Height / 100.0) * 6.0)
            };

            areaPatronymic = new Rectangle
            {
                X = (int)((img.Width / 100.0) * 41.0),
                Y = (int)((img.Height / 100.0) * 68.0),
                Width = (int)((img.Width / 100.0) * 50.0),
                Height = (int)((img.Height / 100.0) * 5.0)
            };

            areaGender = new Rectangle
            {
                X = (int)((img.Width / 100.0) * 37.0),
                Y = (int)((img.Height / 100.0) * 71.5),
                Width = (int)((img.Width / 100.0) * 13.0),
                Height = (int)((img.Height / 100.0) * 5.0)
            };

            areaBirthday = new Rectangle
            {
                X = (int)((img.Width / 100.0) * 57.0),
                Y = (int)((img.Height / 100.0) * 71.5),
                Width = (int)((img.Width / 100.0) * 35.0),
                Height = (int)((img.Height / 100.0) * 4.7)
            };

            areaAddressFirst = new Rectangle       
            {
                X = (int)((img.Width / 100.0) * 42.0),
                Y = (int)((img.Height / 100.0) * 75.4),
                Width = (int)((img.Width / 100.0) * 51.0),
                Height = (int)((img.Height / 100.0) * 5.2)
            };

            areaAddressSecond = new Rectangle
            {
                X = (int)((img.Width / 100.0) * 39.0),
                Y = (int)((img.Height / 100.0) * 79.4),
                Width = (int)((img.Width / 100.0) * 53.0),
                Height = (int)((img.Height / 100.0) * 10.2)
            };

            areaSerial = new Rectangle
            {
                X = (int)((img.Width / 100.0) * 2.0),
                Y = (int)((img.Height / 100.0) * 1.2),
                Width = (int)((img.Width / 100.0) * 32.0),
                Height = (int)((img.Height / 100.0) * 5.5)

            };

            areaNumber = new Rectangle
            {
                X = (int)((img.Width / 100) * 35.0),
                Y = (int)((img.Height / 100) * 1.2),
                Width = (int)((img.Width / 100) * 32.0),
                Height = (int)((img.Height / 100) * 5.5)
            };
        }

        private static Bitmap CropImage(Bitmap img, Rectangle area)
        {
            //img.Clone(area, img.PixelFormat).Save("Image" + new Random().Next().ToString() + ".jpg");

            return img.Clone(area, img.PixelFormat);
        }

        private static string Read(Bitmap img)
        {
            var read = OCR.Process(img);
            var tmp = read.GetText();
            read.Dispose();
            return tmp;
        }

        private static DateTime PostprocessingDate(string date)
        {
            string day = "";
            string year = "";
            string month = "";

            int count = 0;

            foreach (char s in date)
            {
                if (char.IsNumber(s))
                {
                    if (count < 2)
                    {
                        day += s;
                        count++;
                    }
                    else if (count > 1 && count < 4)
                    {
                        month += s;
                        count++;
                    }
                    else if (count > 3 && count < 8)
                    {
                        year += s;
                        count++;
                    }
                }
            }

            int.TryParse(year, out int yearInt);
            int.TryParse(month, out int monthInt);
            int.TryParse(day, out int dayInt);

            try
            {
                return new DateTime(yearInt, monthInt, dayInt);
            }
            catch
            {
                return new DateTime(4040, 4, 4);
            }
        }

        private static SexType PostSex(string sex)
        {
            if (sex.Equals(""))
                return SexType.Male;
            else
            {
                if ((sex[0] == 'Ж') || (sex[0] == 'ж') || (sex[0] == 'Х') || (sex[0] == 'х'))
                    return SexType.Female;
                else
                    return SexType.Male;
            }
        }

        private static string PostProcessingString(string tmp)
        {
            //char[] tmpChar = { '/', ',', '`', '~', '!', '@', '$', '%', '^', '&', '*', '', '', '' };

            var regular = "ЁЙЦУКЕНГШЩЗХЪЭЖДЛОРПАВЫФЯЧСМИТЬБЮ. ёйцукенгшщзхъэждлорпавыфячсмитьбю";

            var result = "";

            foreach(char c in tmp)
            {
                if (regular.IndexOf(c) != -1)
                    result += c;
            }

            return result;
        }

        private static string PostProcessingNumber(string tmp)
        {
            var regular = "0123456789";

            var result = "";

            foreach (char c in tmp)
            {
                if (regular.IndexOf(c) != -1)
                    result += c;
            }

            return result;
        }

        private static string PostProcessingCode(string tmp)
        {
            var count = 0;
            var result = "";

            foreach (char s in tmp)
            {
                if (char.IsNumber(s))
                {
                    if (count < 6)
                    {
                        result += s;
                        count++;
                    }   
                    if (count == 3)
                    {
                        result += '-';
                    }
                }
            }

            return result;
        }
    }
}