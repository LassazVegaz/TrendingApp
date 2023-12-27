using Microsoft.EntityFrameworkCore;
using ShoppingApp.API.ExceptionHandlers;
using ShoppingApp.API.Mapper;
using ShoppingApp.Core.Data;
using ShoppingApp.Core.Options;
using ShoppingApp.Core.Services;
using ShoppingApp.Logic.Services;

var builder = WebApplication.CreateBuilder(args);

// options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

// Exception handlers
builder.Services.AddExceptionHandler<UnknownExceptionHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// db context
var mySqlVersion = new MySqlServerVersion(builder.Configuration.GetValue("MySQLVersion", "8.0.29"));
builder.Services.AddDbContext<ShoppingAppContext>(ops =>
    ops.UseMySql(builder.Configuration.GetConnectionString("ShoppingAppDb"), mySqlVersion));

// mapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

// app services
builder.Services.AddScoped<IUsersService, UsersService>();

var app = builder.Build();

app.UseExceptionHandler(ops => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
