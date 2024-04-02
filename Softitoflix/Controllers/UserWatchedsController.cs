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
    public class UserWatchedsController : ControllerBase
    {
        private readonly SoftitoflixContext _context;

        public UserWatchedsController(SoftitoflixContext context)
        {
            _context = context;
        }

        // GET: api/UserWatcheds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserWatched>>> GetUserWatcheds()
        {
          if (_context.UserWatcheds == null)
          {
              return NotFound();
          }
            return await _context.UserWatcheds.ToListAsync();
        }

        // GET: api/UserWatcheds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWatched>> GetUserWatched(long id)
        {
          if (_context.UserWatcheds == null)
          {
              return NotFound();
          }
            var userWatched = await _context.UserWatcheds.FindAsync(id);

            if (userWatched == null)
            {
                return NotFound();
            }

            return userWatched;
        }

        // PUT: api/UserWatcheds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserWatched(long id, UserWatched userWatched)
        {
            if (id != userWatched.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userWatched).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserWatchedExists(id))
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

        // POST: api/UserWatcheds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserWatched>> PostUserWatched(UserWatched userWatched)
        {
          if (_context.UserWatcheds == null)
          {
              return Problem("Entity set 'SoftitoflixContext.UserWatcheds'  is null.");
          }
            _context.UserWatcheds.Add(userWatched);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserWatchedExists(userWatched.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUserWatched", new { id = userWatched.UserId }, userWatched);
        }

        // DELETE: api/UserWatcheds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserWatched(long id)
        {
            if (_context.UserWatcheds == null)
            {
                return NotFound();
            }
            var userWatched = await _context.UserWatcheds.FindAsync(id);
            if (userWatched == null)
            {
                return NotFound();
            }

            _context.UserWatcheds.Remove(userWatched);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserWatchedExists(long id)
        {
            return (_context.UserWatcheds?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
