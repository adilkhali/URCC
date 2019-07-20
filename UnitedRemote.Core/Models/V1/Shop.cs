﻿
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace UnitedRemote.Core.Models.V1
{
    public class Shop
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Shop's name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Shop's location")]
        [Required]
        public Point Location { get; set; }
    }
}
