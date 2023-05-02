using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Types;
using Sistema.Proyecto;
using Test.Web.GraphQL.Schemas;
using Test.Web.Services;

namespace Test.Web.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUsuarioService,UsuarioService>();

            //GraphQL                
            services.AddScoped<ISchema, UsuarioSchema>(services => new UsuarioSchema(new SelfActivatingServiceProvider(services)));
            //services.AddScoped<ISchema, CategoriaSchema>(services => new CategoriaSchema(new SelfActivatingServiceProvider(services)));

            //SOAP Web Services
            //services.AddSingleton<CalculatorWSDL.CalculatorSoap, CalculatorWSDL.CalculatorSoapClient>();


            return services;
        }

    }
}
