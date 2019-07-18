using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitedRemote.Core.Models.V1;

namespace UnitedRemote.Core.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        IQueryable<ApplicationUser> Get();
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<IdentityResult> Create(ApplicationUser user, string password);
    }
}
