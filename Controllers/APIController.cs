using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnimalChip.Data;
using AnimalChip.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AnimalChip.Controllers
{
    [Produces("application/json")]
    [Route("/api")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public APIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal()
        {
            try
            {
                return await _context.Animal.ToListAsync();
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }
        }


        // GET: /api/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            try
            {
                var animal = await _context.Animal.FindAsync(id);

                if (animal == null)
                {
                    return NotFound();
                }

                return animal;

            } catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }


        }

        // PUT: /api/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            animal.Email = "api@api";
            animal.ModifiedTime = DateTime.Now;
            animal.BirthDate = DateTime.Now;
            _context.Entry(animal).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /api
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(Animal animal)
        {
            try
            {
                if(animal ==null)
                {
                    return BadRequest();
                }

                animal.Email = "api@api";
                animal.ModifiedTime = DateTime.Now;
                animal.BirthDate = DateTime.Now;
                _context.Animal.Add(animal);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }
        }

        // DELETE: /api/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                var animal = await _context.Animal.FindAsync(id);
                if (animal == null)
                {
                    return NotFound();
                }

                _context.Animal.Remove(animal);
                await _context.SaveChangesAsync();

                return NoContent();
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retriving data from database");
            }
        }

        private bool AnimalExists(int id)
        {
            return _context.Animal.Any(e => e.Id == id);
        }
    }
}
