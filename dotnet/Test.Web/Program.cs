using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.EntityFramework.TestDbContext;
using Test.Web.AutoMapper;
using Test.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TestDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SistemaConexion")));
builder.Services.AddHttpContextAccessor();

//Mapper
var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new DtoMappings());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
IoC.AddDependency(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseValidaJwt();

app.MapControllers();



app.Run();
