using GraphQL.Types;
using Test.Web.Controllers.InputOutput;

namespace Test.Web.GraphQL.Types
{
    public class CrearUsuarioOutputType : ObjectGraphType
    {
        public CrearUsuarioOutputType()
        {
            Name = "CrearUsuarioOutput";

            Field<StringGraphType>("mensaje");
        }
    }

    public class LoginUsuarioOutputType : ObjectGraphType
    {
        public LoginUsuarioOutputType()
        {
            Name = "LoginOutput";
            
            Field<DateTimeGraphType>("expires");
            Field<StringGraphType>("mensaje");
            Field<StringGraphType>("token");
            Field<StringGraphType>("usuario");
        }
    }

    public class CalculatorOutputType : ObjectGraphType
    {
        public CalculatorOutputType()
        {
            Name = "ResultadoOutput";

            Field<IntGraphType>("resultado");
        }
    }

    public class CalculatorOutput
    {
        public int Resultado { get; set; }
    }
}
