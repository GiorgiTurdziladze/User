using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.BLL.DTO;
using User.BLL.IRepository;
using User.BLL.Model;

namespace User.API.Controllers
{
    [Route("API/[Controller]/[Action]")]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _repository;
        private ILoggerManager _logger;

        public CityController(ICityRepository repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int ID)
        {
            var result = await _repository.GetCity(ID);
            _logger.LogInfo("object is null");

            if (result != null)
                return Ok(result);
            else
                return NotFound();

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var result = await _repository.GetAllCities();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity(CreateCityDto dto)
        {
            if (dto == null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(n => n.Value.Errors.Any()).Select(n => new {n.Key, n.Value.Errors})
                    .ToList();
                return BadRequest(errors[0].Errors[0].ErrorMessage);
            }

            try
            {
                var result = await _repository.AddCity(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCity(CityModel model)
        {
            if (model == null)
                return BadRequest("object is null");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(n => n.Value.Errors.Any()).Select(n => new { n.Key, n.Value.Errors })
                    .ToList();
                return BadRequest(errors[0].Errors[0].ErrorMessage);
            }

            try
            {
                var result = await _repository.UpdateCity(model);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCity(int ID)
        {
            var result = await _repository.DeleteCity(ID);
            if (!result.Result)
                return BadRequest(result.Message);
            else
                return Ok(result);
        }
    }
}