using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    // This class For Create Menu Document /Table
    public class Menu
    {
        [BsonId]
       // [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<Pizza> Pizza { get; set; }
        public List<Toppings> Toppings { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Crust> Crusts { get; set; }

        [BsonRepresentation(BsonType.String)]
        public List<Size> Sizes { get; set; }

        public int Tax { get; set; }

        
    }

}
