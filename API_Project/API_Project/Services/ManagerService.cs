using API_Project.DataCollection;
using API_Project.Exceptions;
using API_Project.Interface;
using API_Project.Models;
using API_Project.Models.Manager;
using Jose;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using MongoDB.Driver;

namespace API_Project.Services
{
    // to implementing IManagerService Interface in ManagerService class
    public class ManagerService:IManagerService
    {
       
        private readonly IMongoDbContext _mongoDbContext;
        private readonly IJwtToken _jwtToken;
        public ManagerService(IJwtToken jwtToken, IMongoDbContext mongoDbContext)
        {
            this._mongoDbContext = mongoDbContext;
            this._jwtToken = jwtToken;
           
        }

       // this method for used to login the manger 

        public string ManagerLogin(string username, string password)
        {
            // check manager usermane  is exist or not in the  table
              
             Manager  manager = _mongoDbContext.GetManagerDetails(username);

            string token;

            // check manager is null or not
            if (manager  == null)
            {
                throw new UserNotFound("Username Not found");
            }

            else if (username == manager.username && password == manager.passward)
            {
                // call CreateToken method and assiging this token to token variable
                token = _jwtToken.CreateToken(manager.username, "manager");
                return token;
            }


            else
            {
                throw new IncorrectCredential("Incorrect Credential ");
            }
        }

        // this methos used for View all order 
        public List<OrderDetails> ViewAllOrder()
        {
            //to store the details of order
            List<OrderDetails> orders = new List<OrderDetails>();
            
            // fetch all orders and store in allOrder
            List<LoginUser> allOrder  = _mongoDbContext.ViewAllOrder();

            foreach(var item  in allOrder)
            {
                foreach(var item1 in item.Orders)
                {
                    orders.Add(item1);
                }
            }
            return orders;
        }
   
        // To manage the order and its status
        // takin order_id and order_status as parameter
        public void ManageOrder(string order_id, string orderStatus)
        {
            // fetch all orders and store in allOrder
            List<LoginUser> allOrder = _mongoDbContext.ViewAllOrder();

            bool flag = false;
            foreach (var user in allOrder)
            {
                foreach(var item in user.Orders)
                {
                    if(item.Order_Id == order_id)
                    {
                        item.OrderStatus = orderStatus;

                        //call AddDetailsToUser method and
                        // update the order status

                        _mongoDbContext.UpdateTheOrderStatus(user);
                        flag = true;
                        return;
                    }
                }
            }
            //check order  status successfully updated or not 
            if (!flag)
            {
                throw new OrderNotFound("Order Not found with given id"+order_id);
            }

        }

        // used for to get all OrderDetails 
        public OrderDetails OrderDetails(string order_id)
        {

            List<LoginUser> allOrder = _mongoDbContext.ViewAllOrder();
            OrderDetails details = null;
    
            foreach(var user in allOrder)
            {
                foreach(var item in user.Orders)
                {
                    if (item.Order_Id == order_id)
                    {
                        details = item;
                        break;
                    }
                }
            }
            //check orderDetails is addded in the details 
            if(details != null)
            {
                return details;
            }
            else
            {
                throw new OrderNotFound("Order not found with given id" + order_id);
            }
        }
    }  
}
