using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace API_Project.Models
{
    // This class For Storing Ordered Pizaa Details Document /Table
    public class OrderDetails
    {
        [BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public string Order_Id { get; set; }
        public OrderPizza OrderPizzaDetails { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderStatus { get; set; }

        public int OrderAmount { get; set; }

        public int OrderTax { get; set; }

        public int OrderSubTotal { get; set; }

        public string OrderDeliveryAgent { get; set; }

        public string OrderAgentID { get; set; }

        public string OrderAgentContact { get; set; }

    }
}
