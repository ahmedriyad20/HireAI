using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class PaymentMethod
    {
     
        public Guid Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public PaymentMethod Type { get; set; } // e.g., 0=CreditCard,1=PayPal,2=MobileWallet...
        public string? Last4Digits { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
