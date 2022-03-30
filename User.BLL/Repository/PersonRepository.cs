using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using User.BLL.DTO;
using User.BLL.Enum;
using User.BLL.Filter;
using User.BLL.Helpers.Interfaces;
using User.BLL.IMapper;
using User.BLL.IRepository;
using User.BLL.Model;
using User.DAL.Domains;

namespace User.BLL.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonMapper _mapper;

        public PersonRepository(IUnitOfWork unitOfWork, IPersonMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TP_Person> GetPerson(int ID)
        {
            var person = await _unitOfWork.Query<TP_Person>().Where(n => n.DateDeleted == null).Include(n => n.City).FirstOrDefaultAsync(n => n.ID == ID);

            var connectedPerson = await _unitOfWork.Query<TP_ConnectedPerson>().Where(n => n.PersonId == ID).ToListAsync();

            person.ConnectedPeople = connectedPerson;
            return person;
        }

        public async Task<List<PersonModel>> GetAllPersons(PersonFilter filter)
        {
            List<TP_Person> persons = new List<TP_Person>();

            if (filter.FullSearch != null)
            {
                persons = await FullSearch(filter);
            }
            else if (filter.QuickSearch != null)
            {
                persons = await QuickSearch(filter);
            }
            else
            {
                persons = await (from p in _unitOfWork.Query<TP_Person>()
                                 where p.DateDeleted == null
                                 orderby p.DateCreated descending
                                 select p).Include(n => n.City).ToListAsync();
            }

            persons = persons.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToList();

            var list = new List<PersonModel>();

            foreach (var p in persons)
            {
                var person = new PersonModel();

                person.ID = p.ID;
                person.BirthDate = p.BirthDate;
                person.CityId = p.CityId != null ? (int)p.CityId : 0;
                person.CityName = p.City != null ? p.City.Name : null;
                person.Firstname = p.Firstname;
                person.Lastname = p.Lastname;
                person.Gender = (GenderEnum)p.Gender;
                person.PhoneNumber = p.PhoneNumber;
                person.PhoneNumberType = (PhoneNumTypeEnum)p.PhoneNumberType;
                person.ImageLink = p.ImageLink;
                person.IdNumber = p.IdNumber;

                var connected = await _unitOfWork.Query<TP_ConnectedPerson>().Where(n => n.PersonId == p.ID).ToListAsync();

                if (connected != null && connected.Count > 0)
                {
                    person.ConnectedPeople = _mapper.GetConnectedPeopleModelList(connected);
                }
                list.Add(person);
            }

            return list;
        }


        public async Task<TP_Person> AddPerson(AddPersonDto model)
        {
            var person = new TP_Person();

            person.BirthDate = model.BirthDate;
            person.CityId = model.CityId == 0 ? (int?)null : model.CityId;
            person.Firstname = model.Firstname;
            model.Lastname = model.Lastname;
            person.Gender = (int)model.Gender;
            person.PhoneNumber = model.PhoneNumber;
            person.PhoneNumberType = (int)model.PhoneNumberType;
            person.ImageLink = model.ImageLink;
            person.DateCreated = DateTime.Now;

            if (model != null && model.ConnectedPeople.Count > 0)
            {
                person.ConnectedPeople = new List<TP_ConnectedPerson>();

                foreach (var c in model.ConnectedPeople)
                {
                    var toAdd = await GetPerson(c.PersonId);

                    if (toAdd == null)
                    {
                        throw new Exception("Connected User could not be found");
                    }

                    person.ConnectedPeople.Add(new TP_ConnectedPerson()
                    {
                        ConnectedPersonId = c.PersonId,
                        ConnectionType = (int)c.ConnectionType,
                        DateCreated = DateTime.Now
                    });
                }
            }

            _unitOfWork.Add(person);
            await _unitOfWork.CommitAsync();
            return person;
        }

        public async Task<TP_Person> UpdatePerson(UpdatePersonDto model)
        {
            var existing = _unitOfWork.Query<TP_Person>().FirstOrDefault(n => n.ID == model.ID);

            if (existing != null)
            {
                existing.Firstname = model.Firstname;
                existing.Lastname = model.Lastname;
                existing.PhoneNumber = model.PhoneNumber;
                existing.PhoneNumberType = (int)model.PhoneNumberType;
                existing.CityId = model.CityId == 0 ? (int?)null : model.CityId;
                existing.Gender = (int)model.Gender;
                existing.BirthDate = model.BirthDate;

                existing.DateChanged = DateTime.Now;

                await _unitOfWork.CommitAsync();
                return existing;
            }

            return null;
        }

        public async Task<ImageModel> UpdatePersonImage(ImageModel model)
        {
            var existing = _unitOfWork.Query<TP_Person>().FirstOrDefault(n => n.ID == model.ImagePersonId);

            if (existing == null)
                throw new Exception("User Not Found");

            existing.ImageLink = model.ImagePath;

            await _unitOfWork.CommitAsync();

            return model;
        }

        public async Task<OperationResult> DeletePerson(int ID)
        {
            var result = new OperationResult(true);
            var person = _unitOfWork.Query<TP_Person>().FirstOrDefault(n => n.ID == ID);

            if (person == null)
            {
                result.SetError("Person could not be found");
                return result;
            }

            person.DateDeleted = DateTime.Now;

            await _unitOfWork.CommitAsync();
            return result;
        }

        private async Task<List<TP_Person>> FullSearch(PersonFilter filter)
        {
            var people = await (from p in _unitOfWork.Query<TP_Person>()
                                where p.DateDeleted == null &&
                                (EF.Functions.Like(p.Firstname, $"%{filter.QuickSearch}%")
                                || EF.Functions.Like(p.Lastname, $"%{filter.QuickSearch}%")
                                || EF.Functions.Like(p.IdNumber, $"%{filter.QuickSearch}%"))
                                orderby p.DateCreated descending
                                select p).Include(n => n.City).ToListAsync();
            return people;
        }

        private async Task<List<TP_Person>> QuickSearch(PersonFilter filter)
        {
            PropertyInfo[] properties = typeof(TP_Person).GetProperties();

            var allRecords = await (from p in _unitOfWork.Query<TP_Person>()
                                    where p.DateDeleted == null
                                    orderby p.DateCreated descending
                                    select p).Include(n => n.City).ToListAsync();
            
            var list = new List<TP_Person>();
            foreach (var a in allRecords)
            {
                if (a.City != null && a.City.Name.ToLower().Contains(filter.FullSearch.ToLower()))
                {
                    list.Add(a);
                    continue;
                }

                foreach (PropertyInfo p in properties)
                {
                    var val = p.GetValue(a);
                    if (val != null && val.ToString().ToLower().Contains(filter.FullSearch.ToLower()))
                    {
                        list.Add(a);
                        break;
                    }
                }
            }
            return list;
        }
    }
}
