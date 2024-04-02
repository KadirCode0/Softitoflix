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
    public class PlansController : ControllerBase
    {
        private readonly SoftitoflixContext _context;

        public PlansController(SoftitoflixContext context)
        {
            _context = context;
        }

        // GET: api/Plans
        [HttpGet]
        public ActionResult<List<Plan>> GetPlans()
        {
            return _context.Plans.AsNoTracking().ToList();
        }

        // GET: api/Plans/5
        [HttpGet("{id}")]
        public ActionResult<Plan> GetPlan(short id)
        {
            Plan? plan = _context.Plans.Find(id);

            if (plan == null)
            {
                return NotFound();
            }

            return plan;
        }

        // PUT: api/Plans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult PutPlan(short id, Plan plan)
        {
            if (id != plan.Id)
            {
                return BadRequest();
            }
            
            _context.Plans.Update(plan);
            _context.SaveChanges();
            return NoContent();
        }

        // POST: api/Plans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "ContentAdmin")]
        public int PostPlan(Plan plan)
        {
            _context.Plans.Add(plan);
            _context.SaveChanges();

            return plan.Id;
        }

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ContentAdmin")]
        public ActionResult DeletePlan(short id)
        {
            Plan? plan = _context.Plans.Find(id);
            if (plan == null)
            {
                return NotFound();
            }            
            _context.Plans.Remove(plan);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
