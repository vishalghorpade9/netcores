using System.ComponentModel.DataAnnotations;

namespace AuthenticationWebApplication.Models
{
    public class ShopUser
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
    }
}
