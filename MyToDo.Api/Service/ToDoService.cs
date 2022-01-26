using Arch.EntityFrameworkCore.UnitOfWork;
using MyToDo.Api.Context;

namespace MyToDo.Api.Service
{
    /// <summary>
    /// 待办事项
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;

        public ToDoService(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<ApiResponse> AddAsync(ToDo entity)
        {
            try
            {
                await work.GetRepository<ToDo>().InsertAsync(entity);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, entity);
                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(id));
                repository.Delete(todo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(id));
                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetAllAsync();
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDo entity)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(entity.Id));
                todo.Title = entity.Title;
                todo.Content = entity.Content;
                todo.Status = entity.Status;
                todo.UpdateDate = DateTime.Now;
                if(await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, todo);
                return new ApiResponse("添加数据异常！");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
