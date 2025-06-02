using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NaatsWebApp.Models
{
	public class Naats
	{
        public string nid { get; set; }
        public int ano { get; set; }
        public int nno { get; set; }
        public string title { get; set; }
        public string audiopath { get; set; }
        public HttpPostedFileBase audiofile { get; set; }
    }
}