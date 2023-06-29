using MongoDB.Bson.Serialization.Attributes;

namespace API_Project.Models
{
    // This class used for Storing detasils Of Login User to access Order details
    public class LoginUser
    {
        [BsonId]
        public string User_Id { get; set; }
        public String Email { get; set; }
        public List<OrderDetails> Orders { get; set; } = new List<OrderDetails>();

    }
}
