using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PitneyBowesApi.Data;
using PitneyBowesApi.DTOs;
using PitneyBowesApi.Models;

namespace PitneyBowesApi.Controllers
{
    [ApiController]
    [Route("address")]
    public class AddressController : ControllerBase
    {

        private readonly IAddressBookRepository _addressBookRepository;
        private readonly IMapper _mapper;
        
        
        public AddressController(IAddressBookRepository addressBookRepository, IMapper mapper)
        {
            _addressBookRepository = addressBookRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<AddressDto>> GetLastAddedAddressAsync()
        {
            
            var addresses = await _addressBookRepository.GetAll();
            
            if (!addresses.Any())
                return NoContent();

            //Version 1: Position based:
            var address = addresses.Last(); 
            
            //Version 2 DateTime based:
            //var address = await addresses.FirstAsync(add => add.CreatedAt == addresses.Max(ad => ad.CreatedAt));

            return Ok(_mapper.Map<AddressDto>(address));
        }

        [HttpGet("{city}")]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAllAddressesFromCityAsync(string city)
        {

            var result = await _addressBookRepository.Find(address => address.City == city);
            
            var rAddressDtos = _mapper.Map<IEnumerable<AddressDto>>(result);
            if(rAddressDtos.Any())
                return Ok(rAddressDtos);
            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<AddressDto>> AddNewAddressAsync([FromBody] AddressDto newAddress)
        {
            var bodyReader = await Request.BodyReader.ReadAsync();

            //model validation returns bad request when model is invalid
            
            var address = _mapper.Map<Address>(newAddress);
            address.Guid = Guid.NewGuid();
            address.CreatedAt = DateTime.Now;

            await _addressBookRepository.Add(address);
            await _addressBookRepository.SaveChangesAsyc();
            
            var uri = new Uri($"https://{HttpContext.Request.Host.Value}/address/{address.City}");
            
            return Created(uri,newAddress);
        }
    }
}