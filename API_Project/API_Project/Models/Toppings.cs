using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    // This class For Create topping Document /Table
    public class Toppings
    {
        [Key]
        [BsonId]
        [Required]
        public string ToppingId { get; set; }
        [Required]
        public string ToppingName { get; set; }

        [Required]
        public int Topping { get; set; }
    }

}

