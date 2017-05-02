using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    

    public class SubCategory
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "subcategoryname")]
        public string SubCategoryName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "categoryid")]
        public string CategoryId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }


    }
}