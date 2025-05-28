using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaatsWebApp.Models
{
    public class Album
    {
        public string nid { get; set; }
        [Display(Name = "Album No")]
        public string ano { get; set; }
        [Required]
        [Display(Name = "Album title")]
        public string title { get; set; }
        [Display(Name = "Release year")]
        public string year { get; set; }
        public string imgPath { get; set; }
        [Display(Name = "Choose Cover Photo")]
        public HttpPostedFileBase imgfile { get; set; }
    }
}