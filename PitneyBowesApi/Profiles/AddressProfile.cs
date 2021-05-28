using AutoMapper;
using PitneyBowesApi.DTOs;
using PitneyBowesApi.Models;

namespace PitneyBowesApi.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressDto, Address>();
            CreateMap<Address, AddressDto>();
        }
    }
}