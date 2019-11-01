using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DNDAuth.Models
{
    public partial class SecurityQuestions
    {
        [Key]
        public int SecurityQuesID { get; set; }
        public string SecurityQues { get; set; }

    }
}
