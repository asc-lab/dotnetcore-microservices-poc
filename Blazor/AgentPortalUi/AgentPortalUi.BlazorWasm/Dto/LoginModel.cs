using System.ComponentModel.DataAnnotations;

namespace AgentPortalUi.BlazorWasm.Dto
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter your login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}
