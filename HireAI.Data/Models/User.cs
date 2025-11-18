
﻿using HireAI.Data.Helpers.Enums;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool IsPremium { get; set; } = false;
        public AccountType preminumPlans { get; set; } = AccountType.Free;
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}


