using System;
using System.Collections.Generic;
using System.Drawing;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using AForge.Math.Geometry;

namespace CheckImage
{
    public static class ImagePassport
    {
        public static bool HaveFace(Bitmap imagePassport)
        {
            Rectangle cloneRect = new Rectangle((int)(imagePassport.Width * 0.05), (int)(imagePassport.Height / 2.0 + imagePassport.Width * 0.129), (int)(imagePassport.Width * 0.28), (int)(imagePassport.Width * 0.357));
            Bitmap cloneBitmap = imagePassport.Clone(cloneRect, imagePassport.PixelFormat);
            HaarObjectDetector faceDetector = new HaarObjectDetector(new FaceHaarCascade(), minSize: 70, searchMode: ObjectDetectorSearchMode.Average);
            // распознаём лица
            IEnumerable<Rectangle> face = faceDetector.ProcessFrame(cloneBitmap);

            return face.Count() > 0;
        }

        public static bool HaveLabelRF(Bitmap imagePassport)
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
                    Bitmap RF2 = AForge.Imaging.Filters.Grayscale.CommonAlgorithms.BT709.Apply(RF);
                    Bitmap RF3 = threshold.Apply(RF2);

                    if (CompareLabel(RF3) > 70.0)
                        haveLabel = true;
                }
            }
            return haveLabel;
        }

        private static double CompareLabel(Bitmap icon)
        {
            var iHash1 = GetHash(new Bitmap(icon));
            
            var iHash2 = GetHash(new Bitmap(Properties.Resources.RF7));
            
            var equalElements = iHash1.Zip(iHash2, (i, j) => i == j).Count(eq => eq);

            var percent = (double)equalElements * 100 / iHash1.Count;
            return percent;
        }

        private static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            for (int j = 0; j < bmpSource.Height; j++)
                for (int i = 0; i < bmpSource.Width; i++)
                    lResult.Add(bmpSource.GetPixel(i, j).GetBrightness() < 0.5f);
            return lResult;
        }
    }
}
