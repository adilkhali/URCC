using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedRemote.Core.Models.V1
{
    public class ApplicationUser : IdentityUser
    {
        public string Location { get; set; }

    }
}
