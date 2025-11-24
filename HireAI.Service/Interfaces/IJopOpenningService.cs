
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IJopOpenningService
    {
        public  Task AddJopOppenAsny(CreateJopOpeingRequestDto jopOpeingRequestDto);
    }
}
