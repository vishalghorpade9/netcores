using common_class_library_projects.CommonFields;
using System.ComponentModel.DataAnnotations;

namespace user_management_api.Model
{
    public class User: ConfigurationFields
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(500)]
        public string Password { get; set; }
    }
}
