using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories.Interfaces;

namespace UnitedRemote.Core.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> Get() => _userManager.Users;

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _userManager.FindByNameAsync(email);
        }

        public Task<IdentityResult> Create(ApplicationUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }
    }
}
