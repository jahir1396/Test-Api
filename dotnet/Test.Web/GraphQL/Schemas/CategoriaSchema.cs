using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Test.Web.GraphQL.Querys;

namespace Test.Web.GraphQL.Schemas
{
    public class CategoriaSchema : Schema
    {
        public CategoriaSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Mutation = serviceProvider.GetRequiredService<CategoriaMutation>();
        }
    }
}
