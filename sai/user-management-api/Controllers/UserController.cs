using AutoMapper;
using common_class_library_projects;
using common_class_library_projects.Dto.UserManagements;
using common_class_library_projects.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using user_management_api.Data;
using user_management_api.Model;
using user_management_api.Repositories;

namespace user_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManagementDbContext context;
        private readonly IMapper mapper;
        private UnitOfWork unitOfWork;
        private MESResponse mesResponse;

        public UserController(UserManagementDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            unitOfWork = new UnitOfWork(context);
            mesResponse = new MESResponse();
        }

        [HttpGet]
        public IActionResult GetList()
        {
            try
            {

                //                var duplicateExists = unitOfWork.UserRepository.HasDuplicates(new Dictionary<string, object>
                //{
                //    { "UserName", "vishal" }
                //}, new Dictionary<string, object>());

                //                var duplicateExists1 = unitOfWork.UserRepository.HasDuplicates(new Dictionary<string, object>
                //{
                //    { "UserName", "vishal" }    
                //}, new Dictionary<string, object>
                //{    
                //    { "UserId", 2 }
                //});

                var users = unitOfWork.UserRepository.Get();
                var userDto = new List<UserDto>();
                foreach (User user in users)
                {
                    userDto.Add(mapper.Map<UserDto>(user));
                }
                mesResponse.Data = JsonSerializer.Serialize(userDto);
                mesResponse.ResponseMessage = ResponseMessages.ReadSuccess;
            }
            catch (Exception ex)
            {
                mesResponse = CommonFunctions.UpdateMESResponseWithException(mesResponse, ex);
            }
            return Ok(mesResponse);
        }

        [HttpPost]
        public IActionResult Create(UserDto userDto)
        {
            try
            {
                User user = mapper.Map<User>(userDto);
                // process to check duplicate records
                var duplicateExists = unitOfWork.UserRepository.HasDuplicates(new Dictionary<string, object>
                {
                    { "UserName", user.UserName }
                }, new Dictionary<string, object>());

                if (duplicateExists)
                {
                    mesResponse = CommonFunctions.UpdateMESResponseForDuplicateRecord(mesResponse, "UserName");
                }
                else
                {
                    unitOfWork.UserRepository.Insert(user);
                    unitOfWork.Save();
                    var useDtoRes = mapper.Map<UserDto>(user);
                    mesResponse.Data = JsonSerializer.Serialize(useDtoRes);
                    mesResponse.ResponseMessage = ResponseMessages.AddSuccess;
                }
            }
            catch (Exception ex)
            {
                mesResponse = CommonFunctions.UpdateMESResponseWithException(mesResponse, ex);
            }
            return Ok(mesResponse);
        }

        //[HttpPut]
        //public IActionResult Update(UserDto userDto)
        //{
        //    try
        //    {
        //        User user = mapper.Map<User>(userDto);
        //        // process to check duplicate records
        //        var duplicateExists = unitOfWork.UserRepository.HasDuplicates(new Dictionary<string, object>
        //        {
        //            { "UserName", user.UserName }
        //        }, new Dictionary<string, object>());

        //        if (duplicateExists)
        //        {
        //            mesResponse = CommonFunctions.UpdateMESResponseForDuplicateRecord(mesResponse, "UserName");
        //        }
        //        else
        //        {
        //            unitOfWork.UserRepository.Insert(user);
        //            unitOfWork.Save();
        //            var useDtoRes = mapper.Map<UserDto>(user);
        //            mesResponse.Data = JsonSerializer.Serialize(useDtoRes);
        //            mesResponse.ResponseMessage = ResponseMessages.AddSuccess;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        mesResponse = CommonFunctions.UpdateMESResponseWithException(mesResponse, ex);
        //    }
        //    return Ok(mesResponse);
        //}
    }
}
