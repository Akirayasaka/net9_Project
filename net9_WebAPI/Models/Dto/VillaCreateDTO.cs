﻿using System.ComponentModel.DataAnnotations;

namespace net9_WebAPI.Models.Dto
{
    public class VillaCreateDTO
    {
        [Required]
        [StringLength(30)]
        public required string Name { get; set; }
        public string? Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenity { get; set; }
    }
}
