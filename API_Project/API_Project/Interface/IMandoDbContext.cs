using API_Project.Models;
using API_Project.Models.Manager;
using MongoDB.Driver;

namespace API_Project.Interface
{
    public interface IMongoDbContext
    {

        //for registration 
        public User GetUserDetails(string email);
        public void AddUser(LoginUser user);
        public void InsetDetaislToUser(User user);


        //for getMenu
        public ICollection<Menu> GetMenu();

       
        //For createOrder
        public LoginUser GetOrderDetailsWithUser(string UserId);

        public Menu GetMenuID();

        public void AddOrder(LoginUser user);


        public List<LoginUser> ViewAllOrder();

        // public void InsertDataToMenu();

        public void UpdateTheOrderStatus(LoginUser loginUser);

        public Manager GetManagerDetails(string username);

        
        
           


        
    }
}
