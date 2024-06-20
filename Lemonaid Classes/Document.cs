﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Citrus_Browser.Lemoaid_Classes
{
    public class Document
    {
        public InfoHeader info;
        public Page content;
        public Document(InfoHeader header, Page webPage)
        {
            info = header;
            content = webPage;
        }
    }
}
