using System.ComponentModel.DataAnnotations;

namespace net9_WebAPI.Models.Dto
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public required string Name { get; set; }
        public string? Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Sqft { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public required string ImageUrl { get; set; }
        public string? Amenity { get; set; }
    }
}
