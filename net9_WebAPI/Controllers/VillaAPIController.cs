using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net9_WebAPI.Data;
using net9_WebAPI.Models.Dto;

namespace net9_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) {
                return BadRequest();
            }
            VillaDTO result = VillaStore.villaList.FirstOrDefault(x => x.Id == id) ?? new VillaDTO() { Id = -1, Name = "" };
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if (villaDTO.Id > 0)
            {
                return BadRequest();
            }
            if (VillaStore.villaList.FirstOrDefault(x => x.Name.Equals(villaDTO.Name, StringComparison.CurrentCultureIgnoreCase)) != null)
            {
                ModelState.AddModelError("ErrorMessage", "The Name Already Exist!");
                return BadRequest(ModelState);
            }
            int lastestID = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault() != null ? VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id :0;
            VillaDTO data = new() { Id = lastestID + 1, Name = villaDTO.Name };
            VillaStore.villaList.Add(data);
            return Ok(VillaStore.villaList);
        }
    }
}
