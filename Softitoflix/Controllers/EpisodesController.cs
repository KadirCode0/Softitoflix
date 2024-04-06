using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softitoflix.Data;
using Softitoflix.Models;

namespace Softitoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly SoftitoflixContext _context;
        private readonly SignInManager<SoftitoflixUser> _signInManager;

        public EpisodesController(SoftitoflixContext context, SignInManager<SoftitoflixUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        // GET: api/Episodes
        [HttpGet]
        [Authorize]
        public ActionResult<List<Episode>> GetEpisodes(int mediaId, byte seasonNumber)
        {
            return _context.Episodes.Where(e => e.MediaId == mediaId && e.SeasonNumber == seasonNumber).OrderBy(e => e.EpisodeNumber).AsNoTracking().ToList(); 
        }

        // GET: api/Episodes/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Episode> GetEpisode(long id)
        {
            Episode? episode = _context.Episodes.Find(id);
            if (episode == null)
            {
                return NotFound();
            }
            return episode;
        }

        [HttpGet("Watch")]
        [Authorize]
        public ActionResult Watch(long id)
        {
            UserWatched userWatched = new UserWatched();
            Episode? episode = _context.Episodes.Include(e => e.Media).ThenInclude(m => m.MediaRestrictions).FirstOrDefault(e => e.Id == id);

            if (episode == null)
                return NotFound();

            List<MediaRestriction> mediaRestrictions = episode.Media.MediaRestrictions;
            var findUser = _signInManager.UserManager.GetUserAsync(User).Result;
            int userAge = DateTime.Today.Year - findUser.BirthDate.Year;
            try
            {
                foreach(MediaRestriction mediaRestriction in mediaRestrictions)
                {
                    if(mediaRestriction.RestrictionId >= userAge)
                    {
                        return BadRequest();
                    }
                    userWatched.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                    userWatched.EpisodeId = id;
                    _context.UserWatcheds.Add(userWatched);
                    episode.ViewCount++;
                    _context.Episodes.Update(episode);
                    _context.SaveChanges();
                }

                return Ok();
                
            }
            catch (Exception ex)
            {

            }


        }

        // PUT: api/Episodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutEpisode(Episode episode)
        {
            _context.Episodes.Update(episode);
            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Episodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public long PostEpisode(Episode episode)
        {
            _context.Episodes.Add(episode);
            _context.SaveChanges();
            return episode.Id;
        }

        // DELETE: api/Episodes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeleteEpisode(long id)
        {
            Episode? episode = _context.Episodes.Find(id);

            if (episode == null)
            {
                return NotFound();
            }

            episode.isPassive = true;
            _context.Episodes.Update(episode);
            _context.SaveChanges();
            return Ok();
        }

    }
}
