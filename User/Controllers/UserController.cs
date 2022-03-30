using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using User.API.ActionFilters;
using User.BLL.DTO;
using User.BLL.Filter;
using User.BLL.IMapper;
using User.BLL.IRepository;
using User.BLL.Model;

namespace User.API.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IPersonRepository _repository;
        private readonly IPersonMapper _mapper;

        public UserController(IPersonRepository repository, IPersonMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int ID)
        {
            var result = await _repository.GetPerson(ID);

            if (result != null)
                return Ok(_mapper.GetPersonReadDto(result));
            else
                return NotFound();

        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PersonFilter filter)
        {
            var result = await _repository.GetAllPersons(filter);

            return Ok(_mapper.GetPersonReadDtoList(result));
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePerson([FromBody] AddPersonDto dto)
        {
            var result = await _repository.AddPerson(dto);

            return Ok(_mapper.GetPersonReadDto(result));
        }

        [HttpPut]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdatePerson(UpdatePersonDto dto)
        {
            var result = await _repository.UpdatePerson(dto);

            return Ok(_mapper.GetPersonReadDto(result));
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePerson(int ID)
        {
            var result = await _repository.DeletePerson(ID);

            if (!result.Result)
                return BadRequest(result.Message);
            else
                return Ok(result.Message);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadImage(ImageUploadModel model)
        {
            var file = model.Image;
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string uniquePrefix = Guid.NewGuid().ToString().Substring(0, 4);
                var fullPath = Path.Combine(pathToSave, uniquePrefix + "_" + fileName);
                var dbPath = Path.Combine(folderName, uniquePrefix + "_" + fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                _repository.UpdatePersonImage(new ImageModel()
                {
                    ImagePath = dbPath,
                    ImagePersonId = model.PersonId,
                });
                return Ok(new { dbPath });
            }
            else
            {
                return BadRequest("File is Empty");
            }
        }

        [HttpGet("{GetImage}")]
        public async Task<IActionResult> GetImage(int ID)
        {
            var result = await _repository.GetPerson(ID);

            if (result == null)
                return NotFound();

            var image = System.IO.File.OpenRead(result.ImageLink);

            return File(image, "image/jpeg");
        }
    }
}
