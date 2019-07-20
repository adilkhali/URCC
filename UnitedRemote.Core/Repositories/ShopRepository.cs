using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitedRemote.Core.Models.V1;

namespace UnitedRemote.Core.Repositories
{
    class ShopRepository : Repository<Shop>
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
