using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Citrus_Browser
{
    public class Tools
    {
        public static SolidColorBrush ParseColor(string colorName)
        {
            PropertyInfo colorProperty = typeof(Colors).GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);
            if (colorProperty != null)
            {
                Color color = (Color)colorProperty.GetValue(null);
                return new SolidColorBrush(color);
            }
            return null;
        }
        public enum HorizontalAlignment
        {
            Left,
            Center,
            Right,
        }
        public static BitmapImage byteArrayToImage(byte[] bytes)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
    }
}
