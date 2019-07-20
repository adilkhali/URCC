using System;
using System.Collections.Generic;
using System.Text;
using UnitedRemote.Core.Models.V1;

namespace UnitedRemote.Core.Repositories.Interfaces
{
    public interface IShopRepository :IRepository<Shop>
    {
        List<Shop> GetNearbyShops(double latitude, double longitude, double searchRadious);
    }
}
