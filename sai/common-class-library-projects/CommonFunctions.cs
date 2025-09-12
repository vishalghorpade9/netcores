using common_class_library_projects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace common_class_library_projects
{
    public static class CommonFunctions
    {
        public static MESResponse UpdateMESResponseWithException(MESResponse response, Exception ex)
        {
            response.IsSuccess = false;
            response.IsError = true;
            response.ErrorMessage = "Message - "+ex.Message+ ", InnerException - " + ex.InnerException;
            response.ResponseMessage = ResponseMessages.ReadError;
            return response;
        }

        public static MESResponse UpdateMESResponseForDuplicateRecord(MESResponse response, string columnName)
        {
            response.IsSuccess = false;
            response.IsError = true;            
            response.ResponseMessage = ResponseMessages.DuplicateError;
            response.Data = columnName;
            return response;
        }
    }
}
