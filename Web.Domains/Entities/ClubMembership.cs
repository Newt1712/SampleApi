using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domains.Core;

namespace Web.Domains.Entities
{
    public class ClubMembership : BaseEntity<string>
    {
        public string ClubId { get; set; }
        public Club Club { get; set; }
        public int PlayerId { get; set; } // Reference to external Player Service
    }
}
