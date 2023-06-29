using API_Project.Models;
using API_Project.Models.Manager;
using Microsoft.EntityFrameworkCore;

namespace API_Project.DataCollection
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext>dbContextOptions) : base(dbContextOptions)
        {

        }

        // creating table in database
        public DbSet<User> Customer { get; set; }
       // public DbSet<Manager> Manager { get; set; }

        

    }
}
