using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork work, IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string Account, string Password)
        {
            try
            {
                var model = await work.GetRepository<User>().GetFirstOrDefaultAsync(predicate:
                    x => (x.Account.Equals(Account) && x.Password.Equals(Password)));
                if (model == null)
                    return new ApiResponse("账号密码错误，请重试！");
                return new ApiResponse(true, model);
             
            }catch(Exception ex)
            {
                return new ApiResponse("登录失败");
            }
        }

        public async Task<ApiResponse> Register(UserDto user)
        {
            try
            {
                var model = mapper.Map<User>(user);
                var repository = work.GetRepository<User>();
                var userModel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Account.Equals(model.Account));
                if (userModel != null)
                    return new ApiResponse($"当前账号：{model.Account}已存在，请重新注册！");
                model.CreateDate = DateTime.Now;
                await repository.InsertAsync(model);
                if (await work.SaveChangesAsync() > 0)
                    return new ApiResponse(true, model);
                return new ApiResponse("注册失败，请重试！");
            }
            catch(Exception ex)
            {
                return new ApiResponse("注册账号失败！");
            }
        }
    }
}
