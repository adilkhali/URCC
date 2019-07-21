using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnitedRemote.Core.Models.V1
{
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Liked Shops")]
        public virtual ICollection<FavoriteShops> LikedShops { get; set; }
    }
}
