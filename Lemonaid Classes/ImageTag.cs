using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Citrus_Browser.Lemonaid_Classes
{
    public class ImageTag
    {
        public string name;
        public BitmapImage image;
        public double opacity;
        public double width;
        public double height;
        public Tools.HorizontalAlignment horizontalAlignment;
        public double CornerRadius;

        public ImageTag(string nam, BitmapImage img, double opa, double wid, double hei, Tools.HorizontalAlignment horizontalAlignment, double radius)
        {
            name = nam;
            image = img;
            opacity = opa;
            width = wid;
            height = hei;
            this.horizontalAlignment = horizontalAlignment;
            CornerRadius = radius;
        }
    }
}
