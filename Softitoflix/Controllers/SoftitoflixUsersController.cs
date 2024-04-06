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
    public class SoftitoflixUsersController : ControllerBase
    {
        private readonly SignInManager<SoftitoflixUser> _signInManager;
        private readonly SoftitoflixContext _context;

        public SoftitoflixUsersController(SignInManager<SoftitoflixUser> signInManager, SoftitoflixContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public struct LoginModel
        {
            public string userName { get; set; }
            public string password { get; set; }
        }

        // GET: api/SoftitoflixUsers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<SoftitoflixUser>> GetUsers(bool includePassive = true)
        {
            IQueryable<SoftitoflixUser> users = _signInManager.UserManager.Users;

            if(includePassive == false)
            {
                users = users.Where(u => u.isPassive == false);
            }
            return users.AsNoTracking().ToList();
        }

        // GET: api/SoftitoflixUsers/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<SoftitoflixUser> GetSoftitoflixUser(long id)
        {
            SoftitoflixUser? softitoflixUser = null;

            if(User.IsInRole("Admin") == false && User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
            {
                return Unauthorized();
            }
            softitoflixUser = _signInManager.UserManager.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefault();

            if (softitoflixUser == null)
            {
                return NotFound();
            }

            return softitoflixUser;
        }

        // PUT: api/SoftitoflixUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public ActionResult PutSoftitoflixUser(SoftitoflixUser softitoflixUser)
        {


             if(User.FindFirstValue(ClaimTypes.NameIdentifier) != softitoflixUser.Id.ToString())
             {
                return Unauthorized();
             }

            SoftitoflixUser? user = _signInManager.UserManager.Users.Where(u =>u.Id == softitoflixUser.Id).FirstOrDefault();
            if(user == null)
            {
                return NotFound();
            }

            user.PhoneNumber = softitoflixUser.PhoneNumber; 
            user.UserName = softitoflixUser.UserName;
            user.BirthDate = softitoflixUser.BirthDate;
            user.Email = softitoflixUser.Email;
            user.Name = softitoflixUser.Name;
            _signInManager.UserManager.UpdateAsync(user).Wait();

            return Ok();
        }

        // POST: api/SoftitoflixUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<string> PostSoftitoflixUser(SoftitoflixUser softitoflixUser)
        {
            if(User.Identity!.IsAuthenticated)
            {
                return BadRequest();
            } 
            IdentityResult identityResult = _signInManager.UserManager.CreateAsync(softitoflixUser, softitoflixUser.Password).Result;
            if (identityResult != IdentityResult.Success)
            {
                return identityResult.Errors.FirstOrDefault()!.Description;
            }
            return Ok(softitoflixUser.Id);
        }

        // DELETE: api/SoftitoflixUsers/5
        [HttpDelete("{id}")]
        public ActionResult DeleteSoftITOFlixUser(long id)
        {
            SoftitoflixUser? user = null;
            if (User.IsInRole("Admin") == false)
            {
                if (User.FindFirstValue(ClaimTypes.NameIdentifier) != id.ToString())
                {
                    return Unauthorized();
                }
            }

            user = _signInManager.UserManager.Users.Where(u => u.Id == id).FirstOrDefault()!;
            if (user == null)
            {
                return NotFound();
            }

            user.isPassive = true;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("Login")]
        public ActionResult<List<Media>> Login(LoginModel loginModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult;
            SoftitoflixUser softitoflixUser = _signInManager.UserManager.FindByNameAsync(loginModel.userName).Result;
            List<Media> medias = new List<Media>();
            IQueryable<Media> mediaQuery;
            IQueryable<int> userWatches;
            IGrouping<short, MediaCategory>? mediaCategories;


            if (softitoflixUser == null)
            {
                return NotFound();
            }

            signInResult = _signInManager.PasswordSignInAsync(softitoflixUser, loginModel.password, false, false).Result;

            if(User.IsInRole("Admin") == false)
            {
                if(User.IsInRole("ContentAdmin"))
                {
                    if (_context.UserPlans.Where(u => u.UserId == softitoflixUser.Id && u.EndDate == DateTime.Today).Any() == false)
                    {
                        softitoflixUser.isPassive = true;
                        _signInManager.UserManager.UpdateAsync(softitoflixUser).Wait();
                    }
                }
            }

   

            if (softitoflixUser.isPassive == true)
            {
                return Content("Passive");
            }


            if(signInResult.Succeeded == true)
            {
                mediaCategories = _context.UserFavorites.Where(u => u.UserId == softitoflixUser.Id).
                    Include(u => u.Media!).
                    Include(u => u.Media!.MediaCategories).
                    ToList().
                    SelectMany(u => u.Media!.MediaCategories!).
                    GroupBy(m => m.CategoryId).
                    OrderByDescending(m => m.Count()).
                    FirstOrDefault();
                if(mediaCategories != null)
                {
                    userWatches = _context.UserWatcheds.Where(u => u.UserId == softitoflixUser.Id).Include(u => u.Episode).Select(u => u.Episode!.MediaId).Distinct();
                    mediaQuery = _context.Medias.Include(m => m.MediaCategories).Where(mc => mc.MediaCategories!.Any(mc => mc.CategoryId == mediaCategories.Key) && userWatches.Contains(mc.Id));
                    if(softitoflixUser.Restriction != null) 
                    {
                        mediaQuery = mediaQuery.Include(m => m.MediaRestrictions).Where(m => m.MediaRestrictions!.Any(r => r.RestrictionId == softitoflixUser.Restriction));
                    }
                    medias = mediaQuery.ToList();
                }
            }
            return medias;
        }


        [HttpPost("AssignRole")]
        [Authorize(Roles = "Admin")]
        public ActionResult AssignRole(string email)
        {
            SoftitoflixUser? softitoflixUser = _signInManager.UserManager.Users.Where(u =>u.Email == email).FirstOrDefault();
            if (softitoflixUser == null)
            {
                return NotFound();
            }
            _signInManager.UserManager.AddToRoleAsync(softitoflixUser, "ContentAdmin").Wait();
            return Ok();
        }

        [HttpPost("RemoveRole")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRole(string email) 
        {
            SoftitoflixUser? softitoflixUser = _signInManager.UserManager.Users.Where(u => u.Email == email).FirstOrDefault();
            if (softitoflixUser == null)
            {
                return NotFound();
            }
            _signInManager.UserManager.RemoveFromRoleAsync(softitoflixUser, "ContentAdmin").Wait();
            return Ok();
        }


        [HttpPost("Logout")]
        public void Logout()
        {
            _signInManager.SignOutAsync().Wait();
        }

    }
}
