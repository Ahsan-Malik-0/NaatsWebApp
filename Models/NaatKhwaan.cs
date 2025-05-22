using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NaatsWebApp.Models
{
	public class NaatKhwaan
	{
        public int id { get; set; }
        [Required]
        public string fullname { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public char gender { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string pwd { get; set; }
    }
}