
namespace API_Project.Services
{
    //For JWT Token
    public interface IJwtToken
    {
        //create JWT Token
        public string CreateToken(string email,string role);
      
    }
}
