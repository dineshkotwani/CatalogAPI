using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Models
{
    

    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "productname")]
        public string ProductName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "subcategoryid")]
        public string SubCategoryId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}