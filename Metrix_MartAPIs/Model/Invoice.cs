using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Metrix_MartAPIs.Model
{
    public class Invoice
    {
        [Key]
        [JsonProperty("Inv_Id")]
        public string InvId { get; set; }

        [JsonProperty("Emp_Id")]
        public Employee EmpId { get; set; }

        
        [JsonProperty("ProductId")]
        public required Product ProductId { get; set; }

        [JsonProperty("Quantity")]
        public int Quantity { get; set; }

        [JsonProperty("TotalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
