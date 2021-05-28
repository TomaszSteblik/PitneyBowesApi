using System;
using System.ComponentModel.DataAnnotations;

namespace PitneyBowesApi.Models
{
    public class Address
    {
        //TODO: Validation
        //TODO: DTO, mapper
        [Key]
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public int BuildingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}