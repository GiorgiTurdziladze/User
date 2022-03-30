using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.BLL.DTO
{
    public class UpdateCityDto
    {
        [Required]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
