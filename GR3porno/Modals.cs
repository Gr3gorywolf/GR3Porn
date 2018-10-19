using System;
using System.Collections.Generic;
using System.Text;

namespace xvideos_downloader
{
   public class Modals
    {
    public class pagedata
    {
        public int navigationmax { get; set; }
        public List<videosmodels> videos { get; set; }

    }
    public class videosmodels
    {
        public string title { get; set; }
        public string link { get; set; }
        public string thumb { get; set; }
        public string duration { get; set; }
    }
}
}
