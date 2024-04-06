using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softitoflix.Data;
using Softitoflix.Models;

namespace Softitoflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly SoftitoflixContext _context;

        public DirectorsController(SoftitoflixContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        [Authorize]
        public ActionResult<List<Director>> GetDirectors()
        {
            return _context.Directors.ToList();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Director> GetDirector(int id)
        {
            Director? director = _context.Directors.Find(id);

            if (director == null)
            {
                return NotFound();
            }

            return director;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutDirector(Director director)
        {
            _context.Directors.Update(director);
            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public int PostDirector(Director director)
        {

            _context.Directors.Add(director);
            _context.SaveChanges();
            return director.Id;
        }
    }
}
