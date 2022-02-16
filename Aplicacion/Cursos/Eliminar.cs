using System.Net;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{

    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public int Id { get; set; }

        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                     //throw new Exception("No se puede eliminar el curso");
                        throw new ManejadorExcepcion(HttpStatusCode.NotFound, new{curso = "no se encontro el curso"});
                }
                _context.Remove(curso);
                var r = await _context.SaveChangesAsync();
                if (r > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }


}