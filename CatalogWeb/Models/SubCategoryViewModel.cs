using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CatalogWeb.Models
{
    public class SubCategoryViewModel
    {
        public string Id { get; set; }

        [Required]
        public string SubCategoryName { get; set; }

        [Required]
        public string CategoryId { get; set; }

    }
}