using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BLL.DTO;
using User.BLL.Model;
using User.DAL.Domains;

namespace User.BLL.IMapper
{
    public interface IPersonMapper
    {
        PersonReadDto GetPersonReadDto(TP_Person person);
        PersonReadDto GetPersonReadDto(PersonModel personModel);
        List<PersonReadDto> GetPersonReadDtoList(List<PersonModel> personList);
        public List<ConnectedPersonModel> GetConnectedPeopleModelList(List<TP_ConnectedPerson> people);
    }
}
