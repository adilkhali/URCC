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

        public List<Shop> GetSortedShops(double longitude, double latitude)
        {
            Point userLocation = new Point(longitude, latitude)
            {
                SRID = 4326
            };
            return _entities.OrderBy(shop => shop.Location.Distance(userLocation)).ToList();
        }

        public List<Shop> GetNearbyShops( double longitude, double latitude, double searchRadious)
        {
            Point userLocation = new Point(longitude, latitude)
            {
                SRID = 4326
            };
            return _entities.Where(x => x.Location.Distance(userLocation) <= searchRadious).ToList();
        }
    }
}
