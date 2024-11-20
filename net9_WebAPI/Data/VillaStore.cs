using net9_WebAPI.Models.Dto;

namespace net9_WebAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = [
                new VillaDTO { Id = 1, Name = "AAA", Occupancy = 100, Sqft = 2},
                new VillaDTO { Id = 2, Name = "BBB", Occupancy = 200, Sqft = 3}
            ];
    }
}
