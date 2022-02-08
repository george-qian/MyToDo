using MyToDo.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface ILoginService
    {
        Task<ApiResponse<User>> LoginAsync(UserDto user);
        Task<ApiResponse<User>> RegisterAsync(UserDto user);
    }
}
