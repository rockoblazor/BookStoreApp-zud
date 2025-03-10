using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.DTOs.Author;

using BookStoreApp.API.Static;

///using BookStoreApp.API.Configurations;


namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDTO>>> GetAuthors()
        {
            //if (_context.Authors == null)
            //{
            //    return NotFound();
            //}
            //  return await _context.Authors.ToListAsync();
            try
            {
                var authors = await _context.Authors.ToListAsync();
                var authorDtos = mapper.Map<IEnumerable<AuthorReadOnlyDTO>>(authors);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }

            }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDTO>> GetAuthor(int id)
        {
          if (_context.Authors == null)
          {
              return NotFound();
          }
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = mapper.Map<AuthorReadOnlyDTO>(author);
            return Ok(authorDto);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDTO authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest();
            }
            var author = await _context.Authors.FindAsync(id);

            ///_context.Entry(author).State = EntityState.Modified;
            mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDTO>> PostAuthor(AuthorCreateDTO authorDto)
        {
            /*if (_context.Authors == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
            }*/

            /// adding DTO call for authors here 
            var author = mapper.Map<Author>(authorDto);
            ///
            /// _context.Authors.Add(author); works also, but we can be consistent with adding await to match the method type
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return (_context.Authors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
