using Alkaid.Core.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace Alkaid.WPF {
    public static class Utility {
        public static Bitmap RawImageToBitmap(RawImage image) {

            int Width = image.Width;
            int Height = image.Height;
            Bitmap bmp = new(Width, Height);

            Rectangle rect = new(0, 0, Width, Height);

            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int numPixels = Width * Height;
            byte[] RGBA = new byte[numPixels * 4];

            Parallel.For(0, numPixels, i => {
                int index = i * 4;
                uint pixel = image.Pixels[i];
                RGBA[index + 0] = (byte)(pixel >> 0 & 0xFF);  // Blue
                RGBA[index + 1] = (byte)(pixel >> 8 & 0xFF);  // Green
                RGBA[index + 2] = (byte)(pixel >> 16 & 0xFF);  // Red
                RGBA[index + 3] = (byte)(pixel >> 24 & 0xFF);  // Alpha
            });
            IntPtr ptr = bmpData.Scan0;
            Marshal.Copy(RGBA, 0, ptr, numPixels * 4);
            bmp.UnlockBits(bmpData);

            return bmp;
        }
        public static BitmapImage BitmapToImageSource(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
        public static void UpdateImageBox(Image imgBox, Bitmap bitmap) {
            imgBox.Source = BitmapToImageSource(bitmap);
        }
    }
}
