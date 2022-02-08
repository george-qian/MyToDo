using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Context
{
    public class MyToDoContextFactory : IDesignTimeDbContextFactory<MyToDoContext>
    {
        public MyToDoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyToDoContext>();
            optionsBuilder.UseSqlite("Data Source=to.db");

            return new MyToDoContext(optionsBuilder.Options);
        }
    }
}
