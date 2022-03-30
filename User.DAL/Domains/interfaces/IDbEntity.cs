using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.DAL.Domains.interfaces
{
    public interface IDbEntity
    {
        int ID { get; set; }

        DateTime DateCreated { get; set; }
        DateTime? DateChanged { get; set; }
        DateTime? DateDeleted { get; set; }
    }
}
