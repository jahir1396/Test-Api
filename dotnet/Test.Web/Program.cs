using AutoMapper;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Test.EntityFramework.TestDbContext;
using Test.Web.AutoMapper;
using Test.Web.GraphQL.Schemas;
using Test.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Test.Web", Version = "v1" });
});

builder.Services.AddDbContext<TestDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SistemaConexion")));
builder.Services.AddHttpContextAccessor();

//graphQl
//Todos los Schemas se agregaron al archivo IoC

// register graphQL
builder.Services.AddGraphQL(options =>
{
    options.EnableMetrics = true;    
})
.AddGraphTypes(ServiceLifetime.Scoped)
.AddSystemTextJson();


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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test.Web v1"));
    app.UseGraphQLAltair();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseValidaJwt();

app.UseGraphQL<ISchema>();

app.MapControllers();



app.Run();
