using System;
using System.ComponentModel.DataAnnotations;

namespace PitneyBowesApi.Models
{
    public class Address
    {
        [Key]
        public Guid Guid { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [StringLength(5)]
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Range(1,999)]
        [Required]
        public int BuildingNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}