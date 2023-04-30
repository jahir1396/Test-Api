using GraphQL.MicrosoftDI;
using GraphQL.Types;
using Sistema.Proyecto;
using Test.Web.GraphQL.Schemas;

namespace Test.Web.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ISchema, UsuarioSchema>(services => new UsuarioSchema(new SelfActivatingServiceProvider(services)));
            //services.AddScoped<ISchema, CategoriaSchema>(services => new CategoriaSchema(new SelfActivatingServiceProvider(services)));

            return services;
        }

    }
}
