using System.ComponentModel.DataAnnotations;

namespace jwtServerDemo
{
    public class UserInfo
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
