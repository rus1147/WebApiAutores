using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context) //porque necesito una instancia de la appDbContext
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public  async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
            //            al poner Include, va a ponerlo en el get de libros
        }
        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);
            if (!existeAutor)
            {
                return BadRequest($"El autor de Id: {libro.AutorId} no existe");
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
