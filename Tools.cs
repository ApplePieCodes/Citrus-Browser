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
        public static SolidColorBrush ParseColor(string colorName) //Parses a String to a Color. I don't Know how it works cuz i got it online so if someone could document this and submit a pull request that would be great.
        {
            PropertyInfo colorProperty = typeof(Colors).GetProperty(colorName, BindingFlags.Public | BindingFlags.Static);
            if (colorProperty != null)
            {
                Color color = (Color)colorProperty.GetValue(null);
                return new SolidColorBrush(color);
            }
            return null;
        }
        public enum HorizontalAlignment //Horizontal Alignment ENUM
        {
            Left,
            Center,
            Right,
        }
        public static BitmapImage byteArrayToImage(byte[] bytes) // Process a Byte Array to a JPEG Image
        {
            BitmapImage bitmapImage = new BitmapImage(); // Make A New Image

            using (MemoryStream stream = new MemoryStream(bytes)) //Process the image
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage; //Return the image
        }
    }
}
