using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.BLL.DTO;
using User.BLL.Helpers.Interfaces;
using User.BLL.IRepository;
using User.BLL.Model;
using User.DAL.Domains;

namespace User.BLL.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TP_City> GetCity(int ID)
        {
            return await _unitOfWork.Query<TP_City>().Where(n => n.DateDeleted == null).FirstOrDefaultAsync(n => n.ID == ID);
        }

        public async Task<List<CityModel>> GetAllCities()
        {
            var result = await _unitOfWork.Query<TP_City>().Where(n => n.DateDeleted == null).OrderBy(n => n.DateCreated).ToListAsync();

            var model = new List<CityModel>();

            foreach (var i in result)
            {
                model.Add(new CityModel()
                {
                    Name = i.Name,
                    ID = i.ID
                });
            }

            return model;
        }

        public async Task<CityModel> AddCity(CreateCityDto model)
        {
            var city = new TP_City();
            city.Name = model.Name;
            city.DateCreated = DateTime.Now;

            _unitOfWork.Add(city);
            await _unitOfWork.CommitAsync();

            return new CityModel()
            {
                Name = city.Name,
                ID = city.ID
            };
        }

        public async Task<CityModel> UpdateCity(CityModel model)
        {
            var city = _unitOfWork.Query<TP_City>().FirstOrDefault(n => n.ID == model.ID);

            if (city!=null)
            {
                city.Name = model.Name;
                city.DateChanged = DateTime.Now;

                await _unitOfWork.CommitAsync();

                return new CityModel()
                {
                    ID = model.ID,
                    Name = city.Name
                };
            }

            return null;
        }

        public async Task<OperationResult> DeleteCity(int ID)
        {
            var result = new OperationResult(true);

            var city = _unitOfWork.Query<TP_City>().FirstOrDefault(n => n.ID == ID);

            if (city==null)
            {
                result.SetError("City could not be found");
                return result;
            }

            city.DateDeleted = DateTime.Now;
            await _unitOfWork.CommitAsync();
            return result;
        }
    }
}
