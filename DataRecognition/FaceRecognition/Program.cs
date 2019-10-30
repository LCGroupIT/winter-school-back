using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Math.Geometry;
using AForge.Imaging.Filters;
using System.Drawing;
using Accord.Imaging.Filters;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using CheckImage;
using System.IO;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OcrService.Interfaces;
using Domain.Model;


namespace FaceRecognition
{
    class Program
    {

        /*public bool HaveFace(Bitmap imagePassport)
        {
            Rectangle cloneRect = new Rectangle(0, imagePassport.Height/2, imagePassport.Width/2, imagePassport.Height/2);
            Bitmap cloneBitmap = imagePassport.Clone(cloneRect, imagePassport.PixelFormat);
            HaarObjectDetector faceDetector = new HaarObjectDetector(new FaceHaarCascade(), minSize: 70, searchMode: ObjectDetectorSearchMode.Average);
            // создаём объект, который нужен для выделения объектов на изображении
            RectanglesMarker facesMarker = new RectanglesMarker(Color.Red);
            // распознаём лица
            IEnumerable<Rectangle> face = faceDetector.ProcessFrame(cloneBitmap);
            facesMarker.Rectangles = faceDetector.ProcessFrame(cloneBitmap);
            // выделяем лица на изображении
            facesMarker.ApplyInPlace(cloneBitmap);
            cloneBitmap.Save("Clone.jpg");
            if (face.Count() > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool HaveLabelRF(Bitmap imagePassport)
        {
            bool haveLabel = false;
            // 1- grayscale image
            Bitmap grayImage = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709.Apply(imagePassport);

            // 2 - Otsu thresholding
            AForge.Imaging.Filters.OtsuThreshold threshold = new AForge.Imaging.Filters.OtsuThreshold();
            Bitmap binaryImage = threshold.Apply(grayImage);
            AForge.Imaging.Filters.Invert invertFilter = new AForge.Imaging.Filters.Invert();
            Bitmap invertImage = invertFilter.Apply(binaryImage);
            // 3 - Blob counting
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.FilterBlobs = true;
            blobCounter.MinWidth = 10;
            blobCounter.MinWidth = 10;
            blobCounter.ProcessImage(invertImage);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // 4 - check shape of each blob
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            int count = 0;

            // create graphics object for drawing on image
            //Graphics g = Graphics.FromImage(imagePassport);
            //Pen pen = new Pen(Color.Red, 3);

            foreach (Blob blob in blobs)
            {

                List<AForge.IntPoint> edgePoint = blobCounter.GetBlobsEdgePoints(blob);

                // check if shape looks like a circle
                AForge.Point center;
                float radius;

                if (shapeChecker.IsCircle(edgePoint, out center, out radius))
                {
                    count++;
                    Rectangle cloneRect = new Rectangle((int)(center.X - radius), (int)(center.Y - radius + 1.0), (int)(radius * 2) + 1, (int)(radius * 2) + 1);
                    Bitmap RF = new Bitmap(binaryImage.Clone(cloneRect, binaryImage.PixelFormat), 32, 32);
                    //RF = threshold.Apply(RF);
                    Bitmap RF2 = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709.Apply(RF);
                    Bitmap RF3 = threshold.Apply(RF2);

                    if (CompareLabel(RF3) > 70.0)
                        haveLabel = true;
                    //RF3.Save("RF8.jpg");
                    //invertImage.Save("dsf.jpg");
                    //g.DrawEllipse(pen, (int)(center.X - radius), (int)(center.Y - radius+1.0), (int)(radius * 2)+1, (int)(radius * 2)+1);
                    //imagePassport.Save("result.jpg");
                }
            }
            //g.Dispose();
            //pen.Dispose();
            /*if (count == 1)
                return true;
            else
                return false;*/
            /*return haveLabel;
        }

        private double CompareLabel(Bitmap icon)
        {
            var iHash1 = GetHash(new Bitmap(icon));
            var iHash2 = GetHash(new Bitmap("RF7.jpg"));

            var equalElements = iHash1.Zip(iHash2, (i, j) => i == j).Count(eq => eq);

            var percent = (double)equalElements * 100 / iHash1.Count;
            //Console.WriteLine($"Compare Percent: {percent}");
            return percent;
        }

        private static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            //Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            for (int j = 0; j < bmpSource.Height; j++)
                for (int i = 0; i < bmpSource.Width; i++)
                    lResult.Add(bmpSource.GetPixel(i, j).GetBrightness() < 0.5f);
            return lResult;
        }
        */
        static void Main(string[] args)
        {
            //Program program = new Program();
            Passport passport = new Passport();
            Bitmap imagePassport = new Bitmap("Паспорт.jpg");
            byte[] imageByte = null;
            using (MemoryStream ms = new MemoryStream())
            {
                imagePassport.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageByte = ms.ToArray();
            }

            var calculatorClient = ServiceProxy.Create<IOcrService>(new Uri("fabric:/DataServiceApplication/OcrService"));

            passport = calculatorClient.ParsePassport(imageByte).Result;

            Console.WriteLine("No exceptions");
            /*if (ImagePassport.HaveFace(imagePassport))
            {
                Console.WriteLine("Есть лицо");
            }
            else
                Console.WriteLine("Нет лица");
            if (ImagePassport.HaveLabelRF(imagePassport))
            {
                Console.WriteLine("Есть РФ");
            }
            else
                Console.WriteLine("Нет РФ");
            Console.ReadLine();*/

            //Bitmap BlackWhitePassport = (Bitmap)imagePassport.Clone();
            //FiltersSequence filtersSequence = new FiltersSequence();
            /*BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(imagePassport);
            blobCounter.ObjectsOrder = ObjectsOrder.YX;
            blobCounter.FilterBlobs = true;
            Graphics g = Graphics.FromImage(imagePassport);
            Pen redPen = new Pen(Color.Red, 2);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<AForge.IntPoint> corners;
                List<AForge.IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                Point center;
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    if (shapeChecker.CheckPolygonSubType(corners) == PolygonSubType.Rectangle)
                    {
                        List<Point> Points = new List<Point>();
                        foreach (var point in corners)
                        {
                            Points.Add(new Point(point.X, point.Y));
                        }
                        g.DrawPolygon(redPen, Points.ToArray());
                        imagePassport.Save("result.png");
                    }
                }
            }*/


            // сохраняем изображение
            //imagePassport.Save("result.jpg");
        }
    }
}
