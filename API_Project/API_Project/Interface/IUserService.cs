using API_Project.Models;
using System.Data.SqlTypes;
using System.Net.NetworkInformation;

namespace API_Project.Interface
{
    // This Interface is used for user
    public interface IUserService
    {
        //for registration
        public bool Register(User user);

        // for login into account
        public string Login(string emial, string password);

        //Logout 
        public string ForgetPassword(string email);

        

        //  for ViewMenu 
        public IEnumerable<Menu> ViewMenu();

        //public void AddToMenu(Menu menu);

        //For Create Order
        public string CreateOrder(OrderPizza order);

        //For Track the Order Status
        public ICollection<OrderDetails> TrackOrder(string orderId);

        //for View Order History
        public ICollection<OrderDetails> ViewOrderHistory(string orderId);

    }
}
