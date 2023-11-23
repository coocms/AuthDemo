using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlazorWebAssemblyDemo.Shared.Models
{
    
    public class LoginRqtDto
    {
        [Display(Name = "UserName")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Account { get; set; }
        [Display(Name = "Password")]
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }
    }
}
