using API_Project.Models;

namespace API_Project.Interface
{
    // This Interface is used for Manager
    public interface IManagerService
    {
        //for manager login
        public string ManagerLogin(string username, string password);
        
        //for View all order
        public List<OrderDetails> ViewAllOrder();
        
        //for manger the order status
        public void ManageOrder(string order_id, string orderStatus);
        
        //for view all user order details
        public OrderDetails OrderDetails(string order_id);

    }
}
