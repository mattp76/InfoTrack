using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfoTrack.Seo.Web.Models
{
    public class SearchModel
    {
        [Required]
        public string Term { get; set; }

        [Required]
        public string Url { get; set; }

        public List<int> Positions { get; set; }
    }
}