using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BLL.DTO;
using User.BLL.Filter;
using User.BLL.Model;
using User.DAL.Domains;

namespace User.BLL.IRepository
{
    public interface IPersonRepository
    {
        Task<TP_Person> GetPerson(int ID);
        Task<List<PersonModel>> GetAllPersons(PersonFilter filter);
        Task<TP_Person> AddPerson(AddPersonDto model);
        Task<TP_Person> UpdatePerson(UpdatePersonDto model);
        Task<ImageModel> UpdatePersonImage(ImageModel model);
        Task<OperationResult> DeletePerson(int ID);
    }
}
