using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    /// <summary>
    /// 待办事项
    /// </summary>
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
        public async Task<ApiResponse<MemoDto>> AddAsync(MemoDto model)
        {
            try
            {
                var memoEntity = mapper.Map<Memo>(model);
                await work.GetRepository<Memo>().InsertAsync(memoEntity);
                if (await work.SaveChangesAsync() > 0)
                {
                    var dto = mapper.Map<MemoDto>(memoEntity);
                    return new ApiResponse<MemoDto>(true, dto);
            }
                return new ApiResponse<MemoDto>("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse<MemoDto>(ex.Message);
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(id));
                repository.Delete(memo);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, "");
                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<MemoDto>> GetSingleAsync(int id)
        {
            try
            {
                var repository = work.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(id));
                var dto = mapper.Map<MemoDto>(memo);
                return new ApiResponse<MemoDto>(true, dto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<MemoDto>(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = work.GetRepository<Memo>();
                var memos = await repository.GetPagedListAsync(predicate:
                    x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                    pageIndex: parameter.PageIndex,
                    pageSize: parameter.PageSize,
                    orderBy:source=>source.OrderByDescending(t=>t.UpdateDate));
                return new ApiResponse(true, memos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<MemoDto>> UpdateAsync(MemoDto model)
        {
            try
            {
                var memoEntityNew = mapper.Map<Memo>(model);
                var repository = work.GetRepository<Memo>();
                var memoEntity = await repository.GetFirstOrDefaultAsync(predicate: entity => entity.Id.Equals(memoEntityNew.Id));
                memoEntity.Title = model.Title;
                memoEntity.Content = model.Content;
                memoEntity.UpdateDate = DateTime.Now;
                repository.Update(memoEntity);
                if (await work.SaveChangesAsync() > 0)
                {
                    var dto = mapper.Map<MemoDto>(memoEntity);
                    return new ApiResponse<MemoDto>(true, dto);
                }
                return new ApiResponse<MemoDto>("添加数据异常！");
            }
            catch (Exception ex)
            {
                return new ApiResponse<MemoDto>(ex.Message);
            }
        }
    }
}
