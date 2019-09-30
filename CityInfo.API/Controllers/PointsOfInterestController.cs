using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController:Controller
    {
        [HttpGet("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city =
                CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city is null)
                return NotFound();

            return Ok(city?.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsOfInterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city =
                CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city is null)
                return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterest == null)
                return NotFound();

            return Ok(pointOfInterest);

        }

        [HttpPost("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            if (pointOfInterestForCreation == null)
                return BadRequest();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (existingCity == null)
                return NotFound();

            var newId = CitiesDataStore.Current.Cities
                            .SelectMany(c => c.PointsOfInterest).Max(p => p.Id) + 1;


            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = newId,
                Name = pointOfInterestForCreation.Name,
                Description = pointOfInterestForCreation.Description
            };

            existingCity.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",new {cityId=cityId, id= newId }, finalPointOfInterest);
        }

        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            if (pointOfInterestForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCity = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (existingCity == null)
                return NotFound();

            var pointOfInterestDto = existingCity.PointsOfInterest.FirstOrDefault(x => x.Id == id);

            if (pointOfInterestDto is null)
                return NotFound();

            pointOfInterestDto.Name = pointOfInterestForCreation.Name;
            pointOfInterestDto.Description = pointOfInterestForCreation.Description;

            return NoContent();
        }
    }
}
