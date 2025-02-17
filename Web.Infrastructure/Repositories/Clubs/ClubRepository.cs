using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domains.Entities;
using Web.Infrastructure.DBContext;
using Web.Infrastructure.Base;

namespace Web.Infrastructure.Repositories
{
    public class ClubRepository : BaseRepository<string, Club>, IClubRepository
    {
        public ClubRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
