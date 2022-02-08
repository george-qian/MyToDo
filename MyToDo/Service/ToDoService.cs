using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    /// <summary>
    /// 待办事项
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<ToDoDto>> AddAsync(ToDoDto model)
        {
            try
            {
                var todoEntity = mapper.Map<ToDo>(model);
                await work.GetRepository<ToDo>().InsertAsync(todoEntity);
                if (await work.SaveChangesAsync() > 0)
                {
                    var dto = mapper.Map<ToDoDto>(todoEntity);
                    return new ApiResponse<ToDoDto>(true, dto);
                }
                return new ApiResponse<ToDoDto>("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse<ToDoDto>(ex.Message);
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

        public async Task<ApiResponse<ToDoDto>> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(id));
                var dto = mapper.Map<ToDoDto>(todo);
                return new ApiResponse<ToDoDto>(true, dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ToDoDto>(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate:
                    x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.UpdateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
        public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        {
            try
            {
                var repository = work.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate:
                    x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search))
                    &&(parameter.Status == null?true:x.Status.Equals(parameter.Status)),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy: source => source.OrderByDescending(t => t.UpdateDate));
                return new ApiResponse(true, todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
        public async Task<ApiResponse<ToDoDto>> UpdateAsync(ToDoDto model)
        {
            try
            {
                var todoEntityNew = mapper.Map<ToDo>(model);
                var repository = work.GetRepository<ToDo>();
                var todoEntity = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(todoEntityNew.Id));
                todoEntity.Title = model.Title;
                todoEntity.Content = model.Content;
                todoEntity.Status = model.Status;
                todoEntity.UpdateDate = DateTime.Now;
                repository.Update(todoEntity);
                if (await work.SaveChangesAsync() > 0)
                {
                    var dto = mapper.Map<ToDoDto>(todoEntity);
                    return new ApiResponse<ToDoDto>(true, dto);
                }
                return new ApiResponse<ToDoDto>("添加数据异常！");
            }
            catch (Exception ex)
            {
                return new ApiResponse<ToDoDto>(ex.Message);
            }
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            try
            {
                var todos = await work.GetRepository<ToDo>().GetAllAsync(orderBy: x => x.OrderByDescending(t => t.CreateDate));
                var memos = await work.GetRepository<Memo>().GetAllAsync(orderBy: x => x.OrderByDescending(t => t.CreateDate));
                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count();
                summary.CompletedCount=todos.Where(t=>t.Status==1).Count();
                summary.CompletedRatio= (summary.CompletedCount/(double)summary.Sum).ToString("0%");
                summary.MemoCount=memos.Count();
                summary.ToDoList = new System.Collections.ObjectModel.ObservableCollection<ToDoDto>(mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
                summary.MemoList = new System.Collections.ObjectModel.ObservableCollection<MemoDto>(mapper.Map<List<MemoDto>>(memos));
                return new ApiResponse<SummaryDto>(true, summary);
            }
            catch (Exception ex)
            {
                return new ApiResponse<SummaryDto>(ex.Message);
            }
        }
    }
}
