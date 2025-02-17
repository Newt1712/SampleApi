using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Web.Domains.Entities;
using Web.Infrastructure.DBContext;
using Web.Infrastructure.Messaging;
using Web.Infrastructure.Repositories;

namespace Web.Application.Services
{
    public class ClubService : IClubService
    {
        #region Props
        private readonly IClubRepository _clubRepository;
        private readonly IClubMembershipRepository _clubMembershipRepository;
        private readonly IMessageBus _messageBus;

        private readonly DatabaseContext _dbContext;
        #endregion

        #region Ctor
        public ClubService(IClubRepository clubRepository, IMessageBus messageBus, IClubMembershipRepository clubMembershipRepository, DatabaseContext dbContext)
        {
            _clubRepository = clubRepository;
            _messageBus = messageBus;
            _clubMembershipRepository = clubMembershipRepository;
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods
        public async Task<Club> CreateAsync(string name, int playerId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var club = new Club
            {
                Id = Guid.NewGuid().ToString(),
                Name = name
            };

            try
            {
                _clubRepository.Add(club);
                _clubMembershipRepository.Add(new ClubMembership
                {
                    Id = Guid.NewGuid().ToString(),
                    ClubId = club.Id,
                    PlayerId = playerId
                });
                var count = await _clubRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            return club;
        }

        public async Task<Club?> GetByIdAsync(string id)
        {
            var club = await _clubRepository.GetSingleAsync(c => c.Id == id, x => x.Memberships);
            return club;
        }

        public async Task<bool> AddPlayerToClubAsync(string clubId, int playerId)
        {
            var club = await _clubRepository.GetFirstAsync(x => x.Id == clubId);
            if (club == null) return false;

            bool playerExists = await _clubMembershipRepository.ExistAsync(cm => cm.PlayerId == playerId);
            if (playerExists) return false;

            _clubMembershipRepository.Add(new ClubMembership { ClubId = clubId, PlayerId = playerId });
            await _clubMembershipRepository.SaveChangesAsync();
            return true;
        }


        public async Task<bool> IsNameExistedAsync(string name)
        {
            return await _clubRepository.ExistAsync(x => x.Name == name);
        }
        #endregion

        #region Private Methods 
        #endregion

    }
}
