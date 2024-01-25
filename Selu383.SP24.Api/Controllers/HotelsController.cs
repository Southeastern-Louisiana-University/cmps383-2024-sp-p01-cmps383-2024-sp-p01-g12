using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Hotel;

namespace Selu383.SP24.Api.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelsController : ControllerBase
    {

        private readonly DataContext dataContext;

        public HotelsController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HotelDto>> GetAllHotels()
        {
            var hotels = dataContext.Set<Hotel>()
                .Select(x => new HotelDto { 
                    Id = x.Id, 
                    Name = x.Name, 
                    Address = x.Address })
                .ToList();

            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public ActionResult<HotelDto> GetHotelById(int id)
        {
            var hotel = dataContext
                .Set<Hotel>()
                .FirstOrDefault(x => x.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDto = new HotelDto { 
                Id = hotel.Id, 
                Name = hotel.Name, 
                Address = hotel.Address 
            };

            return Ok(hotelDto);
        }

        [HttpPost]
        public ActionResult CreateHotel(HotelDto hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (hotel.Name == string.Empty)
            {
                ModelState.AddModelError("Name", "Name is required");
                return BadRequest(ModelState);
            }

            if (hotel.Name.Length > 120)
            {
                ModelState.AddModelError("Name", "The hotel name must not exceed 120 characters");
                return BadRequest(ModelState);
            }

            if (hotel.Address == string.Empty)
            {
                ModelState.AddModelError("Address", "Address is required");
                return BadRequest(ModelState);
            }

            var newHotel = new Hotel
            {
                Name = hotel.Name,
                Address = hotel.Address,
            };

            dataContext.Set<Hotel>().Add(newHotel);
            dataContext.SaveChanges();

            var createdHotelDto = new HotelDto { 
                Id = newHotel.Id, 
                Name = newHotel.Name,
                Address = newHotel.Address 
            };

            return CreatedAtAction(nameof(GetHotelById), new { id = newHotel.Id }, createdHotelDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateHotelById(int id, HotelDto updatedHotel) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            if (updatedHotel.Name == string.Empty) 
            {
                ModelState.AddModelError("Name", "Name is required");
                return BadRequest(ModelState);
            }

            if (updatedHotel.Name.Length > 120) 
            {
                ModelState.AddModelError("Name", "Name cannot be longer than 120 characters");
                return BadRequest(ModelState);
            }

            if (updatedHotel.Address == string.Empty)
            {
                ModelState.AddModelError("Address", "Address is required");
                return BadRequest(ModelState);
            }

            var existingHotel = dataContext
                .Set<Hotel>()
                .FirstOrDefault(x => x.Id == id);

            if (existingHotel == null) 
            {
                return NotFound();
            }

            existingHotel.Name = updatedHotel.Name;
            existingHotel.Address = updatedHotel.Address;

            dataContext.SaveChanges();

            var updatedHotelDto = new HotelDto {
                Id = existingHotel.Id, 
                Name = existingHotel.Name, 
                Address = existingHotel.Address 
            };

            return Ok(updatedHotelDto);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteHotel(int id) 
        {
            var hotel = dataContext
                .Set<Hotel>()
                .FirstOrDefault(x => x.Id == id);

            if (hotel == null) 
            {
                return NotFound();
            }

            dataContext.Set<Hotel>().Remove(hotel);
            dataContext.SaveChanges();

            return Ok(new HotelDto 
            {
                Name = hotel.Name,
                Address = hotel.Address,
                Id = hotel.Id
            });
        }
    }
}
