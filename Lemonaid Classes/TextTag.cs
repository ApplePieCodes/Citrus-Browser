using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class TextTag
    {
        public string text;
        public string name;
        public int fontSize;
        public FontFamily fontFamily;
        public SolidColorBrush foreground;
        public SolidColorBrush background;
        public Tools.HorizontalAlignment horizontalAlignment;
        public TextTag(string a, string b, int c, FontFamily d, SolidColorBrush e, SolidColorBrush f, Tools.HorizontalAlignment horizontalAlignment)
        {
            text = a;
            name = b ?? string.Empty;
            fontSize = c;
            fontFamily = d;
            foreground = e;
            background = f;
            this.horizontalAlignment = horizontalAlignment;
        }
    }
}
