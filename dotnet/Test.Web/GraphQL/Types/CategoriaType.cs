using GraphQL.Types;
using Sistema.Proyecto;

namespace Test.Web.GraphQL.Types
{
    public class CategoriaType : ObjectGraphType<CatCategoria>
    {
        private readonly IRepository<CatCategoria> _categoriaRepository;
        private readonly IRepository<CatArticulo> _articuloRepository;

        public CategoriaType(IRepository<CatCategoria> categoriaRepository, IRepository<CatArticulo> articuloRepository)
        {
            _categoriaRepository = categoriaRepository;
            _articuloRepository = articuloRepository;

            Name = "Categoria";
            Field(x => x.IdCategoria);
            Field(x => x.Nombre);
            Field(x => x.Descripcion);
            Field(x => x.EstaActivo);

            Field<ListGraphType<ArticuloType>>("Articulos",
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
