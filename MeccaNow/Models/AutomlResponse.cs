using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeccaNow.Models
{
    public class AutomlResponse
    {
        public List<Li> payload { get; set; }
    }

    public class Li
    {
        public classification classification { get; set; }
        public string displayName { get; set; }
    }

    public class classification
    {
        public decimal score { get; set; }
    }
}