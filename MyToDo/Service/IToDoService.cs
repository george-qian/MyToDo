using MyToDo.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IToDoService:IBaseService<ToDoDto>
    {
        Task<ApiResponse> GetAllAsync(ToDoParameter parameter);

        Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}
