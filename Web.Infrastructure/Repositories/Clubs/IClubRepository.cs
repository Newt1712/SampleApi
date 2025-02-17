using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domains.Entities;
using Web.Infrastructure.Base;

namespace Web.Infrastructure.Repositories
{
    public interface IClubRepository : IBaseRepository<string, Club>
    {
    }
}
