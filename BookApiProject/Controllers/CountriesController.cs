using BookApiProject.Dtos;
using BookApiProject.Models;
using BookApiProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BookApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countriesDto = new List<CountryDto>();

            foreach (var item in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return Ok(countriesDto);
        }

        [HttpGet("{countryId}",Name = "GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
            {
                return NotFound();
            }
            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name=country.Name
            };

            

            return Ok(countryDto);
        }


        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            var country = _countryRepository.GetCountryOfAnAuthor(authorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody]Country countryToCreate)
        {
            if(countryToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("",$"Country {countryToCreate.Name} already exisit");
                return StatusCode(422,ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {countryToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry",new { countryId = countryToCreate.Id}, countryToCreate);
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCountry(int countryId, [FromBody]Country updateCountryInfo)
        {
            if(updateCountryInfo == null)
            {
                return BadRequest(ModelState);
            }
            if(countryId != updateCountryInfo.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExist(countryId))
            {
                return NotFound();
            }
            if(_countryRepository.IsDuplicateCountryName(countryId, updateCountryInfo.Name))
            {
                ModelState.AddModelError("", $"Country {updateCountryInfo.Name} already exisit");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.UpdateCountry(updateCountryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updateCountryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExist(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if(_countryRepository.GetAuthorsFromACountry(countryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Country {countryToDelete.Name} cannot be deleted");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong updating {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
