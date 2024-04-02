using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softitoflix.Data;
using Softitoflix.Models;

namespace Softitoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaDirectorsController : ControllerBase
    {
        private readonly SoftitoflixContext _context;

        public MediaDirectorsController(SoftitoflixContext context)
        {
            _context = context;
        }

        // GET: api/MediaDirectors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaDirector>>> GetMediaDirectors()
        {
          if (_context.MediaDirectors == null)
          {
              return NotFound();
          }
            return await _context.MediaDirectors.ToListAsync();
        }

        // GET: api/MediaDirectors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaDirector>> GetMediaDirector(int id)
        {
          if (_context.MediaDirectors == null)
          {
              return NotFound();
          }
            var mediaDirector = await _context.MediaDirectors.FindAsync(id);

            if (mediaDirector == null)
            {
                return NotFound();
            }

            return mediaDirector;
        }

        // PUT: api/MediaDirectors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMediaDirector(int id, MediaDirector mediaDirector)
        {
            if (id != mediaDirector.MediaId)
            {
                return BadRequest();
            }

            _context.Entry(mediaDirector).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaDirectorExists(id))
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

        // POST: api/MediaDirectors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MediaDirector>> PostMediaDirector(MediaDirector mediaDirector)
        {
          if (_context.MediaDirectors == null)
          {
              return Problem("Entity set 'SoftitoflixContext.MediaDirectors'  is null.");
          }
            _context.MediaDirectors.Add(mediaDirector);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MediaDirectorExists(mediaDirector.MediaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMediaDirector", new { id = mediaDirector.MediaId }, mediaDirector);
        }

        // DELETE: api/MediaDirectors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMediaDirector(int id)
        {
            if (_context.MediaDirectors == null)
            {
                return NotFound();
            }
            var mediaDirector = await _context.MediaDirectors.FindAsync(id);
            if (mediaDirector == null)
            {
                return NotFound();
            }

            _context.MediaDirectors.Remove(mediaDirector);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediaDirectorExists(int id)
        {
            return (_context.MediaDirectors?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
    }
}
