using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    public class BillingInfoDto
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = default!;

        [Required]
        public string AddressLine1 { get; set; } = default!;

        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; } = default!;

        [Required]
        public string State { get; set; } = default!;

        [Required]
        public string Country { get; set; } = "United States";

        [Required]
        public string PostalCode { get; set; } = default!;

        public string? Website { get; set; }
        public string? TaxId { get; set; }
    }

    public class CreateBillingInfoDto
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = default!;

        [Required]
        [StringLength(200)]
        public string AddressLine1 { get; set; } = default!;

        [StringLength(200)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string State { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = "United States";

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; } = default!;

        [Url]
        public string? Website { get; set; }

        public string? TaxId { get; set; }
    }
}
