using GraphQL.Types;

namespace Test.Web.GraphQL.Types
{
    public class CategoriaInputType : InputObjectGraphType
    {
        public CategoriaInputType()
        {
            Name = "CategoriasInput";
            Field<IntGraphType>("IdCategoria").Description = "Id de la categoria";
            Field<StringGraphType>("Nombre").Description = "Nombre de la categoria";
            Field<StringGraphType>("Descripcion").Description = "Descripcion de la categoria puede permitir nulos";
            Field<BooleanGraphType>("EstaActivo").Description = "Indica si la categoria está activa o inactiva";            
        }
    }
}
