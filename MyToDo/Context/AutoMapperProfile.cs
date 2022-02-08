using AutoMapper;
using MyToDo.Shared.Dtos;

namespace MyToDo.Context
{
    public class AutoMapperProfile:MapperConfigurationExpression
    {
        public AutoMapperProfile()
        {
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<User,UserDto>().ReverseMap();
        }
    }
}
