using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatalogWeb.Models
{
    public class CategoryViewModel
    {
        public string Id { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}