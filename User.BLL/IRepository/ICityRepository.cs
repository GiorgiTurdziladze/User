using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BLL.DTO;
using User.BLL.Model;
using User.DAL.Domains;

namespace User.BLL.IRepository
{
    public interface ICityRepository
    {
        Task<TP_City> GetCity(int ID);
        Task<List<CityModel>> GetAllCities();
        Task<CityModel> AddCity(CreateCityDto model);
        Task<CityModel> UpdateCity(CityModel model);
        Task<OperationResult> DeleteCity(int ID);
    }
}
