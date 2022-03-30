using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BLL.Enum;
using User.DAL.Annotations;

namespace User.BLL.DTO
{
    public class AddPersonDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [NameValidation.PersonNameValidation(ErrorMessage = "Invalid Firstname")]
        public string Firstname { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [NameValidation.PersonNameValidation(ErrorMessage = "Invalid Lastname")]

        public string Lastname { get; set; }

        public GenderEnum Gender { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only Numbers")]
        public string IdNumber { get; set; }

        [Required]

        [AgeValidation(ErrorMessage = "Invalid Age")]
        public DateTime BirthDate { get; set; }


        public int CityId { get; set; }

        [MinLength(4)]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
        public PhoneNumTypeEnum PhoneNumberType { get; set; }

        public string ImageLink { get; set; }



        public List<AddConnectedPersonDto> ConnectedPeople { get; set; }
    }
}
