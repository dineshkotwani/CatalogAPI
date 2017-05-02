using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatalogWeb.Models
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string SubCategoryId { get; set; }

    }
}