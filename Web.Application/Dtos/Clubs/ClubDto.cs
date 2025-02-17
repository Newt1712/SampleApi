using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Dtos
{
    public class ClubDto
    {
        public string Id {  get; set; }
        public string Name { get; set; }

        public IList<int> Members { get; set; }
    }
}
