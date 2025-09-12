using AutoMapper;
using common_class_library_projects.Dto.UserManagements;
using user_management_api.Model;

namespace user_management_api.Mapping
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
