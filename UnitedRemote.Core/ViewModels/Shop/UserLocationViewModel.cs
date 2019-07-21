
using System.ComponentModel.DataAnnotations;

namespace UnitedRemote.Core.ViewModels.Shop
{
    public class UserLocationViewModel
    {
        [Required]
        public double longitude { get; set; }
        [Required]
        public double latitude { get; set; }
    }
}
