using MyToDo.Shared;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(UserDto user);
        Task<ApiResponse> Register(UserDto user);
    }
}
