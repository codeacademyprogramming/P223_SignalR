using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P223Chat.ViewModels
{
    public class LoginViewModel
    {
        [StringLength(maximumLength:20),Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(maximumLength: 20), Required]
        public string Password { get; set; }
    }
}
