using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp1.Model.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage ="این فیلد ضروری است")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "این فیلد ضروری است")]
        [StringLength(10,MinimumLength =6,ErrorMessage ="طول پسورد باید بین 4 و 6 کاراکتر باشد")]
        public string Password { get; set; }
    }
}
