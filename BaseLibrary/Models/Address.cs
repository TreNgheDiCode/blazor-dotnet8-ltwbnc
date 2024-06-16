using KimVinhHung.Api.Models.Address;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseLibrary.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? AddressDetail { get; set; } // Địa chỉ

        // Relationship with Ward: n - 1
        [ForeignKey("Code")]
        public string? WardId { get; set; }
        public Ward? Ward { get; set; }

        // Relationship with District: n - 1
        [ForeignKey("Code")]
        public string? DistrictId { get; set; }
        public District? District { get; set; }

        // Relationship with Province: n - 1
        [ForeignKey("Code")]
        public string? ProvinceId { get; set; }
        public Province? Province { get; set; }
    }
}
