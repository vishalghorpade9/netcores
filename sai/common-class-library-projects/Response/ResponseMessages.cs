using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_class_library_projects.Response
{
    public static class ResponseMessages
    {
        public static string ReadSuccess = "ReadSuccess";
        public static string AddSuccess = "AddSuccess";
        public static string EditSuccess = "EditSuccess";
        public static string DeleteSuccess = "DeleteSuccess";

        public static string ReadError = "ReadError";
        public static string AddError = "AddError";
        public static string EditError = "EditError";
        public static string DeleteError = "DeleteError";

        public static string DuplicateError = "DuplicateError";
    }
}
