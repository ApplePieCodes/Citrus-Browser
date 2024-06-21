using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class TextTag //Represents Text
    {
        public string text; // Text to be displayed
        public string? name; // Name to be referenced
        public int fontSize; // font size
        public FontFamily fontFamily; // Font family
        public SolidColorBrush foreground; // Foreground Color
        public SolidColorBrush background; // Background Color
        public Tools.HorizontalAlignment horizontalAlignment; // Horizontal alignment
        public TextTag(string a, string b, int c, FontFamily d, SolidColorBrush e, SolidColorBrush f, Tools.HorizontalAlignment horizontalAlignment)
        {
            text = a;
            name = b;
            fontSize = c;
            fontFamily = d;
            foreground = e;
            background = f;
            this.horizontalAlignment = horizontalAlignment;
        }
    }
}
