using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class Page //Represents a page. Contains an ArrayList of tags
    {
        public ArrayList tags { get; set; }
        public Page()
        {
            tags  = new ArrayList();
        }
    }
}
