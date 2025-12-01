using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    /// <summary>
    /// Represents billing information for HR users
    /// </summary>
    public class BillingInfo
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = default!;
        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string? Website { get; set; }

        // Foreign Key
        public int HRId { get; set; }

        // Navigation Property
        public virtual HR? HR { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? TaxId { get; set; }
    }
}
