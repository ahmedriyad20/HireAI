using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    public class PaymentMethodDto
    {
        public string Id { get; set; } = default!;
        public string Type { get; set; } = default!; // card, etc.
        public CardInfoDto Card { get; set; } = default!;
        public bool IsDefault { get; set; }
    }

    public class CardInfoDto
    {
        public string Brand { get; set; } = default!; // visa, mastercard, etc.
        public string Last4 { get; set; } = default!;
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string Country { get; set; } = default!;

        // Helper Property
        public string MaskedNumber => $"**** **** **** {Last4}";
        public string Expiry => $"{ExpMonth:D2}/{ExpYear}";
    }
}
