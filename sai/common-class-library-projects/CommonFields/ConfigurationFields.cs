using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_class_library_projects.CommonFields
{
    public class ConfigurationFields: TransferredFields
    {        
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn  { get; set; } = DateTime.Now;

    }
}
