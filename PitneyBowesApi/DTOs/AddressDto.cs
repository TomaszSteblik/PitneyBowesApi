using System.ComponentModel.DataAnnotations;

namespace PitneyBowesApi.DTOs
{
    public class AddressDto
    {
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
        [Required]
        [Range(1,999)]
        public int BuildingNumber { get; set; }
    }
}