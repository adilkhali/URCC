using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UnitedRemote.Core.ViewModels
{
    public class ShopViewModel
    {

        [NotMapped]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double latitude { get; set; }
        [Required]
        public double longitude { get; set; }
    }
}
