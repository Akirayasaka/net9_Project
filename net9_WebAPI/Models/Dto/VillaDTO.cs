using System.ComponentModel.DataAnnotations;

namespace net9_WebAPI.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public required string Name { get; set; }
    }
}
