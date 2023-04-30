using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using Sistema.Proyecto;
using Test.Web.GraphQL.Types;

namespace Test.Web.GraphQL.Querys
{
    public class CategoriaMutation : ObjectGraphType
    {
        private readonly IRepository<CatCategoria> _categoriaRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CategoriaMutation(IRepository<CatCategoria> categoriaRepository, IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository;
            _unitOfWork = unitOfWork;

            Field<CategoriaType>(
            "crearActualizarCategoria",
            arguments: new QueryArguments(new QueryArgument<NonNullGraphType<CategoriaInputType>>
            {
                Name = "categorias"
            }),
            resolve: context =>
            {
                var categoria = context.GetArgument<CatCategoria>("categorias");

                if (categoria.IdCategoria == 0)
                    _categoriaRepository.Insert(categoria);
                else
                    _categoriaRepository.Update(categoria);
                
                _unitOfWork.SaveChanges();
                return categoria;
            });
        }
    }
}
