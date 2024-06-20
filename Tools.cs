using System;
using System.Reflection;
using System.Windows.Media;

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
    }
}
