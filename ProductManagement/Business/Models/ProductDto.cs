using System.Text.Json.Serialization;

namespace Business.Models
{
    public class ProductDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
