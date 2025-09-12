using common_class_library_projects.CommonFields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_class_library_projects.Dto.UserManagements
{
    public class UserDto: ConfigurationFields
    {     
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
