using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CyberBot
{
    public class ascii_logo
    {
        public string GeneratedAsciiText { get; private set; } = string.Empty;
        public ascii_logo()
        {
            GenerateLogoString();
        }
        private void GenerateLogoString()
        {
            try
            {
                string runningDir = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(runningDir, "CyberBotLogo.jpg");
                if (!File.Exists(filePath))
                {
                    GeneratedAsciiText = "=== [CYBERBOT] ===\n";
                    return;
                }
                BitmapImage sourceImage = new BitmapImage();
                sourceImage.BeginInit();
                sourceImage.UriSource = new Uri(filePath, UriKind.Absolute);
                sourceImage.CacheOption = BitmapCacheOption.OnLoad;
                sourceImage.EndInit();
                int targetWidth = 55;
                int targetHeight = 22;
                ScaleTransform scale = new ScaleTransform((double)targetWidth / sourceImage.PixelWidth, (double)targetHeight / sourceImage.PixelHeight);
                TransformedBitmap resizedImage = new TransformedBitmap(sourceImage, scale);
                FormatConvertedBitmap grayImage = new FormatConvertedBitmap(resizedImage, PixelFormats.Gray8, null, 0);
                int stride = grayImage.PixelWidth;
                byte[] pixels = new byte[grayImage.PixelHeight * stride];
                grayImage.CopyPixels(pixels, stride, 0);
                StringBuilder sb = new StringBuilder();
                string asciiChars = "@#S%?*+;:,. ";
                for (int y = 0; y < grayImage.PixelHeight; y++)
                {
                    for (int x = 0; x < grayImage.PixelWidth; x++)
                    {
                        int index = y * stride + x;
                        byte grayValue = pixels[index];
                        int charIndex = (grayValue * (asciiChars.Length - 1)) / 255;
                        sb.Append(asciiChars[charIndex]);
                    }
                    sb.AppendLine();
                }
                GeneratedAsciiText = sb.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ASCII Error: {ex.Message}");
                GeneratedAsciiText = "=== [CYBERBOT] ===\n";
            }
        }
    }
}