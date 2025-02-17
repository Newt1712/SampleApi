using AutoMapper;
using System.Numerics;
using Web.Application.Dtos;
using Web.Domains.Entities;

namespace Web.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            #region Clubs
            CreateMap<Club, ClubDto>().ReverseMap();
            #endregion

        }

    }
}
