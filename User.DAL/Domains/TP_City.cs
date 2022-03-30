using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.DAL.Domains.interfaces;

namespace User.DAL.Domains
{
    public class TP_City : IDbEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateChanged { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
