using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_class_library_projects.Response
{
    public class MESResponse
    {
        public bool IsSuccess { get; set; } = true;
        public bool IsError { get; set; } = false;
        public bool IsWarning { get; set; } = false;
        public string ErrorMessage{ get; set; }
        public string  ResponseMessage { get; set; }
        public object Data { get; set; }
        public MESResponse()
        {
        }
    }
}
