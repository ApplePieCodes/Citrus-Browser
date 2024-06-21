using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class InfoHeader //Represents the Info Header.
    {
        public string name; //Page Title
        public int version; //page Version
        public string author; //Author Name
        public string language; //Page Language
        public Brush background; //Background Color
        public InfoHeader(string a, int b, string c, string d, Brush f)
        {
            name = a;
            version = b;
            author = c;
            language = d;
            background = f;
        }
    }
}
