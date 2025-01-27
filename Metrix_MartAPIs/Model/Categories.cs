using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Metrix_MartAPIs.Model
{
    public class Categories
    {
        [Key]
        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
        //public ICollection<Product> Products { get; set; }
    }
}
