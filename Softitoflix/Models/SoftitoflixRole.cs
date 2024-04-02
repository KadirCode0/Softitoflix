using Microsoft.AspNetCore.Identity;

namespace Softitoflix.Models
{
    public class SoftitoflixRole : IdentityRole<long>
    {
        public SoftitoflixRole(string roleName) : base(roleName) { }

        public SoftitoflixRole() { }
    }
}
