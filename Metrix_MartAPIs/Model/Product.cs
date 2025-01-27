using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metrix_MartAPIs.Model
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [JsonProperty("ProductId")]
        public string ProductId { get; set; }

        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("QuantityPerUnit")]
        public int QuantityPerUnit { get; set; }

        [JsonProperty("UnitsInStock")]
        public int UnitsInStock {  get; set; }

        [JsonProperty("ProductPrice")]
        public decimal ProductPrice { get; set; }
    }
}
