using net9_WebAPI.Models.Dto;

namespace net9_WebAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = [
                new VillaDTO { Id = 1, Name = "AAA"},
                new VillaDTO { Id = 2, Name = "BBB"}
            ];
    }
}
