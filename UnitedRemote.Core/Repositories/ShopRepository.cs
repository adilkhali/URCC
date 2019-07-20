using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories.Interfaces;

namespace UnitedRemote.Core.Repositories
{
    public class ShopRepository : Repository<Shop> , IShopRepository
    {
        public ShopRepository(DbContext context) :base(context)
        {
        }

        public List<Shop> GetNearbyShops(double latitude, double longitude, double searchRadious)
        {
            Point userLocation = new Point(latitude, longitude);
            return _entities.Where(x => x.Location.Distance(userLocation) <= searchRadious).ToList();
        }
    }
}
