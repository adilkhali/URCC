using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitedRemote.Core.Models.V1;

namespace UnitedRemote.Core.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IdentityResult> RemoveFromFavoriteAsync(ApplicationUser user, int shopId);
        Task<List<Shop>> GetFavoriteAsync(ApplicationUser user);
        Task<IdentityResult> AddToFavoriteAsync(ApplicationUser user, Shop shop);
    }
}
