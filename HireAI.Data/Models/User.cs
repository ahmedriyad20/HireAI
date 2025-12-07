
﻿using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{

    public abstract  class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public enRole Role { get; set; }
        public bool IsPremium { get; set; } = false;
        public string? Phone { get; set; }
        public string? Bio { get; set; }
        public string? Title { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


