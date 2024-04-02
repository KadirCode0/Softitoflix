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
    public class RestrictionsController : ControllerBase
    {
        private readonly SoftitoflixContext _context;

        public RestrictionsController(SoftitoflixContext context)
        {
            _context = context;
        }

        // GET: api/Restrictions
        [HttpGet]
        
        public ActionResult<List<Restriction>> GetRestriction()
        {
            return _context.Restrictions.ToList();
        }

        // GET: api/Restrictions/5
        [HttpGet("{id}")]
        public ActionResult<Restriction> GetRestriction(byte id)
        {
            Restriction? restriction = _context.Restrictions.Find(id);

            if (restriction == null)
            {
                return NotFound();
            }

            return restriction;
        }

        // PUT: api/Restrictions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutRestriction(byte id, Restriction restriction)
        {
            _context.Restrictions.Update(restriction);
            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Restrictions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public int PostRestriction(Restriction restriction)
        {
            _context.Restrictions.Add(restriction);
            _context.SaveChanges();
            return restriction.Id;
        }
    }
}
