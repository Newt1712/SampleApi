using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domains.Entities;

namespace Web.Application.Services
{
    public interface IClubService
    {
        Task<Club> CreateAsync(string name, int playerid);
        Task<Club?> GetByIdAsync(string id);
        Task<bool> AddPlayerToClubAsync(string clubId, int playerId);
        Task<bool> IsNameExistedAsync(string name);
    }
}
