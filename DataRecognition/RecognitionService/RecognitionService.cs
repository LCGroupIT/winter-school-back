using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronOcr;
using System.Drawing;
using System.IO;

namespace RecognitionService
{
    public class RecognitionService
    {
        public Image img;
        private AdvancedOcr OCR;
        private Rectangle areaIssuedFirst;
        private Rectangle areaIssuedSecond;
        private Rectangle areaDateIssued;
        private Rectangle areaCode;
        private Rectangle areaSurname;
        private Rectangle areaName;
        private Rectangle areaPatronymic;
        private Rectangle areaGender;
        private Rectangle areaBirthday;
        private Rectangle areaAddress;
        private Rectangle areaSerial;

        public string ReadIssued
        {
            get
            {
                return OCR.Read(img, areaIssuedFirst).Text + " " + OCR.Read(img, areaIssuedSecond).Text;
            }
        }

        public string ReadDateIssued
        {
            get
            {
                return OCR.Read(img, areaDateIssued).Text;
            }
        }

        public string ReadCode
        {
            get
            {
                return OCR.Read(img, areaCode).Text;
            }
        }

        public string ReadSurname
        {
            get
            {
                return OCR.Read(img, areaSurname).Text;
            }
        }

        public string ReadName
        {
            get
            {
                return OCR.Read(img, areaName).Text;
            }
        }

        public string ReadPatronymic
        {
            get
            {
                return OCR.Read(img, areaPatronymic).Text;
            }
        }

        public string ReadGender
        {
            get
            {
                return OCR.Read(img, areaGender).Text;
            }
        }

        public string ReadBirthday
        {
            get
            {
                return OCR.Read(img, areaBirthday).Text;
            }
        }

        public string ReadAddress
        {
            get
            {
                return OCR.Read(img, areaAddress).Text;
            }
        }

        public string ReadSerial
        {
            get
            {
                img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                return OCR.Read(img, areaSerial).Text;
            }
        }

        public RecognitionService(byte[] bytesArray)
        {
            var ms = new MemoryStream(bytesArray);
            img = Image.FromStream(ms);
           
            AdvancedOcrInit();
            AreasInit();
        }

        #region AdvencedOcrInit
        private void AdvancedOcrInit()
        {
            OCR = new AdvancedOcr
            {
                CleanBackgroundNoise = true,
                EnhanceContrast = true,
                EnhanceResolution = true,
                Language = IronOcr.Languages.Russian.OcrLanguagePack,
                Strategy = AdvancedOcr.OcrStrategy.Advanced,
                ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                DetectWhiteTextOnDarkBackgrounds = false,
                InputImageType = AdvancedOcr.InputTypes.Snippet,
                RotateAndStraighten = false,
                ReadBarCodes = false,
                ColorDepth = 4
            };
        }
        #endregion

        #region AreasInit
        private void AreasInit()
        {
            areaIssuedFirst = new Rectangle
            {
                X = (img.Width / 100) * 25,
                Y = (img.Height / 100) * 8,
                Width = (img.Width / 100) * 75,
                Height = (img.Height / 100) * 5
            };

            areaIssuedSecond = new Rectangle
            {
                X = (img.Width / 100) * 4,
                Y = (img.Height / 100) * 11,
                Width = (img.Width / 100) * 96,
                Height = (img.Height / 100) * 9
            };

            areaDateIssued = new Rectangle
            {
                X = (img.Width / 100) * 20,
                Y = (img.Height / 100) * 20,
                Width = (img.Width / 100) * 25,
                Height = (img.Height / 100) * 5
            };

            areaCode = new Rectangle
            {
                X = (img.Width / 100) * 61,
                Y = (img.Height / 100) * 20,
                Width = (img.Width / 100) * 37,
                Height = (img.Height / 100) * 5
            };

            areaSurname = new Rectangle
            {
                X = (img.Width / 100) * 47,
                Y = (img.Height / 100) * 55,
                Width = (img.Width / 100) * 45,
                Height = (img.Height / 100) * 9
            };

            areaName = new Rectangle
            {
                X = (img.Width / 100) * 47,
                Y = (img.Height / 100) * 64,
                Width = (img.Width / 100) * 50,
                Height = (img.Height / 100) * 6
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
                X = (img.Width / 100) * 10,
                Y = (img.Height / 100) * 1,
                Width = (img.Width / 100) * 30,
                Height = (img.Height / 100) * 7
            };
        }
        #endregion


       /* public string Recognized(int tmp)
        {
            switch (tmp)
            {
                default:
                    return "None";
                case (1):
                    return OCR.Read(img, areaIssuedFirst).Text;
                case (2):
                    return OCR.Read(img, areaIssuedSecond).Text;
            }
        }*/
    }
}
