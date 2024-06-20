using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class InfoHeader
    {
        public string name;
        public int version;
        public string author;
        public string language;
        public SolidColorBrush background;
        public InfoHeader(string a, int b, string c, string d, SolidColorBrush f)
        {
            name = a;
            version = b;
            author = c;
            language = d;
            background = f;
        }
    }
}
