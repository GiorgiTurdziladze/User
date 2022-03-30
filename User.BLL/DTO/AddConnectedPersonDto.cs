using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.BLL.Enum;

namespace User.BLL.DTO
{
    public class AddConnectedPersonDto
    {
        public int PersonId { get; set; }
        public ConnectionTypeEnum ConnectionType { get; set; }
    }
}
