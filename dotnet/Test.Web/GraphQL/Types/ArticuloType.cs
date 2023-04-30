using GraphQL.Types;
using Sistema.Proyecto;

namespace Test.Web.GraphQL.Types
{
    public class ArticuloType : ObjectGraphType<CatArticulo>
    {
        private readonly IRepository<CatCategoria> _categoriaRepository;
        private readonly IRepository<CatArticulo> _articuloRepository;

        public ArticuloType(IRepository<CatCategoria> categoriaRepository,
            IRepository<CatArticulo> articuloRepository)
        {
            _articuloRepository = articuloRepository;
            _categoriaRepository = categoriaRepository;

            Name = "Articulo";
            Field(x => x.IdArticulo);
            Field(x => x.Nombre);
            Field(x => x.Codigo);
            Field(x => x.Descripcion);
            Field(x => x.PrecioVenta);
            Field(x => x.Stock);
            Field<CategoriaType>("Categoria",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "Id" }),
                resolve: context => 
                            _categoriaRepository.Get(context.Source.IdCategoria),
                description: "Datos de la categoria");
                

        }
    }
}
