using API_Project.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.CompilerServices;
using API_Project.Interface;
using API_Project.Models.Manager;
using API_Project.Exceptions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace API_Project.DataCollection
{
    public class MongoDbContext:IMongoDbContext
    {
        private readonly IMongoCollection<Menu> _menuData;
        private readonly IMongoCollection<LoginUser> _userData;
        private readonly IOptions<MongoDBSetting> _dbSetting;
        private readonly IMongoCollection<Manager> _manager;
        private readonly IMongoCollection<User> _customer;


        public MongoDbContext(IOptions<MongoDBSetting> dbSetting)
        {
            this._dbSetting = dbSetting;
            
          
            var mongoClient = new MongoClient(_dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_dbSetting.Value.DatabaseName);
            _menuData = mongoDatabase.GetCollection<Menu>(_dbSetting.Value.PizzaMenu);
            _userData = mongoDatabase.GetCollection<LoginUser>(_dbSetting.Value.UserData);
            _manager = mongoDatabase.GetCollection<Manager>(_dbSetting.Value.Manager);
            _customer = mongoDatabase.GetCollection<User>(_dbSetting.Value.Customer);

        }

        // Find the user details bases on email in User document
        public User GetUserDetails(string email)
        {
             User user =  _customer.Find(x => x.Email == email).FirstOrDefault(); 
            if(user == null)
            {
                throw new UserNotFound("User Not found with " + email);
            }
            else
            {
                return user;
            }
        }

        // Adding the some information of user to LoginUser Document
        public void AddUser(LoginUser user)
        {
            _userData.InsertOne(user);
        }

        //Adding New User details to Document
        public void InsetDetaislToUser(User user)
        {
             _customer.InsertOne(user);
        }

         // Find The Menu Details along with Pizza and topping 
        public ICollection<Menu> GetMenu()
        {
            return _menuData.Find(_ => true).ToList();     
        }
        // Find the user details and order details from LoginUser Document
        public LoginUser GetOrderDetailsWithUser(string User_Id)
        {
            LoginUser user = _userData.Find(a => a.User_Id == User_Id).FirstOrDefault();
            
            if (user == null)
            {
                throw new UserNotFound("UserNot found with " + User_Id) ;
                
            }
            else
            {
                return user;
            }
           
        }

        //get the MenuId 
        public Menu GetMenuID()
        {
            return _menuData.Find(_ => true).FirstOrDefault();
        }

        // add the order details to OrderList which is define in the loginUser class
        // and replace the details in Userdata Document  
        public void AddOrder(LoginUser user)
        {
            _userData.FindOneAndReplace(a => a.User_Id == user.User_Id, user);      
        }
    
        // get the All order of Of user
        public List<LoginUser> ViewAllOrder()
        {
            return _userData.Find(new BsonDocument()).ToList();
        }

        //this method is used to add the updateddetails of order
        ///which is made by manger details of order 
        public void UpdateTheOrderStatus(LoginUser loginUser)
        {
             _userData.FindOneAndReplace(x => x.User_Id == loginUser.User_Id,loginUser);
        }


        //to get the manager details based on username
        public Manager GetManagerDetails(string username )
        {
            return _manager.Find(x => x.username == username).FirstOrDefault();
        }


    }
}
