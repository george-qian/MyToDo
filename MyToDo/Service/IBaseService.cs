using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Shared;
using MyToDo.Shared.Parameters;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public interface IBaseService<T>
    {
        Task<ApiResponse> GetAllAsync(QueryParameter parameter);
        Task<ApiResponse<T>> GetSingleAsync(int id);
        Task<ApiResponse<T>> AddAsync(T model);
        Task<ApiResponse<T>> UpdateAsync(T model);
        Task<ApiResponse> DeleteAsync(int id);
    }
}
