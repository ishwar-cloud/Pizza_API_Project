using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace API_Project.Models
{
    // This class For Create Pizza Document /Table
    public class Pizza
    {
        [Key]
        [BsonId]
        [Required]
        public string PizzaId { get; set; }
        [Required]
        public string PizzaName { get; set; }

        [Required]
        public int PizzaPrice { get; set; }



    }
}
