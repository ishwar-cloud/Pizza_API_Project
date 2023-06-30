using API_Project.Exceptions;
using API_Project.Interface;
using API_Project.Models;

namespace API_Project.Services
{
    // to implementing IUserService interface in UserService class
    public class UserService:IUserService
    {
        private static string userId; // To store Login user Id
       
        
        private readonly IMongoDbContext _mongoDbContext;
        
        private readonly IJwtToken _jwtToken;

        // constractor 
        public UserService(IMongoDbContext mongoRepository,IJwtToken jwtToken)
        {
           
            this._mongoDbContext = mongoRepository;
            this._jwtToken = jwtToken;
        }

        /// <summary>
        /// user For new user Registration
        /// taking User object as parameter for rgistration
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="UserAlreadyExist"></exception>

        public bool Register(User user)
        {
            // check user is Aready exits or not
            
            User customer = _mongoDbContext.GetUserDetails(user.Email);

            // if customer is null then throw Exception
            if (customer != null)
            {
                throw new UserAlreadyExist("User Already Exist with given Email Id " + user.Email);
            }

            // Adding new user in table
            var user1 = new User()
            {

                User_Id = "u" + DateTime.Now.ToString("ss"),
                Email = user.Email,
                Name = user.Name,
                ContactNo = user.ContactNo,
                Password = user.Password,
             

            };

            //adding login details to LoginUser Table
            _mongoDbContext.AddUser(new LoginUser()
            {
                User_Id = user1.User_Id,
                Email = user.Email,
                Orders = new List<OrderDetails>()
            });

            

            _mongoDbContext.InsetDetaislToUser(user1);

            return true;
        }

        /// <summary>
        /// user for login the user 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFound"></exception>
        /// <exception cref="IncorrectCredential"></exception>
        public string Login(string email,string password)
        {
            // check user email_id  is exist or not in the  table
           

            User customer = _mongoDbContext.GetUserDetails(email);
            string token;

            // check customer is null or not
            if (customer == null)
            {
                throw new UserNotFound("User Not Found With Given Email: " + email);
            }

            else if(email == customer.Email && password == customer.Password)
            {

                 UserService.userId = customer.User_Id; 
                // assigning userid value to global variable 

                // call CreateToken method and assiging this token to token variable
                token =_jwtToken.CreateToken(customer.Email, "user");
               
                return token;
            }

            else
            {
                throw new IncorrectCredential("Incorrect Credential ");
            }
            
        }
        
        /// <summary>
        /// used for get the passwaord from database 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFound"></exception>
        public string ForgetPassword(string email)
        {
            // check user email_id  is exist or not in the  table
            User customer = _mongoDbContext.GetUserDetails(email);

            //ckeck custome is null or not
            if (customer != null)
            {
                return customer.Password;
            }

            throw new UserNotFound("given Email id doesnot exist");
        }

        // to view the menu 
        public IEnumerable<Menu> ViewMenu()
        { 

            
             return _mongoDbContext.GetMenu();
             
           
        }


        /// <summary>
        /// This Method use for Creating The 
        /// Taking  OrderPizza Class object "order"  as parameter
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NullPointerException"></exception>
        /// <exception cref="MenuNotFound"></exception>
        
        public string CreateOrder(OrderPizza order)
        {

            if (order == null)
            {
                throw new ArgumentException("Invalid input body. Order details are missing.");
            }


            // To get the Details of Logied user 
             LoginUser user = _mongoDbContext.GetOrderDetailsWithUser(userId);
            
            if(user == null)
            {
                throw new NullPointerException("$\"User with ID {userId} not found.\"");
            }

            Menu menu = _mongoDbContext.GetMenuID();
            Models.Pizza pizzaID = menu.Pizza.Where(x => x.PizzaId == order.Pizza_Id).FirstOrDefault();

            if(pizzaID == null)
            {
                throw new MenuNotFound("Not Avalialabel");
            }
            

            var amount = (100 * order.Quantity)
                    + (order.Size == Size.Small ? 50 : (order.Size == Size.Medium ? 100 : 150))
                    + (order.Topping_Id.Count * 50);



            // create ordear details 
            var OrderDetails = new OrderDetails()
            {
                Order_Id = "O" + DateTime.Now.ToString("ss"),
                OrderPizzaDetails = order,
                OrderDate = DateTime.Now,
                OrderStatus = "Accepted",
                OrderAmount = amount,
                OrderTax = 10,
                OrderSubTotal = amount + 25,
                OrderDeliveryAgent = "Ishwar",
                OrderAgentID = "Ishwar_01",
                OrderAgentContact = "9130322039"
            };


            // adding order Details to OrderList in LoginUser document
            user.Orders.Add(OrderDetails);
            
            //call AddOrder method 
            _mongoDbContext.AddOrder(user);
          
            return OrderDetails.Order_Id;
        }

        /// <summary>
        /// this methos for track the order status 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NullPointerException"></exception>
        public ICollection<OrderDetails> TrackOrder(string orderId)
        {
            // fetcing details from LoginUser documnet

            LoginUser user = _mongoDbContext.GetOrderDetailsWithUser(userId);
           

            OrderDetails id = user.Orders.Where(x => x.Order_Id == orderId).FirstOrDefault();

            if ( id == null)
            {
                throw new NullPointerException("Ordernot found with "+ orderId);
            }
            
            // creating new list to store orderDetails 
            var details = new List<OrderDetails>();

            details.Add(id);
            return details; 
        }

        /// <summary>
        /// this method used for ViewOrderHistory 
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="NullPointerException"></exception>
        public ICollection<OrderDetails> ViewOrderHistory(string orderId)
        {
            // fetcing details from LoginUser documnet
            LoginUser user = _mongoDbContext.GetOrderDetailsWithUser(userId);
           
            OrderDetails id = user.Orders.Where(x => x.Order_Id == orderId).FirstOrDefault();

            // creating new list to store orderDetails 
            var details = new List<OrderDetails>();

            
                if (id == null)
                {
                    throw new NullPointerException("Ordernot found with "+ orderId);
                }
            // store all details of order 
            foreach (var order in user.Orders)
            { 
                details.Add(order);
            }
            return details;

        }

        
    }
}
