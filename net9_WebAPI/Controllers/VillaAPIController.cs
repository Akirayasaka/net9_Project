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
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            try
            {
                return Ok(await _db.Villas.ToListAsync());
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var result = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTO)
        {
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if (await _db.Villas.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
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
            await _db.SaveChangesAsync();
            return Ok(_db.Villas.ToList());
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) { return BadRequest(); }
            Villa? villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || villaDTO.Id != id || id == 0) { return BadRequest(); }
            Villa? villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
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
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (id == 0 || patchDTO == null) { return BadRequest(); }
            Villa? villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            VillaUpdateDTO villaUpdateDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Rate = villa.Rate,
                Sqft = villa.Sqft,
                Occupancy = villa.Occupancy,
                ImageUrl = villa.ImageUrl,
                Amenity = villa.Amenity,
            };
            patchDTO.ApplyTo(villaUpdateDTO, ModelState);
            Villa model = new()
            {
                Id = villaUpdateDTO.Id,
                Name = villaUpdateDTO.Name,
                Details = villaUpdateDTO.Details,
                Rate = villaUpdateDTO.Rate,
                Sqft= villaUpdateDTO.Sqft,
                Occupancy= villaUpdateDTO.Occupancy,
                ImageUrl = villaUpdateDTO.ImageUrl,
                Amenity= villaUpdateDTO.Amenity,
                UpdatedDate = DateTime.Now,
            };
            _db.Update(model);
            await _db.SaveChangesAsync();
            if (ModelState.IsValid) { return BadRequest(ModelState); }
            return NoContent();
        }
    }
}
