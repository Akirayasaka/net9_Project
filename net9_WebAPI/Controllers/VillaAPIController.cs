using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net9_WebAPI.Data;
using net9_WebAPI.Models;
using net9_WebAPI.Models.Dto;

namespace net9_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController(ApplicationDbContext _db) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            try
            {
                return Ok(_db.Villas.ToList());
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
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
            var result = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
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
            if (_db.Villas.FirstOrDefault(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("ErrorMessage", "The Name Already Exist!");
                return BadRequest(ModelState);
            }
            Villa newData = new() {
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                Occupancy = villaDTO.Occupancy,
                Amenity = villaDTO.Amenity,
                ImageUrl = villaDTO.ImageUrl,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            _db.Villas.Add(newData);
            _db.SaveChanges();
            return Ok(_db.Villas.ToList());
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0) { return BadRequest(); }
            Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || villaDTO.Id != id || id == 0) { return BadRequest(); }
            Villa? villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            villa.Name = villaDTO.Name;
            villa.Details = villaDTO.Details;
            villa.Rate = villaDTO.Rate;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            villa.Amenity = villaDTO.Amenity;
            villa.ImageUrl = villaDTO.ImageUrl;
            villa.UpdatedDate = DateTime.Now;
            _db.Villas.Update(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (id == 0 || patchDTO == null) { return BadRequest(); }
            Villa? villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            //patchDTO.ApplyTo(villa, ModelState);
            //if (ModelState.IsValid) { return BadRequest(ModelState); }
            return NoContent();
        }
    }
}
