using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            _addressBookRepositoryStub.Setup(repository => repository.GetAll()).
                ReturnsAsync(new List<Address>().AsQueryable());
            
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
            
            _addressBookRepositoryStub.Setup(repository => repository.GetAll()).
                ReturnsAsync(expectedAddresses.AsQueryable());
            
            var controller = new AddressController(_addressBookRepositoryStub.Object, _loggerStub.Object,_mapper);
            
            //Act
            var result = await controller.GetLastAddedAddressAsync();
            
            //Assert
            result.Result.Should().BeOfType<OkResult>();
            result.Value.Should().BeEquivalentTo(_mapper.Map<AddressDto>(expectedResultValue));

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
                ZipCode = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                BuildingNumber = _random.Next(100),
                CreatedAt = DateTime.Now.Subtract(TimeSpan.FromDays(_random.Next(5,20)))
            };
        }
    }
}