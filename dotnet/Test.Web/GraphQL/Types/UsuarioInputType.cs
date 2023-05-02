using GraphQL.Types;

namespace Test.Web.GraphQL.Types
{
    public class UsuarioInputType : InputObjectGraphType
    {
        public UsuarioInputType()
        {
            Name = "UsuarioInput";
            Field<NonNullGraphType<IntGraphType>>("IdRol").Description= "Id rol asignado al usuario";
            Field<NonNullGraphType<StringGraphType>>("Nombre").Description = "Nombre del nuevo usuario";
            Field<StringGraphType>("TipoDocumento").Description = "Tipo de documento para usuario";
            Field<StringGraphType>("NumDocumento").Description = "Numero de documento asignado al usuario";
            Field<StringGraphType>("Direccion").Description = "Dirección de facturación de usuario";
            Field<StringGraphType>("Telefono").Description = "Número telefónico de contacto de usuario";
            Field<NonNullGraphType<StringGraphType>>("Email").Description = "Dirección de correo electrónico del usuario";
            Field<NonNullGraphType<StringGraphType>>("Password").Description = "Contraseña";
        }
    }

    public class UsuarioLoginType : InputObjectGraphType
    {
        public UsuarioLoginType()
        {
            Name = "UsuarioLoginInput";

            Field<NonNullGraphType<StringGraphType>>("Email").Description = "Dirección de correo electrónico del usuario";
            Field<NonNullGraphType<StringGraphType>>("Password").Description = "Contraseña";
            Field<StringGraphType>("Token").Description = "Token";
        }
    }

    public class CambiarRolUsuariosInputType : InputObjectGraphType
    {
        public CambiarRolUsuariosInputType()
        {
            Name = "CambiarRolUsuariosInput";

            Field<NonNullGraphType<IntGraphType>>("IdRol").Description = "IdRol";
            Field<NonNullGraphType<ListGraphType<IntGraphType>>>("IdUsuarios").Description = "Id de los usuarios cambiar roles";
        }
    }

    public class CambiarRolUsuariosInput
    {
        public int IdRol { get; set; }
        public List<int> IdUsuarios { get; set; }
    }
    
    public class UsuariosIdType : InputObjectGraphType
    {
        public UsuariosIdType()
        {
            Name = "UsuarioId";
            Field<NonNullGraphType<ListGraphType<IntGraphType>>>("IdUsuarios").Description = "Id de los usuarios cambiar roles";
        }
    }    
}
