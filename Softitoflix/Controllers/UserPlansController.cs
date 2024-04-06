using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Softitoflix.Data;
using Softitoflix.Models;


namespace SoftITOFlix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPlansController : ControllerBase
    {
        private readonly SoftitoflixContext _context;
        private readonly UserManager<SoftitoflixUser> _userManager;

        public UserPlansController(SoftitoflixContext context, UserManager<SoftitoflixUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/UserPlans
        [HttpGet]
        [Authorize]
        public ActionResult<List<UserPlan>> GetUserPlans()
        {
            return _context.UserPlans.ToList();
        }

        // GET: api/UserPlans/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserPlan> GetUserPlan(long id)
        {
            var userPlan = _context.UserPlans.Find(id);

            if (userPlan == null)
            {
                return NotFound();
            }

            return userPlan;
        }

        // PUT: api/UserPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ActionResult PutUserPlan(long id, UserPlan userPlan)
        {
            if (id != userPlan.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userPlan).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }

        // POST: api/UserPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostUserPlan(string email, short planId)
        {
            Plan? plan = _context.Plans.Find(planId);
            SoftitoflixUser? softitoflixUser = _userManager.Users.Where(u => u.Email == email).FirstOrDefault();
            if (softitoflixUser != null)
            {
                UserPlan userPlan = new UserPlan(); 
                userPlan.UserId = softitoflixUser.Id;
                userPlan.PlanId = planId;
                userPlan.StartDate = DateTime.Today;
                userPlan.EndDate = userPlan.StartDate.AddMonths(1);
                softitoflixUser.isPassive = false;
                _context.UserPlans.Add(userPlan);
                _context.SaveChanges();
            }

        }

        // DELETE: api/UserPlans/5
        [HttpDelete("{id}")]
        public ActionResult DeleteUserPlan(long id)
        {
            UserPlan? userPlan = _context.UserPlans.Find(id);
            if (userPlan == null)
            {
                return NotFound();
            }

            _context.UserPlans.Remove(userPlan);
            _context.SaveChanges();

            return NoContent();
        }
    }
}