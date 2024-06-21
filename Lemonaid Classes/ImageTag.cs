using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Citrus_Browser.Lemonaid_Classes
{
    public class ImageTag //Represents an Image
    {
        public string name; //Name to be referenced
        public BitmapImage image; //Image
        public double opacity; // Opacity
        public double width; //Image Width
        public double height; // Image Height
        public Tools.HorizontalAlignment horizontalAlignment; // Horizontal Alignment
        public double CornerRadius; // Corner Radius

        public ImageTag(string nam, BitmapImage img, double opa, double wid, double hei, Tools.HorizontalAlignment hor, double rad)
        {
            name = nam;
            image = img;
            opacity = opa;
            width = wid;
            height = hei;
            horizontalAlignment = hor;
            CornerRadius = rad;
        }
    }
}
