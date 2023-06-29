using API_Project.Interface;
using API_Project.Models;
using API_Project.Models.Manager;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API_Project.DataCollection
{
    public class InsertDataToDb: IInserDataToDB
    {
        private readonly IMongoCollection<Menu> _menuData;
        private readonly IMongoCollection<Manager> _manager;
        
        private readonly IOptions<MongoDBSetting> _dbSetting;
        public InsertDataToDb(IOptions<MongoDBSetting> dbSetting)
        {
            this._dbSetting = dbSetting;
            var mongoClient = new MongoClient(_dbSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_dbSetting.Value.DatabaseName);
            _menuData = mongoDatabase.GetCollection<Menu>(_dbSetting.Value.PizzaMenu);
            _manager = mongoDatabase.GetCollection<Manager>(_dbSetting.Value.Manager);
            


        }

     
        public void InsertDataToMenu()
        {
            _menuData.DeleteMany(new BsonDocument());
            // Insert new data
            var menu = new Menu
            {
                Id = "m" + DateTime.Now.ToString("ss"),

                Pizza = new List<Pizza>
            {
                new Pizza { PizzaId = "p1", PizzaName = "Galore",PizzaPrice =110 },
                new Pizza { PizzaId = "p2", PizzaName = "Impizzable" ,PizzaPrice =100},
                new Pizza { PizzaId = "p3", PizzaName = "Melted",PizzaPrice =110},
                new Pizza { PizzaId = "p4", PizzaName = "Broadway " ,PizzaPrice =90},
                new Pizza { PizzaId = "p5", PizzaName = "Paradise",PizzaPrice =130 }
            },
                Toppings = new List<Toppings>
            {
                new Toppings { ToppingId = "t1", ToppingName = "Pepperoni",Topping=40 },
                new Toppings { ToppingId = "t2", ToppingName = "Sausage",Topping=60},
                new Toppings { ToppingId = "t3", ToppingName = "Cheese",Topping=75},
                new Toppings { ToppingId = "t4", ToppingName = "artichoke" ,Topping=35},
                new Toppings { ToppingId = "t5", ToppingName = "Taco ",Topping=40 },
              
            },
                Crusts = new List<Crust>
            {
                Crust.StuffedCrust,
                Crust.CrackerCrust,
                Crust.FlatBreadCrust,
                Crust.ThinCrust
            },
                Sizes = new List<Size>
            {
                Size.Small,
                Size.Medium,
                Size.Large
            },
                Tax = 10
            };

            _manager.DeleteMany(new BsonDocument());
            var manager = new Manager()
            {

                username ="peter",
                passward ="peter"
            };

       
            _menuData.InsertOne(menu);
            
            _manager.InsertOne(manager);

        }

        
        
    }
}
