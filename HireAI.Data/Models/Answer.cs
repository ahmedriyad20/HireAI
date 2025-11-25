using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Answer
    {
        public int Id { get; set; } 
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; } = false;

        //Foreign Keys
         public int QuestionId { get; set; }
       public Question Question { get; set; }
    }
}
