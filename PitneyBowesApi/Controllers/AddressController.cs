using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        
        
        public AddressController(IAddressBookRepository addressBookRepository, ILogger<AddressController> logger,
            IMapper mapper)
        {
            _addressBookRepository = addressBookRepository;
            _logger = logger;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<AddressDto>> GetLastAddedAddressAsync()
        {
            //logowanie requesta oraz odpowiedzi
            //pobiera ostatnio dodany adres
            //zwraca noContent jesli nie ma adresow
            //zwraca ok i address z czasem najblizszym teraz
            var a = await _addressBookRepository.GetAll();
            return Ok();
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //returns created if valid object
            //returns bad request if invalid object
            //return location in header
            return Ok();
        }
    }
}