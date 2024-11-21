using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
            if (id == 0)
            {
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
            int lastestID = 0;
            VillaDTO? latestRowData = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault();
            if (latestRowData != null) { lastestID = latestRowData.Id; }
            VillaDTO data = new() { Id = lastestID + 1, Name = villaDTO.Name };
            VillaStore.villaList.Add(data);
            return Ok(VillaStore.villaList);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0) { return BadRequest(); }
            VillaDTO? villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || villaDTO.Id != id || id == 0) { return BadRequest(); }
            VillaDTO? villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (id == 0 || patchDTO == null) { return BadRequest(); }
            VillaDTO? villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            patchDTO.ApplyTo(villa, ModelState);
            if (ModelState.IsValid) { return BadRequest(ModelState); }
            return NoContent();
        }
    }
}
