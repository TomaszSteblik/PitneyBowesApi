using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using PitneyBowesApi.Controllers;
using PitneyBowesApi.Data;
using PitneyBowesApi.DTOs;
using PitneyBowesApi.Models;
using PitneyBowesApi.Profiles;
using Xunit;

namespace PitneyBowesTests
{
    public class AddressControllerUnitTests
    {
        private readonly Mock<IAddressBookRepository> _addressBookRepositoryStub = new();
        private readonly Mock<ILogger<AddressController>> _loggerStub = new();

        private readonly IMapper _mapper =
            new Mapper(new MapperConfiguration(config => config.AddProfile(new AddressProfile())));
        private readonly Random _random = new();
        
        //UnitOfWork_StateUnderTest_ExpectedBehavior
        
        [Fact]
        public async Task GetLastAddedAddressAsync_WithNoAddresses_ReturnsNoContent()
        {
            //Arrange
            _addressBookRepositoryStub.Setup(repository => repository.GetAll())
                .ReturnsAsync(new List<Address>().AsQueryable());
            
            var controller = new AddressController(_addressBookRepositoryStub.Object, _loggerStub.Object,_mapper);
            
            //Act
            var response = await controller.GetLastAddedAddressAsync();
            
            //Assert
            response.Result.Should().BeOfType<NoContentResult>();
            
        }

        [Fact]
        public async Task GetLastAddedAddressAsync_WithExistingAddresses_ReturnsNewestAddress()
        {
            //Arrange
            var expectedAddresses = new List<Address>();

            for (int i = 0; i < 10; i++)
            {
                expectedAddresses.Add(GenerateAddress());
            }
            expectedAddresses[^1].CreatedAt = DateTime.Now;
            var expectedResultValue = expectedAddresses[^1];

            var mockQueryable = expectedAddresses.AsQueryable();
            
            _addressBookRepositoryStub.Setup(repository => repository.GetAll())
                .ReturnsAsync(mockQueryable.BuildMock().Object);
            
            var controller = new AddressController(_addressBookRepositoryStub.Object, _loggerStub.Object,_mapper);
            
            //Act
            var actionResult = await controller.GetLastAddedAddressAsync();
            
            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result as OkObjectResult;
            result?.Value.Should().BeEquivalentTo(_mapper.Map<AddressDto>(expectedResultValue));

        }

        [Fact]
        public async Task GetAllAddressesFromCityAsync_WithNoAddressesWithGivenCity_ReturnsNoContent()
        {
            //Arrange
            var searchedCity = "Bielsko-Biała";
            
            _addressBookRepositoryStub
                .Setup(repository => repository.Find(x=> x.City==searchedCity))
                .ReturnsAsync(new List<Address>().AsQueryable);
            
            var controller = new AddressController(_addressBookRepositoryStub.Object, _loggerStub.Object, _mapper);
            
            //Act
            var result = await controller.GetAllAddressesFromCityAsync(searchedCity);
            
            //Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetAllAddressesFromCityAsync_WithMatchingCity_ReturnsArrayOfAddresses()
        {
            //Arrange
            var searchedCity = "Bielsko-Biała";

            var expectedAddresses = new List<Address>();

            for (int i = 0; i < 10; i++)
            {
                expectedAddresses.Add(GenerateAddress());
                expectedAddresses[i].City = searchedCity;
            }

            _addressBookRepositoryStub
                .Setup(repository => repository.Find(x=> x.City==searchedCity))
                .ReturnsAsync(expectedAddresses.AsQueryable);            
            
            var controller = new AddressController(_addressBookRepositoryStub.Object, _loggerStub.Object, _mapper);
            //Act
            var actionResult = await controller.GetAllAddressesFromCityAsync(searchedCity);

            //Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result as OkObjectResult;
            result?.Value.Should().BeEquivalentTo(_mapper.Map<List<AddressDto>>(expectedAddresses));
        }

        [Fact]
        public async Task AddNewAddressAsync_WithValidAddress_ReturnsCreatedAddress()
        {
            //Arange
            var newAddressDto = GenerateAddressDto();

            var controller = new AddressController(_addressBookRepositoryStub.Object, _loggerStub.Object, _mapper)
            {
                ControllerContext = new()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        Request = { Host = new HostString("fakeUri:5001")}
                    }
                }
            };
            
            //Act

            var actionResult = await controller.AddNewAddressAsync(newAddressDto);
            
            //Assert

            actionResult.Result.Should().BeOfType<CreatedResult>();
            var result = actionResult.Result as CreatedResult;
            result?.Value.Should().BeEquivalentTo(newAddressDto);
            
        }

        private Address GenerateAddress()
        {
            return new Address()
            {
                Guid = Guid.NewGuid(),
                City = Guid.NewGuid().ToString(),
                Street = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString().Substring(0,5),
                PhoneNumber = Guid.NewGuid().ToString().Substring(0,9),
                BuildingNumber = _random.Next(2,100),
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(_random.Next(5,20)))
            };
        }

        private AddressDto GenerateAddressDto()
        {
            return new AddressDto()
            {
                City = Guid.NewGuid().ToString(),
                Street = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString().Substring(0,5),
                PhoneNumber = Guid.NewGuid().ToString().Substring(0,9),
                BuildingNumber = _random.Next(2,100),
            };
        }
    }
}