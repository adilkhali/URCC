using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitedRemote.Core.Models.V1;

namespace UnitedRemote.Core.Repositories.Interfaces
{
    public class FavoriteRepository : IFavoriteRepository
    {
        protected readonly DbContext _context;
        protected readonly DbSet<FavoriteShops> _entity;

        public FavoriteRepository(DbContext context)
        {
            _context = context;
            _entity = context.Set<FavoriteShops>();
        }

        public async Task<IdentityResult> AddToFavoriteAsync(ApplicationUser user, Shop shop)
        {
            var current = _context.Set<ApplicationUser>().Include(x=>x.LikedShops).FirstOrDefault(u => u.Id == user.Id);
            var likedShop = await _entity.Where(entry => entry.UserId == user.Id && entry.ShopId == shop.Id).SingleOrDefaultAsync();
            if (likedShop == null)
            {
                likedShop = new FavoriteShops()
                {
                    ShopId = shop.Id,
                    UserId = current.Id
                };
                var result = await _entity.AddAsync(likedShop);
                try
                {
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return await Task.FromResult(IdentityResult.Success);
                    }
                    else
                    {
                        return await Task.FromResult(IdentityResult.Failed(new IdentityError
                        {
                            Code = "",
                            Description = "error While adding to favorite"
                        }));
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

            return await Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "",
                    Description = "Shop already exist"
                })
            );
        }

        public async Task<List<Shop>> GetFavoriteAsync(ApplicationUser user)
        {
            return await _entity.Where(entry => entry.UserId == user.Id)
                .Include(x => x.LikedShop)
                .Select(x => x.LikedShop)
                .ToListAsync();
        }

        public async Task<IdentityResult> RemoveFromFavoriteAsync(ApplicationUser user, int shopId)
        {
           var likedShop = await _entity.Where(entry => entry.UserId == user.Id && entry.ShopId == shopId).SingleOrDefaultAsync();
            if (likedShop != null)
            {
                _entity.Remove(likedShop);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return await Task.FromResult(IdentityResult.Success);
                }
            }

            return await Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "",
                Description = "Error while trying to remove the shop from favorite"
            })
);
        }
    }
}
