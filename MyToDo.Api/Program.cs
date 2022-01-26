using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build()["ConnectionStrings:ToDoConnection"];
builder.Services.AddDbContext<MyToDoContext>(option =>
{
    option.UseSqlite(connectionString);
})
    .AddUnitOfWork<MyToDoContext>()
    .AddCustomRepository<ToDo,ToDoRepository>()
    .AddCustomRepository<Memo,MemoRepository>()
    .AddCustomRepository<User,UserRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
