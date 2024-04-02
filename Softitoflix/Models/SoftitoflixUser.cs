using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Softitoflix.Models;

// Add profile data for application users by adding properties to the SoftitoflixUser class
public class SoftitoflixUser : IdentityUser<long>
{
    [Column(TypeName = "date")]
    public DateTime BirthDate { get; set; }

    [StringLength(100, MinimumLength = 2)]
    [Column(TypeName = "nvarchar(100)")]
    public string Name { get; set; } = "";

    public bool isPassive { get; set; }

    [NotMapped]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = "";

    [NotMapped]
    public byte? Restriction
    { 
        get 
        { 
            int age = DateTime.Today.Year - BirthDate.Year;
            if (age < 7)
            {
                return 7;
            }
            else
            {
                if(age < 13)
                {
                    return 13;
                }
                else
                {
                    if (age < 18)
                    {
                        return 18;
                    }
                }
            }
            return null;
        } 
        set 
        {

        } 
    }
}

