using AutoMapper;
using DemoMultiApp.Data.Model;
using DemoMultiApp.Data.ViewModel.User;

namespace DemoMultiApp.API.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserPostViewModel, UserModel>();
        }
    }
}
