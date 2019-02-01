using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;
using System.Drawing;
using System.IO;
using AForge.Imaging;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Domain.Model;


namespace RecognitionService
{
    public static class Recognition
    {

        public static Passport PassportRecognition(Bitmap imagePassport)
        {
            var passport = new Passport();

            AdvancedOcrInit();
            AreasInit(imagePassport);

            Bitmap grayDate = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709.Apply(imagePassport);
            AForge.Imaging.Filters.OtsuThreshold threshold = new AForge.Imaging.Filters.OtsuThreshold();
            Bitmap binaryImage = threshold.Apply(grayDate);
            AForge.Imaging.Filters.Invert invertFilter = new AForge.Imaging.Filters.Invert();

            passport.Sex = PostSex(OCR.Read(imagePassport, areaGender).Text);

            imagePassport = invertFilter.Apply(binaryImage);
            
            passport.IssuedBy = OCR.Read(imagePassport, areaIssuedFirst).Text + " " + OCR.Read(imagePassport, areaIssuedSecond).Text;
            passport.IssuedOn = PostDate(OCR.Read(grayDate, areaDateIssued).Text);
            passport.IssuedDepartment = OCR.Read(imagePassport, areaCode).Text;
            passport.Lastname = OCR.Read(grayDate, areaSurname).Text;
            passport.Firstname = OCR.Read(grayDate, areaName).Text;
            passport.Secondname = OCR.Read(imagePassport, areaPatronymic).Text;
            passport.Birthday = PostDate(OCR.Read(imagePassport, areaBirthday).Text);
            passport.Address = OCR.Read(imagePassport, areaAddress).Text;

            grayDate.RotateFlip(RotateFlipType.Rotate270FlipNone);
            passport.Series = OCR.Read(invertFilter.Apply(grayDate), areaSerial).Text;
            passport.Number = OCR.Read(invertFilter.Apply(grayDate), areaNumber).Text;

            return passport;
        }

        private static AdvancedOcr OCR;
        private static Rectangle areaIssuedFirst;
        private static Rectangle areaIssuedSecond;
        private static Rectangle areaDateIssued;
        private static Rectangle areaCode;
        private static Rectangle areaSurname;
        private static Rectangle areaName;
        private static Rectangle areaPatronymic;
        private static Rectangle areaGender;
        private static Rectangle areaBirthday;
        private static Rectangle areaAddress;
        private static Rectangle areaSerial;
        private static Rectangle areaNumber;

        #region AdvencedOcrInit
        private static void AdvancedOcrInit()
        {
            OCR = new AdvancedOcr
            {
                CleanBackgroundNoise = true,
                EnhanceContrast = true,
                EnhanceResolution = false,
                Language = IronOcr.Languages.Russian.OcrLanguagePack,
                Strategy = AdvancedOcr.OcrStrategy.Advanced,
                ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                DetectWhiteTextOnDarkBackgrounds = true,
                InputImageType = AdvancedOcr.InputTypes.AutoDetect,
                RotateAndStraighten = true,
                ReadBarCodes = false,
                ColorDepth = 4
            };
        }
        #endregion

        #region AreasInit
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
                X = (img.Width / 100) * 47,
                Y = (img.Height / 100) * 70,
                Width = (img.Width / 100) * 50,
                Height = (img.Height / 100) * 5
            };

            areaGender = new Rectangle
            {
                X = (img.Width / 100) * 45,
                Y = (img.Height / 100) * 75,
                Width = (img.Width / 100) * 10,
                Height = (img.Height / 100) * 5
            };

            areaBirthday = new Rectangle
            {
                X = (img.Width / 100) * 57,
                Y = (img.Height / 100) * 75,
                Width = (img.Width / 100) * 40,
                Height = (img.Height / 100) * 5
            };

            areaAddress = new Rectangle
            {
                X = (img.Width / 100) * 47,
                Y = (img.Height / 100) * 79,
                Width = (img.Width / 100) * 50,
                Height = (img.Height / 100) * 13
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
        #endregion

        private static DateTime PostDate(string date)
        {
            /*Bitmap dateImage = img.Clone(areaSurname, img.PixelFormat);
            dateImage.Save("dateImage.jpg");
            */
            //var temp = OCR.Read(img, areaDateIssued).Text;
            //char[] tmp;
            string day = "";
            string year = "";
            string month = "";
            //string dateString = "";
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
            //Console.WriteLine(day + " " + month + " " + year);
            //return dateString;

            int yearInt;
            int.TryParse(year, out yearInt);

            int monthInt;
            int.TryParse(month, out monthInt);

            int dayInt;
            int.TryParse(day, out dayInt);

            //  Console.WriteLine(OCR.Read(img)); 

            return /*readDateIssued = */new DateTime(yearInt, monthInt, dayInt);
        }

        private static SexType PostSex(string sex)
        {
            if ((sex[0] == 'М') || (sex[0] == 'м') || (sex[0] == 'Н') || (sex[0] == 'н'))
                return SexType.Male;
            else
                return SexType.Female;
        }
    }
}

