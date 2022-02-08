﻿using Arch.EntityFrameworkCore.UnitOfWork;

namespace MyToDo.Context.Repository
{
    public class MemoRepository : Repository<Memo>, IRepository<Memo>
    {
        public MemoRepository(MyToDoContext dbContext) : base(dbContext)
        {
        }
    }
}
