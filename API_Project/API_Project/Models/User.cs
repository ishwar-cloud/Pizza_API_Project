using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace API_Project.Models
{
    // This class For Create User Document /Table
    public class User
    {

        //[Key] //Priimary Key in database for user table
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [BsonId]
        public string User_Id { get; set; }
        
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]

        public string ContactNo { get; set; }

        [Required]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        
    }
    //public class Jwt
    //{
    //    public string key { get; set; }
    //    public string Issuer { get; set; }
    //    public string Audience { get; set; }
    //    public string Subject { get; set; }
    //}
}
