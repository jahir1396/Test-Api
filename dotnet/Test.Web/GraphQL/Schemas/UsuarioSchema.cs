using GraphQL;
using GraphQL.Types;
using Test.Web.GraphQL.Querys;

namespace Test.Web.GraphQL.Schemas
{
    public class UsuarioSchema  : Schema
    {
        public UsuarioSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<UsuarioQuery>();            
            Mutation = serviceProvider.GetRequiredService<UsuarioMutation>();
        }
    }
}
