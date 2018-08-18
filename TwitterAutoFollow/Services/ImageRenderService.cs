using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;

namespace TwitterAutoFollow.Services
{
    public static class ImageRenderService
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static System.Drawing.Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }
        private static Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new Bitmap(bounds.Width, bounds.Height);
            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
            }
            return result;
        }
        private static Bitmap ConvertToFormat(this System.Drawing.Image image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }
        public static List<System.Drawing.Point> TranslateImageToCoordinates(System.Drawing.Image image)
        {

            //List<PixelPoint> pixelPoints = new List<PixelPoint>();
            List<Rectangle> results = new List<Rectangle>();
            List<System.Drawing.Point> points = new List<System.Drawing.Point>();

            Bitmap followImage = ConvertToFormat(Properties.Resources.followImage, PixelFormat.Format24bppRgb);

            Bitmap bitmap = ConvertToFormat(image, PixelFormat.Format24bppRgb);
            int multiplier = 3;
            followImage = ConvertToFormat(new Bitmap(followImage, new Size(followImage.Width / multiplier, followImage.Height / multiplier)), PixelFormat.Format24bppRgb);
            bitmap = ConvertToFormat(new Bitmap(bitmap, new Size(bitmap.Width / multiplier, bitmap.Height / multiplier)), PixelFormat.Format24bppRgb);

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.951f);

            TemplateMatch[] matchings = tm.ProcessImage(bitmap, followImage);

            BitmapData data = bitmap.LockBits(
                 new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                 ImageLockMode.ReadWrite, image.PixelFormat);
            foreach (TemplateMatch m in matchings)
            {
                results.Add(m.Rectangle);
            }
            bitmap.UnlockBits(data);
            foreach(var item in results)
            {
                int centerX = item.X + 10;
                int centerY = item.Y + 5;
                points.Add(new System.Drawing.Point(centerX * multiplier, centerY * multiplier));
            }
            return points;
        }
   
        private static void DrawRectanglesOnImage(System.Drawing.Image background, List<Rectangle> rectangles)
        {
            //For Testing.
            using (Graphics g = Graphics.FromImage(background))
            {
                foreach(var rect in rectangles)
                {
                    Rectangle r = rect;
                    g.DrawRectangle(Pens.Red, r);
                }
            }
        }
    }
}
