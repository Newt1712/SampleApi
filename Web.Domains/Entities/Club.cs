using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domains.Core;

namespace Web.Domains.Entities
{
    public class Club : BaseEntity<string>
    {
        public string Name { get; set; }
        public ICollection<ClubMembership> Memberships { get; set; } = new List<ClubMembership>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Club() 
        { 
            Id = Guid.NewGuid().ToString();
        }
    }
}
