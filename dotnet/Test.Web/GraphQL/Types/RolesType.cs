using GraphQL.Types;
using Sistema.Proyecto;

namespace Test.Web.GraphQL.Types
{
    public class RolesType : ObjectGraphType<CatRol>
    {
        public RolesType()
        {
            Name = "Rol";
            Field(x => x.IdRol);
            Field(x => x.Nombre);
            Field(x => x.EstaActivo);

            Field<ListGraphType<UsuariosType>>("Usuarios",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<IntGraphType>
                    {
                        Name = "Id"
                    }
                }));
        }
    }
}
