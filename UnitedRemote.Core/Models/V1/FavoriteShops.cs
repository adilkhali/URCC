using System;
using System.Collections.Generic;
using System.Text;

namespace UnitedRemote.Core.Models.V1
{
    public class FavoriteShops
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int ShopId { get; set; }
        public virtual Shop LikedShop { get; set; }
    }
}
