﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<UserPlan>>> GetUserPlans()
        {
          if (_context.UserPlans == null)
          {
              return NotFound();
          }
            return await _context.UserPlans.ToListAsync();
        }

        // GET: api/UserPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserPlan>> GetUserPlan(long id)
        {
          if (_context.UserPlans == null)
          {
              return NotFound();
          }
            var userPlan = await _context.UserPlans.FindAsync(id);

            if (userPlan == null)
            {
                return NotFound();
            }

            return userPlan;
        }

        // PUT: api/UserPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserPlan(long id, UserPlan userPlan)
        {
            if (id != userPlan.Id)
            {
                return BadRequest();
            }

            _context.Entry(userPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPlanExists(id))
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

        // POST: api/UserPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostUserPlan(string email, short planId)
        {
            UserPlan userPlan = new UserPlan();
            SoftitoflixUser? user = _userManager.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user != null)
            {
                userPlan.UserId = user.Id;
                userPlan.PlanId = planId;
                _context.UserPlans.Add(userPlan);
                _context.SaveChanges();
            }
        }

        // DELETE: api/UserPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserPlan(long id)
        {
            if (_context.UserPlans == null)
            {
                return NotFound();
            }
            var userPlan = await _context.UserPlans.FindAsync(id);
            if (userPlan == null)
            {
                return NotFound();
            }

            _context.UserPlans.Remove(userPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserPlanExists(long id)
        {
            return (_context.UserPlans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
