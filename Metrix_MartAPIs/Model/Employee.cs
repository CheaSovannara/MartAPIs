using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metrix_MartAPIs.Model
{
    [Table ("Employees")]
    public class Employee
    {
        [Key]
        [JsonProperty("Emp_Id")]
        public string Emp_Id { get; set; }

        [JsonProperty("Lastname")]
        public string LastName { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("HireDate")]
        public DateOnly HireDate { get; set; }

        [JsonProperty("Username")]
        public string UserName { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }
        //public string Message { get; internal set; }
    }
}
