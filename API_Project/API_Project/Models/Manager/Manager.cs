using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace API_Project.Models.Manager
{
    public class Manager
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string passward { get; set; }
    }
}
