using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace API_Project.Models
{
    // This class For Create PizzaOrder Document /Table
    public class OrderPizza
    {
        [Key]
        [Required]
        public string Pizza_Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Size Size { get; set; }

        [Required]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Crust Crust { get; set; }

        [Required]
        public List<string> Topping_Id { get; set; }
    }
}
