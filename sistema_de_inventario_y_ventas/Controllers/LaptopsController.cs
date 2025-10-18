using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_de_inventario_y_ventas.Context;
using sistema_de_inventario_y_ventas.Context.Entitys;

namespace sistema_de_inventario_y_ventas.Controllers
{
        [Route("api/[controller]")]
        public class LaptopsController : ControllerBase
        {
                private readonly AppDbContext _context;
                public LaptopsController(AppDbContext contex) {
                        _context = contex;
                }

                [HttpGet]
                public async Task<List<Laptop>> Get() => await _context.Laptops.ToListAsync();


                [HttpGet("{id:int}", Name = "GetLaptop")]
                public async Task<ActionResult<Laptop>> Get(int id ) {
                        var laptop = await _context.Laptops.FirstOrDefaultAsync(x => x.Id == id); 

                       if(laptop is null)
                        {
                                return NotFound();
                        }

                       return laptop;
                }


                [HttpPost]
                public async Task<CreatedAtRouteResult> Post (Laptop laptop ) {
                        _context.Add(laptop);

                        await _context.SaveChangesAsync();

                        return CreatedAtRoute("GetLaptop", new { id = laptop.Id }, laptop);
                }

                [HttpPut("{id:int}")]

                public async Task<ActionResult> Put( int id, Laptop laptop ) {
                        var existeLaptop = await _context.Laptops.AnyAsync(x => x.Id == id);

                        if(!existeLaptop)
                        {
                                return NotFound();
                        }

                        _context.Update(laptop);

                        await _context.SaveChangesAsync();

                        return NoContent();
                }


                [HttpDelete("{id:int}")]
                public async Task<ActionResult> Delete( int id ) {
                        var filasBorradas = await _context.Laptops.Where(x => x.Id == id).ExecuteDeleteAsync( );
                        
                        if(filasBorradas == 0)
                        {
                                return NotFound();
                        }

                        return NoContent(); 
                }
        }
}
