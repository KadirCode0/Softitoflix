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
    public class MediaRestrictionsController : ControllerBase
    {
        private readonly SoftitoflixContext _context;

        public MediaRestrictionsController(SoftitoflixContext context)
        {
            _context = context;
        }

        // GET: api/MediaRestrictions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MediaRestriction>>> GetMediaRestrictions()
        {
          if (_context.MediaRestrictions == null)
          {
              return NotFound();
          }
            return await _context.MediaRestrictions.ToListAsync();
        }

        // GET: api/MediaRestrictions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaRestriction>> GetMediaRestriction(int id)
        {
          if (_context.MediaRestrictions == null)
          {
              return NotFound();
          }
            var mediaRestriction = await _context.MediaRestrictions.FindAsync(id);

            if (mediaRestriction == null)
            {
                return NotFound();
            }

            return mediaRestriction;
        }

        // PUT: api/MediaRestrictions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMediaRestriction(int id, MediaRestriction mediaRestriction)
        {
            if (id != mediaRestriction.MediaId)
            {
                return BadRequest();
            }

            _context.Entry(mediaRestriction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaRestrictionExists(id))
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

        // POST: api/MediaRestrictions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MediaRestriction>> PostMediaRestriction(MediaRestriction mediaRestriction)
        {
          if (_context.MediaRestrictions == null)
          {
              return Problem("Entity set 'SoftitoflixContext.MediaRestrictions'  is null.");
          }
            _context.MediaRestrictions.Add(mediaRestriction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MediaRestrictionExists(mediaRestriction.MediaId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMediaRestriction", new { id = mediaRestriction.MediaId }, mediaRestriction);
        }

        // DELETE: api/MediaRestrictions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMediaRestriction(int id)
        {
            if (_context.MediaRestrictions == null)
            {
                return NotFound();
            }
            var mediaRestriction = await _context.MediaRestrictions.FindAsync(id);
            if (mediaRestriction == null)
            {
                return NotFound();
            }

            _context.MediaRestrictions.Remove(mediaRestriction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediaRestrictionExists(int id)
        {
            return (_context.MediaRestrictions?.Any(e => e.MediaId == id)).GetValueOrDefault();
        }
    }
}
